using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nana.Framework.Config.Logger
{
    public class NullConfigLogger : IConfigLogger
    {

        public void Log(string message, EnumLogLevel level)
        {
            // do nothing
        }
    }
}
