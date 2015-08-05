using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Nana.Framework.Common;
using Nana.Framework.Config.Loader;
using Nana.Framework.Utility;

namespace Nana.Framework.Config.Watcher
{
    public class LocalConfigFileWatcher : AbstractConfigWatcher
    {
        private Thread _watchThread = null;
        private static readonly object _lock = new object();

        public LocalConfigFileWatcher(Settings settings) : base(settings)
        {
            
        }

        public override void StartWatch()
        {
            lock (_lock)
            {
                if (this._watchThread != null
                    && this.IsWatchThreadRunning())
                {
                    Settings.Logger.Log(
                        "watchThread已经是Running状态，不必重新启动", 
                        EnumLogLevel.Debug);
                    return;
                }

                this._watchThread = new Thread(() => LoopWatching(this.Settings));
                this._watchThread.IsBackground = true;
                this._watchThread.Start();

                Settings.Logger.Log(
                    "watchThread已经启动",
                    EnumLogLevel.Debug);
            }
            
        }

        public override void StopWatch()
        {
            lock (_lock)
            {
                if (this._watchThread != null)
                {
                    this._watchThread.Abort();
                    Settings.Logger.Log(
                        "watchThread已经申请abort",
                        EnumLogLevel.Debug);
                }
            }
        }

        private bool IsWatchThreadRunning()
        {
            return (this._watchThread.ThreadState & (ThreadState.Stopped | ThreadState.Unstarted)) == 0;
        }

        private void LoopWatching(Settings settings)
        {
            while (true)
            {
                try
                {
                    var localConfigLoader = new LocalConfigLoader(this.Settings);
                    List<string> confNameList = ConfigUnitPool.Instance.Cache.Keys.ToList();

                    foreach (var confName in confNameList)
                    {
                        if (ConfigUnitPool.Instance.Cache[confName] == null) continue;

                        var configOfDifferentVersions = 
                            ConfigUnitPool.Instance.Cache[confName].ToDictionary(
                                p => p.Key, p => p.Value);
                        foreach (var version in configOfDifferentVersions.Keys)
                        {
                            var configUnitInPool = configOfDifferentVersions[version];
                            var configUnitFromLocalFile = new Retrier<ConfigUnit>().Try(
                                () => localConfigLoader.LoadConfig(confName, version), 
                                settings.MaxTryTimesWhenFailToLoadConfig,
                                (int times, Exception ex) =>
                                {
                                    string message = string.Format("第{0}次重试, Message: {1}, StackTrace: {2}", 
                                        times, ex.Message, ex.StackTrace);
                                    settings.Logger.Log(message, EnumLogLevel.Error);
                                    return false;
                                });

                            if (configUnitFromLocalFile == null)
                            {
                                Settings.Logger.Log(String.Format(
                                    "从ConfigUnitPool中删除配置信息, ConfName={0}, Version={1}",
                                    confName, version),
                                    EnumLogLevel.Debug);
                                ConfigUnitPool.Instance.Remove(confName, version);
                            }
                            else if (!configUnitInPool.Equals(configUnitFromLocalFile))
                            {
                                Settings.Logger.Log(String.Format(
                                    "本地配置文件和ConfigUnitPool中的配置信息不一致，更新ConfigUnitPool中的配置信息, ConfName={0}, Version={1}",
                                    confName, version),
                                    EnumLogLevel.Debug);
                                ConfigUnitPool.Instance.Set(confName, version, configUnitFromLocalFile);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    string message = string.Format("轮询检查本地配置文件发生异常，异常信息: {0}, {1}", ex.Message, ex.StackTrace);
                    settings.Logger.Log(message, EnumLogLevel.Error);
                }

                Thread.Sleep(this.Settings.ConfigWatcherPollingInterval);
            }
        }
    }
}
