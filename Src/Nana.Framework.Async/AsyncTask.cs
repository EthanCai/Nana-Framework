using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nana.Framework.Async
{
    public class AsyncTask
    {
        private AsyncTask[] m_dependencies;
        private HashSet<AsyncTask> m_successors;

        public object Id { get; private set; }
        public object Context { get; private set; }
        public AsyncTaskStatus Status { get; internal set; }
        internal Func<object, DispatchPolicy> Predicate { get; private set; }

        //第一个参数是callBack 默认不管
        //第二个参数为 AsynTask本身
        //第三个参数为 context 转入的，
        //第４个参数为返回类型　
        //   task.Begin(this.m_asyncEnumerator.EndVoid(0, this.CancelTask), task, task.Context);
        internal Func<AsyncCallback, object, object, IAsyncResult> Begin { get; private set; }
        
        // task.End(asyncResult, true, task.Context);
        internal Action<IAsyncResult, bool, object> End { get; private set; }

        private AsyncTask() { }

        internal AsyncTask(
            object id,
            Func<object, DispatchPolicy> predicate,
            Func<AsyncCallback, object, object, IAsyncResult> begin,
            Action<IAsyncResult, bool, object> end,
            object context,
            IEnumerable<AsyncTask> dependencies)
        {
            this.Id = id;
            this.Predicate = predicate;
            this.Begin = begin;
            this.End = end;
            this.Context = context;
            this.Status = AsyncTaskStatus.Pending;
            this.m_successors = new HashSet<AsyncTask>();

            this.m_dependencies = dependencies.Where(d => d != null).Distinct().ToArray();
            foreach (var task in this.m_dependencies)
            {
                task.m_successors.Add(this);
            }
        }

        internal IEnumerable<AsyncTask> Successors
        {
            get
            {
                return this.m_successors;
            }
        }

        internal bool DependenciesSucceeded
        {
            get
            {
                return this.m_dependencies.All(d =>
                    d.Status == AsyncTaskStatus.Succeeded ||
                    d.Status == AsyncTaskStatus.MarkedAsSucceeded);
            }
        }

        internal void Close()
        {
            this.Predicate = null;
            this.Begin = null;
            this.End = null;
            this.m_dependencies = null;
            this.m_successors = null;
        }

        internal AsyncTask MakeSnapshot()
        {
            return new AsyncTask
            {
                Id = this.Id,
                Status = this.Status
            };
        }
    }
}
