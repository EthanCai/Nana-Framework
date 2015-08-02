using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nana.Framework.Config.Loader;
using Xunit;

namespace Nana.Framework.Config.UnitTest
{
    public class LocalConfigLoaderTest
    {
        [Fact()]
        public void TestLoadXMLConfig()
        {
            Settings settings = new Settings()
            {
                AppName = "Web",
                LocalConfigDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestData"),
                LocalConfigFormat = Settings.XML_Config_Format
            };
            AbstractConfigLoader configLoader = new LocalConfigLoader(settings);
            ConfigUnit actual = configLoader.LoadConfig("DaoConfig", "201507300747");
            Assert.Equal("Web", actual.AppName);
        }

        [Fact()]
        public void TestLoadJSONConfig()
        {
            Settings settings = new Settings()
            {
                AppName = "Web",
                LocalConfigDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestData"),
                LocalConfigFormat = Settings.JSON_Config_Format
            };
            AbstractConfigLoader configLoader = new LocalConfigLoader(settings);
            ConfigUnit actual = configLoader.LoadConfig("DaoConfig", "201507300747");
            Assert.Equal("Web", actual.AppName);
        }
    }
}
