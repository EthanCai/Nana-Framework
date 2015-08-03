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
            Settings settings = Settings.GetDefaultSettings();
            settings.AppName = "Web";
            settings.LocalConfigFileSource.LocalConfigDirectory =
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestData");
            settings.LocalConfigFileSource.LocalConfigFormat =
                LocalConfigFileSource.XML_Config_Format;

            AbstractConfigLoader configLoader = new LocalConfigLoader(settings);
            ConfigUnit actual = configLoader.LoadConfig("DaoConfig", "201507300747");
            Assert.Equal("Web", actual.AppName);
        }

        [Fact()]
        public void TestLoadJSONConfig()
        {
            Settings settings = Settings.GetDefaultSettings();
            settings.AppName = "Web";
            settings.LocalConfigFileSource.LocalConfigDirectory =
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestData");
            settings.LocalConfigFileSource.LocalConfigFormat =
                LocalConfigFileSource.JSON_Config_Format;

            AbstractConfigLoader configLoader = new LocalConfigLoader(settings);
            ConfigUnit actual = configLoader.LoadConfig("DaoConfig", "201507300747");
            Assert.Equal("Web", actual.AppName);
        }
    }
}
