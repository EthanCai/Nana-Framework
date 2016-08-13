using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Wintellect.Threading.AsyncProgModel;

namespace Nana.Framework.Async
{
    public class AsyncTaskDispatcher
    {
        private Dictionary<object, AsyncTask> m_tasks;
        private AsyncEnumerator m_asyncEnumerator;
        private Dictionary<object, Exception> m_taskExceptions;
        public bool CancelOnFailure { get; private set; }
        public DispatchStatus Status { get; private set; }

        private int m_runningTaskCount;

        public AsyncTaskDispatcher()
            : this(false)
        {
        }

        public AsyncTaskDispatcher(bool cancelOnFailure)
        {
            this.CancelOnFailure = cancelOnFailure;
            this.m_tasks = new Dictionary<object, AsyncTask>();
            this.Status = DispatchStatus.NotStarted;
        }

        public void RegisterTasks(params Action[] userFuncs)
        {
            foreach (Action action in userFuncs)
            {
                RegisterTask(action);
            }
        }
        public AsyncTask RegisterTask(Action userFunc)
        {
         
              var  id = "m_taskID_" + this.m_tasks.Count;
            
            return RegisterTask(id, userFunc);
        }

        public AsyncTask RegisterTask(
            object id,
            Action userFunc)
        {
            return this.RegisterTask(id, null, (cb, state, context) => (userFunc.BeginInvoke(cb, state)), (ar, cancelling, context) =>
                                                                                                               {
                                                                                                                   //userFunc();
                                                                                                                   userFunc.EndInvoke(ar);
                                                                                                               }, null);
        }

        public AsyncTask RegisterTask(
            object id,
            Func<AsyncCallback, object, object, IAsyncResult> begin,
            Action<IAsyncResult, bool, object> end)
        {
            return this.RegisterTask(id, null, begin, end, null);
        }

        public AsyncTask RegisterTask(
            object id,
            Func<AsyncCallback, object, object, IAsyncResult> begin,
            Action<IAsyncResult, bool, object> end,
            params object[] dependencies)
        {
            return this.RegisterTask(id, null, begin, end, null, dependencies);
        }

        public AsyncTask RegisterTask(
            object id,
            Func<object, DispatchPolicy> predicate,
            Func<AsyncCallback, object, object, IAsyncResult> begin,
            Action<IAsyncResult, bool, object> end,
            object context,
            params object[] dependencies)
        {
            lock (this.m_tasks)
            {
                if (this.Status != DispatchStatus.NotStarted)
                {
                    throw new InvalidOperationException("Task can only be registered before dispatching.");
                }

                this.CheckRegisterTaskArgs(id, begin, end, dependencies);

                AsyncTask task = new AsyncTask(id, predicate, begin, end, context, dependencies.Select(d => this.m_tasks[d]));
                this.m_tasks.Add(id, task);

                return task;
            }
        }

        private void CheckRegisterTaskArgs(
            object id,
            Func<AsyncCallback, object, object, IAsyncResult> begin,
            Action<IAsyncResult, bool, object> end,
            object[] dependencies)
        {
        
            if(id==null) throw new ArgumentNullException("id");

            if (begin == null) throw new ArgumentNullException("begin");
            if (end == null) throw new ArgumentNullException("end");
            if (dependencies == null) throw new ArgumentNullException("dependencies");

            foreach (var taskId in dependencies)
            {
                if (taskId == null)
                {
                    throw new ArgumentException("None of the dependencies can be none.");
                }

                if (!this.m_tasks.ContainsKey(taskId))
                {
                    throw new ArgumentOutOfRangeException("Task id mismatched: " + taskId);
                }
            }
        }

        public IAsyncResult BeginDispatch(AsyncCallback asyncCallback, object asyncState)
        {
            lock (this.m_tasks)
            {
                if (this.Status != DispatchStatus.NotStarted)
                {
                    throw new InvalidOperationException("An AsyncTaskDispatcher can be started only once.");
                }

                this.Status = DispatchStatus.Dispatching;
            }

            this.m_taskExceptions = new Dictionary<object, Exception>();
            var taskToStart = this.m_tasks.Values.Where(t => t.DependenciesSucceeded).ToList();
            IEnumerator<int> enumerator = this.GetWorkerEnumerator(taskToStart);

            this.m_asyncEnumerator = new AsyncEnumerator();
            return this.m_asyncEnumerator.BeginExecute(enumerator, asyncCallback, asyncState);
        }

        public void EndDispatch(IAsyncResult asyncResult)
        {
            this.m_asyncEnumerator.EndExecute(asyncResult);
            if (this.m_taskExceptions.Count > 0)
            {
                throw new DispatchException(this.GetTaskSnapshots(), this.m_taskExceptions);
            }
        }

