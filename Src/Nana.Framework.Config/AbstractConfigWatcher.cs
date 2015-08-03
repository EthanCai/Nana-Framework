using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nana.Framework.Config
{
    public abstract class AbstractConfigWatcher
    {
        protected Settings Settings { get; set; }

        public AbstractConfigWatcher(Settings settings)
        {
            this.Settings = settings;
        }

        public abstract void StartWatch();

        public abstract void StopWatch();
    }
}
