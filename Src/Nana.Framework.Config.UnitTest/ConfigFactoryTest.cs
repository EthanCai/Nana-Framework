using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nana.Framework.Config.Logger;
using Xunit;

namespace Nana.Framework.Config.UnitTest
{
    public class ConfigFactoryTest
    {
        [Fact()]
        public void TestGetConfig()
        {
            ConfigFactory.Settings.AppName = "Web";
            ConfigFactory.Settings.Logger = new ConsoleConfigLogger();

            ConfigUnit actual = ConfigFactory.GetConfig("DaoConfig", "201507300747");

            Assert.Equal("Web", actual.AppName);
        }
    }
}
