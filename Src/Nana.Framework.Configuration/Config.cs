using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using Nana.Framework.Utility;

namespace Nana.Framework.Configuration
{
    public class Config
    {
        #region 变量

        protected static IList<ConfigUnit> _configUnitList = new List<ConfigUnit>();
        protected static Dictionary<string, ConfigUnit> _configUnitHash = new Dictionary<string, ConfigUnit>();

        private static Dictionary<string, string> _configFileFullPathDic = null;
        private static readonly object _configFileFullPathDicLock = new object();

        private static readonly object _startLock = new object();

        /// <summary>
        /// 开始启动配置文件标识
        /// </summary>
        private static bool _isStartWatch = false;

        #endregion

        #region 属性

        /// <summary>
        /// 配置文件文件夹的路径
        /// </summary>
        public static string ConfigFileFolder
        {
            get
            {
                string result = null;

                string isDebug = ConfigurationManager.AppSettings["IsDebug"];

                //本地测试环境需要在App.config或者web.config中设置IsDebug参数
                //如果IsDebug为true，则从与Solution目录平级的conf目录下读取配置
                //如果IsDebug为false，则从程序（或网站）部署目录下的conf目录读取配置
                if (!string.IsNullOrEmpty(isDebug)
                    && "true".Equals(isDebug.Trim(), StringComparison.InvariantCultureIgnoreCase))
                {
                    result = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, 
                        @"..\..\..\conf\");
                }
                else
                {
                    result = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, 
                        HttpContext.Current != null ? @".\conf\" : @"..\conf\");
                }

                return result;
            }
        }

        /// <summary>
        /// 配置文件全路径Dictionary《文件名，全路径》
        /// </summary>
        public static Dictionary<string, string> ConfigFileFullPathDic
        {
            get
            {
                if (_configFileFullPathDic == null)
                {
                    InitializeConfigFileFullPathDic();
                }
                return _configFileFullPathDic;
            }
            set
            {
                _configFileFullPathDic = value;
            }
        }

        private static void InitializeConfigFileFullPathDic()
        {
            try
            {
                Monitor.Enter(_configFileFullPathDicLock);
                if (_configFileFullPathDic == null)
                {
                    _configFileFullPathDic = new Dictionary<string, string>();
                    List<string> allConfigFiles = Directory.GetFiles(
                        ConfigFileFolder, "*.config", SearchOption.AllDirectories).ToList();
                    foreach (string item in allConfigFiles)
                    {
                        string configFileName = Path.GetFileName(item).ToLower();
                        string configFileFullPath = Path.GetFullPath(item).ToLower();
                        if (_configFileFullPathDic.ContainsKey(configFileName))
                        {
                            throw new Exception(string.Format(
                                "错误：配置文件根目录下存在多个名为'{0}'的配置文件！", configFileName));
                        }
                        _configFileFullPathDic.Add(configFileName, configFileFullPath);
                    }
                }
            }
            finally
            {
                Monitor.Exit(_configFileFullPathDicLock);
            }
        }

        #endregion

        #region 方法

        /// <summary>
        /// 获得指定的配置
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        internal static ConfigUnit GetConfigUnit(Type type)
        {
            try
            {
                if (!_configUnitHash.Keys.Contains(type.ToString()))
                {
                    GetConfigFromFile(type);
                }
                return (ConfigUnit)_configUnitHash[type.ToString()];
            }
            catch (InitConfigException)
            {
                throw;
            }
            catch (Exception ex)
            {
                try
                {
                    TxtLogHelper.Instance.Log(string.Format(
                        "获取配置信息出现异常，ConfigType={0}，错误信息：{1}。{2}",
                        type.FullName, ex.Message, ex.StackTrace), EnumConfigLogLevel.Error);
                }
                catch { }
                return null;
            }
        }

        internal static void LoadConfigManually()
        {
            TxtLogHelper.Instance.Log(string.Format("开始强制重新加载内存中已经存在的配置！"), EnumConfigLogLevel.Info);
            OnFileUpdate();
            TxtLogHelper.Instance.Log(string.Format("强制重新加载内存中已经存在的配置结束！"), EnumConfigLogLevel.Info);
        }

