using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nana.Framework.Config.UnitTest.Sample
{
    public class SampleConfigUnit : IConfigUnit
    {
        public string ConnectionString { get; set; }
        public void Load(string configContent)
        {
            ConnectionString = configContent;
        }
    }
}
