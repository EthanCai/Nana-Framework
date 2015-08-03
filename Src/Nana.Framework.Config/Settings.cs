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
using Nana.Framework.Config.Watcher;

namespace Nana.Framework.Config
{
    public class Settings
    {
        private const int Default_Max_Try_Times_When_Fail_To_Load_Config = 3;

        public AbstractConfigLoader Loader { get; set; }

        public IConfigLogger Logger { get; set; }

        public AbstractConfigWatcher Watcher { get; set; }

        /// <summary>
        /// App名称
        /// </summary>
        /// <value>
        /// The name of the application.
        /// </value>
        public string AppName { get; set; }

        

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
        public int ConfigWatcherPollingInterval { get; set; }

        public LocalConfigFileSource LocalConfigFileSource { get; set; }

        public RemoteConfigSource RemoteConfigSource { get; set; }

        private Settings()
        {
            
        }

        public static Settings GetDefaultSettings()
        {
            Settings result = new Settings();
            result.Loader = result.GetDefaultConfigLoader();
            result.Logger = result.GetDefaultConfigLogger();
            result.Watcher = result.GetDefaultConfigWatcher();

            result.AppName = result.GetDefaultAppName();

            result.MaxTryTimesWhenFailToLoadConfig = result.GetMaxTryTimesWhenFailToLoadConfig();
            result.ConfigWatcherPollingInterval = result.GetDefaultPollingInterval();

            result.LocalConfigFileSource = new LocalConfigFileSource();

            return result;
        }

        private AbstractConfigWatcher GetDefaultConfigWatcher()
        {
            return new LocalConfigFileWatcher(this);
        }

        private string GetDefaultAppName()
        {
            return ConfigurationManager.AppSettings["AppName"];
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

        private IConfigLogger GetDefaultConfigLogger()
        {
            return new NullConfigLogger();
        }

        private AbstractConfigLoader GetDefaultConfigLoader()
        {
            return new LocalConfigLoader(this);
        }
    }
}
