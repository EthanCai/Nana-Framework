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

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is ConfigUnit))
            {
                return false;
            }

            var unit = obj as ConfigUnit;

            if (this.AppName != unit.AppName
                || this.ConfName != unit.ConfName
                || this.Version != unit.Version)
            {
                return false;
            }

            if ((this.Items == null && unit.Items != null)
                || (this.Items != null && unit.Items == null))
            {
                return false;
            }

            if (this.Items != null && unit.Items != null)
            {
                bool isDifferent = this.Items.Any(p => 
                    !unit.Items.ContainsKey(p.Key) || (unit.Items[p.Key] != p.Value));
                if (isDifferent) return false;
            }

            return true;
        }
    }
}
