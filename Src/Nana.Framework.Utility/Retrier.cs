using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nana.Framework.Utility
{
    /// <summary>
    /// 指定次数重复执行一段逻辑
    /// 参考http://blogs.msdn.com/b/dgartner/archive/2010/03/09/trying-and-retrying-in-c.aspx
    /// </summary>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    public class Retrier<TResult>
    {
        public TResult Try(Func<TResult> func,
            int maxRetries, Func<int, Exception, bool> exceptionHandler = null)
        {
            return TryWithDelay(func, maxRetries, 0, exceptionHandler);
        }

        /// <summary>
        /// Tries the with delay.
        /// </summary>
        /// <param name="func">The function.</param>
        /// <param name="maxRetries">The maximum retries.</param>
        /// <param name="delayInMilliseconds">The delay in milliseconds.</param>
        /// <param name="exceptionHandler">异常处理代理，返回true表示中止重试</param>
        /// <returns></returns>
        public TResult TryWithDelay(Func<TResult> func, int maxRetries,
            int delayInMilliseconds, Func<int, Exception, bool> exceptionHandler = null)
        {
            TResult returnValue = default(TResult);
            int numTries = 0;
            bool succeeded = false;
            while (numTries < maxRetries)
            {
                try
                {
                    returnValue = func();
                    succeeded = true;
                }
                catch (Exception ex)
                {
                    if (exceptionHandler != null)
                    {
                        bool isStopTry = exceptionHandler(numTries+1, ex);
                        if (isStopTry) break; 
                    }
                }
                finally
                {
                    numTries++;
                }
                
                if (succeeded) return returnValue;

                if (delayInMilliseconds > 0) System.Threading.Thread.Sleep(delayInMilliseconds);
            }
            return default(TResult);
        }
    }
}
