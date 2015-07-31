using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nana.Framework.Config.UnitTest
{
    public class SampleConfigUnit : ConfigUnit
    {
        public string ConnectionString { get; set; }
        
        public override void Reload()
        {
            string configFileText = File.ReadAllText(this.FullPath);

            this.ConnectionString = configFileText;
        }
    }
}
