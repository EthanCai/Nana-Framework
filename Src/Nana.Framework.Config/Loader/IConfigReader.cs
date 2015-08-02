using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nana.Framework.Config.Loader
{
    interface IConfigReader
    {
        ConfigUnit Read(string text);
    }
}
