using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nana.Framework.Config
{
    public class InitConfigException : Exception
    {

        public InitConfigException()
            : base()
        {
        }

        public InitConfigException(string message)
            : base(message)
        {
        }

        public InitConfigException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

    }
}
