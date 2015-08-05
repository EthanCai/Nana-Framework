using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Nana.Framework.Config
{
    class ConfigUnitPool
    {
        private static readonly ConfigUnitPool _instance = new ConfigUnitPool();
        private ConcurrentDictionary<string, ConcurrentDictionary<string, ConfigUnit>> _configUnitCache =
            new ConcurrentDictionary<string, ConcurrentDictionary<string, ConfigUnit>>();   // <confName, <version, ConfigUnit>>
        private object _lock = new object();

        public ConcurrentDictionary<string, ConcurrentDictionary<string, ConfigUnit>> Cache
        {
            get { return this._configUnitCache; }
        }

        private ConfigUnitPool()
        { 
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
            if (configUnit == null)
            {
                throw new InitConfigException("configUnit不能为null");
            }

            lock (this._lock)
            {
                if (!this._configUnitCache.ContainsKey(confName))
                {

                    this._configUnitCache[confName] = new ConcurrentDictionary<string, ConfigUnit>();
                    this._configUnitCache[confName][version] = configUnit;
                    return;
                }

                this._configUnitCache[confName][version] = configUnit;
            }
        }

        public void Remove(string confName, string version)
        {
            lock (this._lock)
            {
                if (!this._configUnitCache.ContainsKey(confName)
                    || !this._configUnitCache[confName].ContainsKey(version))
                {
                    return;
                }

                ConfigUnit removedConfigUnit = null;
                this._configUnitCache[confName].TryRemove(version, out removedConfigUnit);
            }
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
            lock (this._lock)
            {
                this._configUnitCache.Clear();
            }
        }
    }
}
