using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;


namespace Nana.Framework.Config
{
    public class ConfigUnit
    {
        public string AppName { get; set; }

        public string ConfName { get; set; }

        public string Version { get; set; }

        public Dictionary<string, string> Items { get; set; }

    }
}
