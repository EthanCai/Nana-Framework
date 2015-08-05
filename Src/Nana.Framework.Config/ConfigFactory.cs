using System;
using System.Collections.Generic;
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

            try
            {
                Settings.Logger.Log(string.Format(
                    "从ConfigUnitPool中读取配置信息, AppName={0}, ConfName={1}, Version={2}", 
                    Settings.AppName, confName, version),
                    EnumLogLevel.Debug);
                result = ConfigUnitPool.Instance.Get(confName, version);
                
                if (result != null)
                {
                    Settings.Logger.Log(
                        "ConfigUnitPool中已存在配置信息",
                        EnumLogLevel.Debug);
                    return result;
                }

                Settings.Logger.Log(
                    "通过ConfigLoader加载配置信息",
                    EnumLogLevel.Debug);
                result = Settings.Loader.LoadConfig(confName, version);
                if (result != null)
                {
                    Settings.Logger.Log(
                        "通过ConfigLoader完成加载，现在保存到ConfigUnitPool",
                        EnumLogLevel.Debug);
                    ConfigUnitPool.Instance.Set(confName, version, result);

                    Settings.Logger.Log(
                        "启动定时检查配置更新信息",
                        EnumLogLevel.Debug);
                    Settings.Watcher.StartWatch();
                }
            }
            catch (Exception ex)
            {
                string message = string.Format(@"读取配置发生错误, Message: {0}, StackTrace: {1}",
                    ex.Message, ex.StackTrace);
                Settings.Logger.Log(message, EnumLogLevel.Error);
            }

            return result;
        }

    }
}