        /// <summary>
        /// 获取内存中所有已经加载的ConfigUnit
        /// </summary>
        /// <returns></returns>
        internal static IList<ConfigUnit> GetAllConfigUnitListInMemory()
        {
            return _configUnitList;
        }

        /// <summary>
        /// 获取当前应用对应的所有配置文件
        /// </summary>
        /// <returns></returns>
        internal static List<FileInfo> GetAllConfigFileListOfCurrentApp()
        {
            List<FileInfo> rst = new List<FileInfo>();
            DirectoryInfo di = new DirectoryInfo(ConfigFileFolder);
            FileInfo[] files = di.GetFiles("*.config", SearchOption.AllDirectories);
            if (files != null && files.Length > 0)
            {
                rst = files.ToList();
            }
            return rst;
        }

        #region private methods

        /// <summary>
        /// Type类型的配置在_ConfigUnitHash中不存在时，从文件中加载到内存
        /// </summary>
        /// <param name="configType">具体配置类类型</param>
        private static void GetConfigFromFile(Type configType)
        {
            try
            {
                Monitor.Enter(_startLock);
                #region 将配置实例加入配置列表

                //如果有其他线程将配置加载到内存了，那么不需要再次加载配置
                if (_configUnitHash.Keys.Contains(configType.ToString()))
                {
                    return;
                }

                if (!configType.IsSubclassOf(typeof(ConfigUnit)))
                {
                    throw new Exception(string.Format(
                        "传入参数配置类型‘{0}’不是配置基类‘{1}’的子类！", configType.FullName, typeof(ConfigUnit).FullName));
                }

                #endregion

                _configUnitList.Add((ConfigUnit)Activator.CreateInstance(configType));

                //从本地配置文件加载配置到内存
                TxtLogHelper.Instance.Log(string.Format("=====开始加载配置{0}（本地文件）=====", configType.FullName), EnumConfigLogLevel.Info);
                OnFileUpdate();
                TxtLogHelper.Instance.Log(string.Format("=====完成加载配置{0}（本地文件）=====" + Environment.NewLine, configType.FullName), EnumConfigLogLevel.Info);

                //启动线程，每隔5分钟检查一下文件修改时间是否改变，确定是否重新加载配置；保证配置修改后会生效
                if (!_isStartWatch)
                {
                    Thread t = new Thread(FileSpool);
                    t.Start();
                    _isStartWatch = true;
                }

            }
            catch (InitConfigException e)
            {
                throw e;
            }
            catch (Exception ex)
            {
                TxtLogHelper.Instance.Log(string.Format("GetConfigFromFile错误 将无法获取对应的数据！错误信息：{0}，{1}", ex.Message, ex.StackTrace),
                        EnumConfigLogLevel.Fatal);
            }
            finally
            {
                Monitor.Exit(_startLock);
            }
        }

        #endregion

        #region 根据本地文件更新配置数据

        private const int MAX_RETRY_TIME = 3;
        private const int MIN_RETRY_MS = 100;
        private const int MAX_RETRY_MS = 200;
        private static Random ran = new Random(DateTime.Now.Millisecond);

#if DEBUG
        private const int UpdateInterval = 1000; 
#else
        private const int UpdateInterval = 1000 * 60 * 5;   // 更新间隔 单位ms 当前值 5分钟
#endif

