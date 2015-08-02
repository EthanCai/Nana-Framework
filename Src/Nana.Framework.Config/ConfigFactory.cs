using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace Nana.Framework.Config
{
    public class ConfigFactory
    {
        private readonly static Settings _settings = Settings.GetDefaultSettings();

        public static Settings Settings
        {
            get { return _settings; }
        }

        /// <summary>
        /// 获得指定配置集合的最新版配置信息
        /// </summary>
        /// <param name="name">配置集合的名称</param>
        /// <returns></returns>
        public static ConfigUnit GetConfig(string confName, string version)
        {
            ConfigUnit result = null;

            result = ConfigUnitPool.Instance.Get(confName, version);
            if (result != null)
            {
                return result;
            }

            result = Settings.Loader.LoadConfig(confName, version);
            if (result != null)
            {
                ConfigUnitPool.Instance.Set(confName, version, result);
            }

            return result;
        }

    }
}
