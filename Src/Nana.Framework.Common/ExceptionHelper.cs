using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nana.Framework.Common
{
    public class ExceptionHelper
    {
        /// <summary>
        /// 取得最底层的异常
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static Exception GetBaseInnerException(Exception ex)
        {
            if (ex == null)
            {
                return null;
            }
            else
            {
                Exception innerException = ex.InnerException;
                Exception exception = ex;
                for (; innerException != null; innerException = innerException.InnerException)
                    exception = innerException;
                return exception;
            }
        }

        /// <summary>
        /// 取得异常及其内部异常的全部Message及StackTrace
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static string GetAllExceptionMessageAndStackTrace(Exception ex)
        {
            if (ex == null)
            {
                return string.Empty;
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                Exception point = ex;
                while (point != null)
                {
                    sb.Append(string.Format("===================={0}====================\r\n", point.GetType().Name));
                    sb.Append(ex.Message);
                    sb.Append("\r\n");
                    sb.Append(ex.StackTrace);
                    sb.Append("\r\n");

                    point = point.InnerException;
                }

                return sb.ToString();
            }
        }
    }
}
