using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Nana.Framework.Config.Loader
{
    class JSONConfigReader : IConfigReader
    {
        public ConfigUnit Read(string text)
        {
            ConfigUnit result = new ConfigUnit();

            var jObject = JObject.Parse(text);

            result.AppName = jObject["AppName"].Value<string>();
            result.ConfName = jObject["ConfName"].Value<string>();
            result.Version = jObject["Version"].Value<string>();

            result.Items = new Dictionary<string, string>();
            foreach (JProperty item in jObject["Data"].Children())
            {
                string key = item.Name;
                string value = item.Value.ToString();
                result.Items[key] = value;
            }

            return result;
        }
    }
}
