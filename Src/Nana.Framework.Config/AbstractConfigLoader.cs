using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nana.Framework.Config
{
    public abstract class AbstractConfigLoader
    {
        protected Settings Settings { get; set; }

        public AbstractConfigLoader(Settings settings)
        {
            this.Settings = settings;
        }

        public abstract ConfigUnit LoadConfig(string confName, string version);
    }
}
