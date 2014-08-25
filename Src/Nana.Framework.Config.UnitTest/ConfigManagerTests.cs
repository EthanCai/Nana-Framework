using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Nana.Framework.Config;
using Nana.Framework.Config.UnitTest.Sample;
using NUnit.Framework;

namespace Nana.Framework.Config.UnitTest
{
    [TestFixture()]
    public class ConfigManagerTests
    {
        [SetUp()]
        public void BeforeEachTest()
        {
            ConfigManager.Clear();
            File.WriteAllText(
                "..\\Sample\\SampleConfig.config", "TestConnectionString");
        }

        [TearDown()]
        public void AfterEachTest()
        {
            ConfigManager.Clear();
            File.WriteAllText(
                "..\\Sample\\SampleConfig.config", "TestConnectionString");
        }

        [Test()]
        public void Test_Clear_Whether_Works()
        {
            var configUnit = ConfigManager.LoadFile<SampleConfigUnit>(
                "..\\Sample\\SampleConfig.config");

            ConfigManager.Clear();

            configUnit = ConfigManager.Get<SampleConfigUnit>();

            string expected = null;

            Assert.AreEqual(expected, configUnit);
        }

        [Test()]
        public void Test_Load_Exist_File()
        {
            var configUnit = ConfigManager.LoadFile<SampleConfigUnit>(
                "..\\Sample\\SampleConfig.config");

            string expected = "TestConnectionString";

            Assert.AreEqual(expected, configUnit.ConnectionString);
        }

        [Test()]
        [ExpectedException(typeof(FileNotFoundException))]
        public void Test_Load_Not_Exist_File()
        {
            var configUnit = ConfigManager.LoadFile<SampleConfigUnit>(
                "..\\Sample\\SampleConfigNotExist.config");

            string expected = "TestConnectionString";

            Assert.AreEqual(expected, configUnit.ConnectionString);
        }

        [Test()]
        [ExpectedException(typeof(NullReferenceException))]
        public void Test_Get_Before_Load()
        {
            var configUnit = ConfigManager.Get<SampleConfigUnit>();

            string expected = "TestConnectionString";

            Assert.AreEqual(expected, configUnit.ConnectionString);
        }

        [Test()]
        public void Test_Get_After_Load()
        {
            ConfigManager.LoadFile<SampleConfigUnit>(
                "..\\Sample\\SampleConfig.config");

            var configUnit = ConfigManager.Get<SampleConfigUnit>();
            string expected = "TestConnectionString";

            Assert.AreEqual(expected, configUnit.ConnectionString);
        }

        [Test()]
        public void Test_Whether_Reload_When_Config_File_Change()
        {
            ConfigManager.LoadFile<SampleConfigUnit>("..\\Sample\\SampleConfig.config");
            File.WriteAllText("..\\Sample\\SampleConfig.config", "TestConnectionString2");

            Thread.Sleep(400);

            var configUnit = ConfigManager.Get<SampleConfigUnit>();
            Assert.AreEqual("TestConnectionString2", configUnit.ConnectionString);
        }

        [Test()]
        public void Test_Whether_Trigger_Event_When_Config_File_Change()
        {
            ConfigManager.LoadFile<SampleConfigUnit>("..\\Sample\\SampleConfig.config");
            var handler = new SampleConfigUnitChangeHandler();
            ConfigManager.RegChangeHandler<SampleConfigUnit>(handler);
            
            File.WriteAllText("..\\Sample\\SampleConfig.config", "TestConnectionString2");

            Thread.Sleep(400);

            var configUnit = ConfigManager.Get<SampleConfigUnit>();
            Assert.AreEqual("TestConnectionString2", configUnit.ConnectionString);
            Assert.AreEqual("TestConnectionString2", ((SampleConfigUnit)handler.ConfigUnit).ConnectionString);
        }

        class SampleConfigUnitChangeHandler : IConfigChangeHandler
        {
            public IConfigUnit ConfigUnit { get; set; }

            public void OnChange(IConfigUnit configUnit)
            {
                this.ConfigUnit = configUnit;
            }
        }
    }
}
