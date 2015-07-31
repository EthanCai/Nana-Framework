using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nana.Framework.Config
{
    /// <summary>
    /// 初始配置失败 异常
    /// </summary>
    public class InitConfigException : Exception
    {

        public InitConfigException() : base()
        {
        }

        public InitConfigException(string message) : base(message)
        {
        }

        public InitConfigException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

    }
}
