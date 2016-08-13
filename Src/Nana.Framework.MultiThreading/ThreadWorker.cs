using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wintellect.Threading.AsyncProgModel;

namespace Nana.Framework.MultiThreading
{
    //=================================================================================================
    /// <summary>
    /// dongbin.li
    /// 多线程任务执行器
    /// 传进来一个Ienumberable列表
    /// 一个用来工作的function
    /// 抱歉，暂时不支持返回值
    /// 如果需要返回值请使用ThreadWorker<T, TResult>
    /// </summary>
    public class ThreadWorker<T>
    {
        private const int DefaultThreadsNum = 10;
        public int _threadNum;
        private delegate void JobWorkHandler(IEnumerable<T> jobSource, Action<T> _jobAction);

        private IEnumerable<T> jobSource;
        private Action<T> _jobAction;

        public ThreadWorker(IEnumerable<T> source, Action<T> action)
            : this(source, DefaultThreadsNum, action)
        {
        }

        public ThreadWorker(IEnumerable<T> source, int threadNum, Action<T> action)
        {
            jobSource = source;
            _jobAction = action;
            _threadNum = threadNum;
        }

        public void Do()
        {
            AsyncEnumerator ae = new AsyncEnumerator();
            ae.EndExecute(ae.BeginExecute(SetValueToMemCacheEnumerator(ae, jobSource, _jobAction), null));
        }

        private void DoOneJob(IEnumerable<T> jobSource, Action<T> _jobAction)
        {
            foreach (var job in jobSource)
            {
                _jobAction.Invoke(job);
            }
        }

        private IEnumerator<int> SetValueToMemCacheEnumerator(AsyncEnumerator ae, IEnumerable<T> jobSource, Action<T> _jobAction)
        {
            int totalCount = jobSource.Count();
            int perThread = totalCount / _threadNum;//每个线程的工作数目
            if (perThread == 0)
                perThread = totalCount;
            //实际的工人数目
            int realWorker = 0;
            //开始分配
            int skipCount = 0;
            IEnumerable<T> oneJob = jobSource.Skip(skipCount).Take(perThread);

            while (oneJob.Count() != 0)
            {
                JobWorkHandler handler = new ThreadWorker<T>.JobWorkHandler(DoOneJob);
                handler.BeginInvoke(oneJob, _jobAction, ae.End(), handler);
                realWorker++;
                skipCount += perThread;
                //给下一个工人分配
                oneJob = jobSource.Skip(skipCount).Take(perThread);
            }

            //交工
            yield return realWorker;
            //验收
            for (int i = 0; i < realWorker; i++)
            {
                IAsyncResult asyncResualt = ae.DequeueAsyncResult();
                JobWorkHandler handler = asyncResualt.AsyncState as JobWorkHandler;
                handler.EndInvoke(asyncResualt);
            }

        }


    }

    //====================================================================================================
    /// <summary>
    /// dongbin.li
    /// 多线程任务执行器
    /// 传进来一个Ienumberable列表，告诉我入参是什么，每一个function的出参是什么
    /// 一个用来工作的function
    /// 将得到一个dic字典返回值，每个值和结果1--1对应
    /// Demo:
    /// ThreadWorker<int,int> worker = new ThreadWorker<int,int>(new List<int>() { 1, 2, 3, 4 }, 2, new Func<int, int>(a => { return a+1; }));
    /// Dictionary<int, int> results = worker.Do();
    /// the result would be like this
    /// KEY VALUE
    /// 1   2
    /// 2   3
    /// 3   4
    /// 4   5
    /// </summary>
    public class ThreadWorker<T, TResult>
    {
        private const int DefaultThreadsNum = 10;
        public int _threadNum;
        private delegate Dictionary<T, TResult> JobWorkHandler(IEnumerable<T> jobSource, Func<T, TResult> _jobAction);

        private IEnumerable<T> jobSource;

        public ThreadWorker(IEnumerable<T> source, Func<T, TResult> func)
            : this(source, DefaultThreadsNum, func)
        {
}

        private Func<T, TResult> _func;
        public ThreadWorker(IEnumerable<T> source, int threadNum, Func<T, TResult> func)
        {
            jobSource = source;
            _func = func;
            _threadNum = threadNum;
        }
        public Dictionary<T, TResult> Do()
        {
            AsyncEnumerator<Dictionary<T, TResult>> ae = new AsyncEnumerator<Dictionary<T, TResult>>();
            return ae.EndExecute(ae.BeginExecute(MyTask(ae, jobSource, _func), null));
        }

        private Dictionary<T, TResult> DoOneJob(IEnumerable<T> jobSource, Func<T, TResult> func)
        {
            Dictionary<T, TResult> one_job_result = new Dictionary<T, TResult>();
            foreach (var job in jobSource)
            {
                var result = func.Invoke(job);
                //add to final result
                if (!one_job_result.ContainsKey(job))
                    one_job_result.Add(job, result);
            }
            return one_job_result;
        }

