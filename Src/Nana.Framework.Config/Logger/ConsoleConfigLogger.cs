using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nana.Framework.Config.Logger
{
    public class ConsoleConfigLogger : IConfigLogger
    {
        public void Log(string message, EnumLogLevel level)
        {
            string time = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff");
            string messageRemovedNewLine = message.Replace(System.Environment.NewLine, " ");
            string output = string.Format("[{0}] [{1}] {2}", 
                time, level.ToString(), messageRemovedNewLine);
            Console.WriteLine(output);
        }
    }
}
