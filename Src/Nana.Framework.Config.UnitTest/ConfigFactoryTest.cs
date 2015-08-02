using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Nana.Framework.Config.UnitTest
{
    public class ConfigFactoryTest
    {
        [Fact()]
        public void GetConfigTest()
        {
            ConfigFactory.Settings.AppName = "Web";

            ConfigUnit actual = ConfigFactory.GetConfig("DaoConfig", "201507300747");

            Assert.Equal("Web", actual.AppName);
        }
    }
}