        private IEnumerator<int> MyTask(AsyncEnumerator<Dictionary<T, TResult>> ae, IEnumerable<T> jobSource, Func<T, TResult> _jobAction)
        {
            int totalCount = jobSource.Count();
            int perThread = totalCount / _threadNum;//每个线程的工作数目
            if (perThread == 0)
                perThread = totalCount;
            //实际的工人数目
            int realWorker = 0;
            //开始分配
            int skipCount = 0;
            IEnumerable<T> oneJob = jobSource.Skip(skipCount).Take(perThread);

            while (oneJob.Count() != 0)
            {
                JobWorkHandler handler = new ThreadWorker<T, TResult>.JobWorkHandler(DoOneJob);
                handler.BeginInvoke(oneJob, _func, ae.End(), handler);
                realWorker++;
                skipCount += perThread;
                //给下一个工人分配
                oneJob = jobSource.Skip(skipCount).Take(perThread);
            }

            //交工
            yield return realWorker;
            //验收
            //here is the results
            Dictionary<T, TResult> finalResuls = new Dictionary<T, TResult>();
            for (int i = 0; i < realWorker; i++)
            {
                IAsyncResult asyncResualt = ae.DequeueAsyncResult();
                JobWorkHandler handler = asyncResualt.AsyncState as JobWorkHandler;
                var rr = handler.EndInvoke(asyncResualt);
                if (rr != null && rr.Count > 0)
                {
                    foreach (var k in rr.Keys)
                    {
                        if (!finalResuls.ContainsKey(k))
                            finalResuls.Add(k, rr[k]);
                    }
                }
            }
            ae.Result = finalResuls;

        }

    }


    /// <summary>
    /// ThreadWorker的改进类，支持get(keys)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    public class ThreadWorkerForGet<T, TResult>
    {
        private const int DefaultThreadsNum = 10;
        public int _threadNum;
        private delegate Dictionary<T, TResult> JobWorkGetHandler(IEnumerable<T> jobSource, Func<List<T>, List<TResult>> _jobAction);

        private IEnumerable<T> jobSource;

        public ThreadWorkerForGet(IEnumerable<T> source, Func<List<T>, List<TResult>> func)
            : this(source, DefaultThreadsNum, func)
        {
}

        private Func<List<T>, List<TResult>> _func;
        public ThreadWorkerForGet(IEnumerable<T> source, int threadNum, Func<List<T>, List<TResult>> func)
        {
            jobSource = source;
            _func = func;
            _threadNum = threadNum;
        }
        public Dictionary<T, TResult> Do()
        {
            AsyncEnumerator<Dictionary<T, TResult>> ae = new AsyncEnumerator<Dictionary<T, TResult>>();
            return ae.EndExecute(ae.BeginExecute(MyTask(ae, jobSource, _func), null));
        }

        private Dictionary<T, TResult> DoOneJob(IEnumerable<T> jobSource, Func<List<T>, List<TResult>> func)
        {
            List<T> jobSourceList = jobSource.ToList();
            Dictionary<T, TResult> oneJobResult = new Dictionary<T, TResult>();
            List<TResult> retobj = func.Invoke(jobSourceList);


            for (int i = 0; i < jobSourceList.Count(); i++)
            {
                if (!oneJobResult.ContainsKey(jobSourceList[i]))
                   oneJobResult.Add(jobSourceList[i], retobj[i]);
            }
           
            return oneJobResult;
        }

        private IEnumerator<int> MyTask(AsyncEnumerator<Dictionary<T, TResult>> ae, IEnumerable<T> jobSource, Func<List<T>, List<TResult>> _jobAction)
        {
            int totalCount = jobSource.Count();
            int perThread = totalCount / _threadNum;//每个线程的工作数目
            if (perThread == 0)
                perThread = totalCount;
            //实际的工人数目
            int realWorker = 0;
            //开始分配
            int skipCount = 0;
            IEnumerable<T> oneJob = jobSource.Skip(skipCount).Take(perThread);

            while (oneJob.Count() != 0)
            {
                JobWorkGetHandler handler = DoOneJob;
                handler.BeginInvoke(oneJob, _func, ae.End(), handler);
                realWorker++;
                skipCount += perThread;
                //给下一个工人分配
                oneJob = jobSource.Skip(skipCount).Take(perThread);
            }

            //交工
            yield return realWorker;
            //验收
            //here is the results
            Dictionary<T, TResult> finalResuls = new Dictionary<T, TResult>();
            for (int i = 0; i < realWorker; i++)
            {
                IAsyncResult asyncResualt = ae.DequeueAsyncResult();
                JobWorkGetHandler handler = asyncResualt.AsyncState as JobWorkGetHandler;
                var rr = handler.EndInvoke(asyncResualt);
                if (rr != null && rr.Count > 0)
                {
                    foreach (var k in rr.Keys)
                    {
                        if (!finalResuls.ContainsKey(k))
                            finalResuls.Add(k, rr[k]);
                    }
                }
            }
            ae.Result = finalResuls;

        }

    }
}
