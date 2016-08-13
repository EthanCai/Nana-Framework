using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Threading;
using Wintellect.Threading.AsyncProgModel;

namespace Nana.Framework.Async
{
    public class TaskHelper
    {
      
        /// <summary>
        /// 执行并行任务，不捕获异常
        /// </summary>
        /// <param name="dispatcher"></param>
		public static void Execute(AsyncTaskDispatcher dispatcher)
        {
            AsyncEnumerator ae = new AsyncEnumerator();
     
            ae.EndExecute(ae.BeginExecute(DoTask(ae, dispatcher), null));
      
        }
		
        /// <summary>
        /// 执行并行任务，捕获异常并把第一个异常直接扔出来
        /// </summary>
        /// <param name="dispatcher"></param>
        public static void ExecuteAndThrowFirstException(AsyncTaskDispatcher dispatcher)
        {
            try
            {
                AsyncEnumerator ae = new AsyncEnumerator();
                ae.EndExecute(ae.BeginExecute(DoTask(ae, dispatcher), null));
            }
            catch (Exception ex)
            {
                if (ex is DispatchException)
                {
                    DispatchException disEx = ex as DispatchException;

                    if (disEx.TaskExceptions != null && disEx.TaskExceptions.Count > 0)
                    {
                        Exception cex = disEx.TaskExceptions.First().Value;
                        throw cex;
                    }
                }
                else
                {
                    throw ex;
                }
            }
        }
		
        /// <summary>
        /// 另起线程异步执行任务，不等待结果
        /// </summary>
        /// <param name="userFuncs"></param>
        /// <returns></returns>
        public static IAsyncResult ProcessActionInAsyncThread(params Action[] userFuncs)
        {
            AsyncTaskDispatcher s_dispatcher = new AsyncTaskDispatcher(false);

            s_dispatcher.RegisterTasks(userFuncs);

            IAsyncResult asyncResult = s_dispatcher.BeginDispatch(
               (ar) =>
               {
                   try
                   {
                       s_dispatcher.EndDispatch(ar);
                   }
                   catch (DispatchException ex)
                   {
                   }
               },
               null);
            return asyncResult;
        }
		
        /// <summary>
        /// 并行变串行
        /// </summary>
        /// <param name="ae"></param>
        /// <param name="dispatcher"></param>
        /// <returns></returns>
        private static IEnumerator<Int32> DoTask(AsyncEnumerator ae,AsyncTaskDispatcher dispatcher)
        {
            dispatcher.BeginDispatch(ae.End(), null);
            yield return 1;
            dispatcher.EndDispatch(ae.DequeueAsyncResult());
        }
    }
}
