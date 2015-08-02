using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nana.Framework.Config
{
    public interface IConfigLogger
    {
        void Log(string message, EnumLogLevel level);
    }
}
