using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nana.Framework.Config
{
    class ConfigUnitPool
    {
        private static ConfigUnitPool instance = new ConfigUnitPool();
        private Dictionary<string, IConfigUnit> _configUnitDictionary =
            new Dictionary<string, IConfigUnit>();  //缓存所有的配置

        private ConfigUnitPool()
        {
        }

        public static ConfigUnitPool Instance
        {
            get
            {
                return instance;
            }
        }

        public void Set(string key, IConfigUnit configUnit)
        {
            this._configUnitDictionary[key] = configUnit;
        }

        public IConfigUnit Get(string key)
        {
            if (!this._configUnitDictionary.ContainsKey(key))
            {
                return null;
            }

            return this._configUnitDictionary[key];
        }

        public void Clear()
        {
            this._configUnitDictionary.Clear();
        }
    }
}
