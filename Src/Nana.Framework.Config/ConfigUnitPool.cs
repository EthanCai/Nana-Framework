using System;
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
        private Dictionary<string, Dictionary<string, ConfigUnit>> _configUnitCache = 
            new Dictionary<string, Dictionary<string, ConfigUnit>>();    // <confName, <version, ConfigUnit>>
        private ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();

        public Dictionary<string, Dictionary<string, ConfigUnit>> Cache
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

            try 
            {
                this._lock.EnterWriteLock();    //防止多个线程同时写，发生冲突

                if (!this._configUnitCache.ContainsKey(confName))
                {
                    this._configUnitCache[confName] = new Dictionary<string, ConfigUnit>()
                    {
                        {version, configUnit}
                    };
                    return;
                }

                this._configUnitCache[confName][version] = configUnit;
            }
            finally
            {
                this._lock.ExitWriteLock();
            }
        }

        public void Remove(string confName, string version)
        {
            try
            {
                this._lock.EnterWriteLock();

                if (!this._configUnitCache.ContainsKey(confName)
                    || !this._configUnitCache[confName].ContainsKey(version))
                {
                    return;
                }

                this._configUnitCache[confName].Remove(version);
            }
            finally
            {
                this._lock.ExitWriteLock();
            }
        }

        public ConfigUnit Get(string confName, string version)
        {
            try
            {
                this._lock.EnterReadLock();

                if (!this._configUnitCache.ContainsKey(confName)
                    || !this._configUnitCache[confName].ContainsKey(version))
                {
                    return null;
                }

                return this._configUnitCache[confName][version];
            }
            finally
            {
                this._lock.ExitReadLock();
            }
        }

        public void Clear()
        {
            try
            {
                this._lock.EnterWriteLock();

                this._configUnitCache.Clear();
            }
            finally
            {
                this._lock.ExitWriteLock();
            }
        }
    }
}
