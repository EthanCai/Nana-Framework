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
    public class JSONConfigReaderTest
    {

        [Fact()]
        public void TestReadAppNameIsCorrect()
        {
            ConfigUnit actual = GetSampleConfigUnit();
            Assert.Equal("Web", actual.AppName);
        }

        [Fact()]
        public void TestReadConfNameIsCorrect()
        {
            ConfigUnit actual = GetSampleConfigUnit();
            Assert.Equal("DaoConfig", actual.ConfName);
        }

        [Fact()]
        public void TestReadVersionIsCorrect()
        {
            ConfigUnit actual = GetSampleConfigUnit();
            Assert.Equal("201507300747", actual.Version);
        }

        [Fact()]
        public void TestReadItemsAreCorrect()
        {
            ConfigUnit actual = GetSampleConfigUnit();

            var expect = new Dictionary<string, string>()
            {
                { "key1", "value1" },
                { "key2", "value2" },
                { "key3", "value3" },
                { "key4", "value4" }
            };

            Assert.Equal(expect, actual.Items);
        }

        private ConfigUnit GetSampleConfigUnit()
        {
            JSONConfigReader reader = new JSONConfigReader();
            string text =
                File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                    ".\\TestData\\Web_DaoConfig_201507300747.json"));
            return reader.Read(text); 
        }
    }
}
