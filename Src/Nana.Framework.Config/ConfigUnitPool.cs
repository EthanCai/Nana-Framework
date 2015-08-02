using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nana.Framework.Config
{
    class ConfigUnitPool
    {
        private static readonly ConfigUnitPool _instance = new ConfigUnitPool();
        private Dictionary<string, SortedDictionary<string, ConfigUnit>> _configUnitCache = null;    // <confName, <version, ConfigUnit>>

        private ConfigUnitPool()
        {
            this._configUnitCache = new Dictionary<string, SortedDictionary<string, ConfigUnit>>();  
        }

        public static ConfigUnitPool Instance
        {
            get
            {
                return _instance;
            }
        }

        public void Set(string confName, string version, ConfigUnit configUnit)
        {
            if (!this._configUnitCache.ContainsKey(confName))
            {
                this._configUnitCache[confName] = new SortedDictionary<string, ConfigUnit>()
                {
                    {version, configUnit}
                };
                return;
            }

            this._configUnitCache[confName][version] = configUnit;
        }

        public ConfigUnit Get(string confName, string version)
        {
            if (!this._configUnitCache.ContainsKey(confName)
                || !this._configUnitCache[confName].ContainsKey(version))
            {
                return null;
            }

            return this._configUnitCache[confName][version];
        }

        public void Clear()
        {
            this._configUnitCache.Clear();
        }
    }
}