        public Dictionary<object, AsyncTask> GetTaskSnapshots()
        {
            lock (this.m_tasks)
            {
                return this.m_tasks.ToDictionary(
                    p => p.Key,
                    p => p.Value.MakeSnapshot());
            }
        }

        public AsyncTask GetTask(object id)
        {
            AsyncTask task;
            lock (this.m_tasks)
            {
                return this.m_tasks.TryGetValue(id, out task) ? task : null;
            }
        }

        public IEnumerator<int> GetWorkerEnumerator(IEnumerable<AsyncTask> tasksToStart)
        {
            this.m_runningTaskCount = 0;

            foreach (AsyncTask task in tasksToStart)
            {
                try
                {
                    this.Start(task);
                }
                catch (AsyncTaskException ex)
                {
                    if (this.HandleTaskFailure(ex.Task, ex.InnerException))
                    {
                        this.Status = DispatchStatus.Finished;
                        yield break;
                    }

                    break;
                }
            }

            while (this.m_runningTaskCount > 0)
            {
                yield return 1;

                this.m_runningTaskCount--;
                IAsyncResult asyncResult = this.m_asyncEnumerator.DequeueAsyncResult();
                AsyncTask finishedTask = (AsyncTask) asyncResult.AsyncState;

                try
                {
                    finishedTask.End(asyncResult, false, finishedTask.Context);
                    finishedTask.Status = AsyncTaskStatus.Succeeded;

                    if (this.Status == DispatchStatus.Dispatching)
                    {
                        this.StartSuccessors(finishedTask);
                    }

                    finishedTask.Close();
                }
                catch (AsyncTaskException ex)
                {
                    if (this.HandleTaskFailure(ex.Task, ex.InnerException)) break;
                }
                catch (Exception ex)
                {
                    if (this.HandleTaskFailure(finishedTask, ex)) break;
                }
            }

            this.Status = DispatchStatus.Finished;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ex"></param>
        /// <returns>true for cancel dispatching immediately</returns>
        private bool HandleTaskFailure(AsyncTask task, Exception ex)
        {
            this.m_taskExceptions.Add(task.Id, ex);
            task.Status = AsyncTaskStatus.Failed;
            task.Close();

            if (this.CancelOnFailure)
            {
                lock (this.m_tasks)
                {
                    var runningTasks = this.m_tasks.Values.Where(t => t.Status == AsyncTaskStatus.Executing);
                    foreach (AsyncTask t in runningTasks)
                    {
                        t.Status = AsyncTaskStatus.Cancelling;
                    }
                }

                this.Status = DispatchStatus.Cancelling;
                this.m_asyncEnumerator.Cancel(null);
                return true;
            }
            else
            {
                this.Status = DispatchStatus.Waiting;
                return false;
            }
        }

        private void Start(AsyncTask task)
        {
            DispatchPolicy policy;
            try
            {
                policy = task.Predicate == null ? DispatchPolicy.Normal : task.Predicate(task.Context);
            }
            catch (Exception ex)
            {
                throw new AsyncTaskException(task, ex);
            }

            if (policy == DispatchPolicy.Normal)
            {
                try
                {
                    task.Begin(this.m_asyncEnumerator.EndVoid(0, this.CancelTask), task, task.Context);

                    this.m_runningTaskCount++;
                    task.Status = AsyncTaskStatus.Executing;
                }
                catch (Exception ex)
                {
                    throw new AsyncTaskException(task, ex);
                }
            }
            else if (policy == DispatchPolicy.MarkAsCancelled)
            {
                task.Status = AsyncTaskStatus.MarkedAsCancelled;
                task.Close();
            }
            else // policy == DispatchPolicy.Succeeded
            {
                task.Status = AsyncTaskStatus.MarkedAsSucceeded;
                this.StartSuccessors(task);
                task.Close();
            }
        }

        private void StartSuccessors(AsyncTask task)
        {
            Func<AsyncTask, bool> predicate = t =>
                                              t.Status == AsyncTaskStatus.Pending &&
                                              t.DependenciesSucceeded;

            foreach (AsyncTask successor in task.Successors.Where(predicate))
            {
                this.Start(successor);
            }
        }

        private void CancelTask(IAsyncResult asyncResult)
        {
            AsyncTask task = (AsyncTask) asyncResult.AsyncState;
            try
            {
                task.End(asyncResult, true, task.Context);
            }
            catch
            {
            }
            finally
            {
                this.m_runningTaskCount--;
                task.Status = AsyncTaskStatus.Cancelled;
                task.Close();
            }
        }
    }
}