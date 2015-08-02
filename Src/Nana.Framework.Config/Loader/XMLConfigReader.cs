using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Nana.Framework.Config.Loader
{
    class XMLConfigReader : IConfigReader
    {
        public ConfigUnit Read(string text)
        {
            ConfigUnit result = new ConfigUnit();

            XDocument xdoc = XDocument.Parse(text);
            result.AppName = xdoc.Root.Element("AppName").Value;
            result.ConfName = xdoc.Root.Element("ConfName").Value;
            result.Version = xdoc.Root.Element("Version").Value;

            result.Items = new Dictionary<string, string>();
            foreach (var xElement in xdoc.Root.Element("Data").Elements("Item"))
            {
                string key = xElement.Attribute("key").Value;
                string value = xElement.Value;
                result.Items[key] = value;
            }

            return result;
        }
    }
}