        /// <summary>
        /// 定时轮巡本地配置文件，以便将最新配置加载到内存中
        /// </summary>
        /// <param name="para">配置文件所在路径</param>
        private static void FileSpool(object para)
        {
            while (true)
            {
                try
                {
                    //读取当前文件修改时间，并存入哈希表
                    DirectoryInfo di = new DirectoryInfo(ConfigFileFolder);
                    Dictionary<string, FileInfo> fileHash = di.GetFiles("*.config", SearchOption.AllDirectories).ToDictionary(p => p.Name);

                    while (true)
                    {
                        try
                        {
                            //轮巡时间间隔
                            Thread.Sleep(UpdateInterval);

                            di = new DirectoryInfo(ConfigFileFolder);
                            FileInfo[] files = di.GetFiles("*.config", SearchOption.AllDirectories);
                            foreach (FileInfo fi in files)
                            {
                                //配置文件被修改，重新加载配置
                                if (fileHash.ContainsKey(fi.Name)
                                    && fi.LastWriteTime != fileHash[fi.Name].LastWriteTime)
                                {
                                    TxtLogHelper.Instance.Log(
                                        string.Format("主动监测发现{0}在{1}被修改（本地文件）！", fi.Name, fi.LastWriteTime), 
                                        EnumConfigLogLevel.Warn);

                                    OnFileUpdate();
                                    fileHash = di.GetFiles("*.config", SearchOption.AllDirectories).ToDictionary(p => p.Name);
                                    break;
                                }
                                //如果fileHash中不包含这个文件，说明这个配置是新加进来的；
                                //也需要加入到监控的文件列表中
                                else if (!fileHash.ContainsKey(fi.Name))
                                {
                                    fileHash[fi.Name] = fi;
                                }
                            }
                        }
                        catch (Exception ee)
                        {
                            TxtLogHelper.Instance.Log(string.Format(
                                "轮询时发生异常（本地文件），异常信息：{0}；{1}", ee.Message, ee.StackTrace), 
                                EnumConfigLogLevel.Error);
                        }
                    }
                }
                catch (Exception ee)
                {
                    TxtLogHelper.Instance.Log(string.Format(
                        "轮询时发生异常（本地文件），可能无法读取配置文件FileInfo,程序将重新启动轮询。异常信息：{0}；{1}", 
                        ee.Message, ee.StackTrace),
                        EnumConfigLogLevel.Error);
                }

                Thread.Sleep(ran.Next(MIN_RETRY_MS, MAX_RETRY_MS));
            }
        }

        /// <summary>
        /// 将_ConfigUnitList中的每个ConfigUnit重新加载一次，并更新到_ConfigUnitHash中；
        /// </summary>
        /// <param name="status">配置文件所在目录（Config.Path）</param>
        private static void OnFileUpdate()
        {
            //更新_ConfigUnitList
            for (int i = 0; i < _configUnitList.Count; i++)
            {
                int triedTimes = 0;
                while (triedTimes < MAX_RETRY_TIME && !UpdateOneConfig(_configUnitList[i]))
                {
                    triedTimes++;
                    Thread.Sleep(ran.Next(MIN_RETRY_MS, MAX_RETRY_MS));
                    if (triedTimes < MAX_RETRY_TIME)
                    {
                        TxtLogHelper.Instance.Log(string.Format("第{1}次重试,最多重试{2}次，开始加载配置(本地文件){0}"
                            , _configUnitList[i].GetType().FullName, triedTimes, MAX_RETRY_TIME - 1), 
                            EnumConfigLogLevel.Info);
                    }
                }
            }

            //更新_ConfigUnitHash，将配置实例的透明代理放到hash中
            foreach (ConfigUnit unit in _configUnitList)
            {
                _configUnitHash[unit.GetType().ToString()] = unit;
            }
        }

        /// <summary>
        /// 加载一个配置到内存
        /// </summary>
        /// <param name="status">路径</param>
        /// <param name="i">数组下标</param>
        /// <returns>是否加载成功</returns>
        private static bool UpdateOneConfig(ConfigUnit cu)
        {
            bool retVal = true;
            try
            {
                TxtLogHelper.Instance.Log(
                    string.Format("开始从本地文件‘{0}’加载配置！", cu.FullPath), 
                    EnumConfigLogLevel.Info);

                lock (cu)
                {
                    // 调用Reload重新加载配置内容，而不是生成一个新的对象，原因是对于基础组件来说，
                    // 上层业务组件可能依赖基础对象，基础对象的管理必须要严格，不能随意抛弃，创建基础对象
                    cu.Reload();
                }
            }
            catch (Exception e)
            {
                TxtLogHelper.Instance.Log(
                    string.Format("加载配置{1}失败（本地文件），失败原因：{2}，线程ID：{0}",
                        Thread.CurrentThread.ManagedThreadId, cu.GetType().FullName, e.Message), 
                    EnumConfigLogLevel.Error);

                retVal = false;
            }
            return retVal;
        }

        #endregion 根据本地文件更新配置数据

        #endregion
    }
}
