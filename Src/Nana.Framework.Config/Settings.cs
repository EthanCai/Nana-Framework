using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Nana.Framework.Config.Loader;
using Nana.Framework.Config.Logger;

namespace Nana.Framework.Config
{
    public class Settings
    {
        private const int Default_Max_Try_Times_When_Fail_To_Load_Config = 3;

        public const string XML_Config_Format = "xml";
        public const string JSON_Config_Format = "json";
        public const string YAML_Config_Format = "yaml";

        public AbstractConfigLoader Loader { get; set; }

        public IConfigLogger Logger { get; set; }

        /// <summary>
        /// App名称
        /// </summary>
        /// <value>
        /// The name of the application.
        /// </value>
        public string AppName { get; set; }

        /// <summary>
        /// 本地配置文件保存的目录
        /// </summary>
        /// <value>
        /// The configuration directory.
        /// </value>
        public string LocalConfigDirectory { get; set; }

        private string _localConfigFormat = null;

        /// <summary>
        /// 本地配置文件的格式
        /// </summary>
        /// <value>
        /// xml, json, yaml
        /// </value>
        public string LocalConfigFormat
        {
            get { return this._localConfigFormat; }
            set
            {
                if (value != XML_Config_Format
                    && value != JSON_Config_Format
                    && value != YAML_Config_Format)
                {
                    string message = string.Format("不支持LocalConfigFormat为{0}的配置文件", LocalConfigFormat);
                    throw new InitConfigException(message);
                }
                this._localConfigFormat = value;
            }
        }

        public string LocalConfigFileExt
        {
            get
            {
                switch (LocalConfigFormat)
                {
                    case XML_Config_Format:
                        return ".config";
                        break;
                    case JSON_Config_Format:
                        return ".json";
                        break;
                    case YAML_Config_Format:
                        return ".yaml";
                        break;
                    default:
                        string message = string.Format("不支持LocalConfigFormat为{0}的配置文件", LocalConfigFormat);
                        throw new InitConfigException(message);
                        break;
                }
            }
        }

        /// <summary>
        /// 远程配置本地缓存目录
        /// </summary>
        /// <value>
        /// The remote configuration cache directory.
        /// </value>
        public string RemoteConfigCacheDirectory { get; set; }

        /// <summary>
        /// 加载配置失败，最大的重试次数
        /// </summary>
        /// <value>
        /// The maximum try times when fail to load configuration.
        /// </value>
        public int MaxTryTimesWhenFailToLoadConfig { get; set; }

        /// <summary>
        /// 检查配置是否有变化的轮询间隔，单位秒
        /// </summary>
        /// <value>
        /// The polling interval.
        /// </value>
        public int PollingInterval { get; set; }

        public Settings()
        {
            
        }

        public static Settings GetDefaultSettings()
        {
            Settings result = new Settings();
            result.Loader = result.GetDefaultConfigLoader();
            result.Logger = result.GetDefaultConfigLogger();
            result.AppName = result.GetDefaultAppName();
            result.LocalConfigDirectory = result.GetDefaultLocalConfigDirectory();
            result.LocalConfigFormat = result.GetDefaultLocalConfigFormat();
            result.RemoteConfigCacheDirectory = result.GetDefaultRemoteConfigCacheDirectory();
            result.MaxTryTimesWhenFailToLoadConfig = result.GetMaxTryTimesWhenFailToLoadConfig();
            result.PollingInterval = result.GetDefaultPollingInterval();

            return result;
        } 

        private int GetDefaultPollingInterval()
        {
            #if DEBUG
                return 1000 * 1;    // 1秒 
            #else
                return 1000 * 60 * 5;   // 更新间隔 单位ms 当前值 5分钟
            #endif
        }

        private int GetMaxTryTimesWhenFailToLoadConfig()
        {
            return Default_Max_Try_Times_When_Fail_To_Load_Config;
        }

        private string GetDefaultRemoteConfigCacheDirectory()
        {
            return Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase,
                 HttpContext.Current != null ? @".\App_Data\confcache\" : @"..\confcache\");
        }

        private string GetDefaultLocalConfigFormat()
        {
            return XML_Config_Format;
        }

        private string GetDefaultLocalConfigDirectory()
        {
            return Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase,
                 HttpContext.Current != null ? @".\App_Data\conf\" : @"..\conf\"); 
        }

        private string GetDefaultAppName()
        {
            return ConfigurationManager.AppSettings["AppName"];
        }

        private IConfigLogger GetDefaultConfigLogger()
        {
            return new ConsoleConfigLogger();
        }

        private AbstractConfigLoader GetDefaultConfigLoader()
        {
            return new LocalConfigLoader(this);
        }
    }
}
