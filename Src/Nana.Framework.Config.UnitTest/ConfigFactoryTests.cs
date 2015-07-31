using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Nana.Framework.Config;

namespace Nana.Framework.Config.UnitTest
{
    public class ConfigFactoryTests
    {
        [Fact()]
        public void GetConfigTest()
        {
            SampleConfigUnit config = ConfigFactory.GetConfig<SampleConfigUnit>();

            Assert.Equal("TestConnectionString", config.ConnectionString);
        }
    }
}
