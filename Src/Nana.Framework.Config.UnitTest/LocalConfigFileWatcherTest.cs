using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Nana.Framework.Config.Logger;
using Xunit;

namespace Nana.Framework.Config.UnitTest
{
    public class LocalConfigFileWatcherTest
    {
        public LocalConfigFileWatcherTest()
        {
            
        }

        [Fact()]
        public void TestLocalConfigFileWatcher()
        {
            WriteXMLConfigFile(new ConfigUnit()
            {
                AppName = "Web",
                ConfName = "NetworkConfig",
                Version = "201507300747",
                Items = new Dictionary<string, string>() { { "key1", "value1" } }
            });

            ConfigFactory.Settings.AppName = "Web";

            ConfigUnit actual = ConfigFactory.GetConfig("NetworkConfig", "201507300747");

            Assert.Equal(new Dictionary<string, string>() {
                { "key1", "value1" }
            }, actual.Items);

            WriteXMLConfigFile(new ConfigUnit()
            {
                AppName = "Web",
                ConfName = "NetworkConfig",
                Version = "201507300747",
                Items = new Dictionary<string, string>() { { "key2", "value2" } }
            });

            Thread.Sleep(ConfigFactory.Settings.ConfigWatcherPollingInterval + 1000);

            var actualAfterUpdate = ConfigFactory.GetConfig("NetworkConfig", "201507300747");

            Assert.Equal(new Dictionary<string, string>() {
                { "key2", "value2" }
            }, actualAfterUpdate.Items);
        }

        private void WriteXMLConfigFile(ConfigUnit unit)
        {
            string template = @"<?xml version=""1.0"" encoding=""utf-8"" ?>
<Config>
  <AppName>{0}</AppName>
  <ConfName>{1}</ConfName>
  <Version>{2}</Version>
  <Data>
    <Item key=""{3}"">{4}</Item>
  </Data>
</Config>";

            string xml = string.Format(template,
                unit.AppName, unit.ConfName, unit.Version,
                unit.Items.First().Key, unit.Items.First().Value);

            string fileName = string.Format("{0}_{1}_{2}.config", 
                unit.AppName, unit.ConfName, unit.Version);

            string filePath = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory, "..\\Conf\\", fileName);

            File.WriteAllText(filePath, xml);
        }
    }
}
