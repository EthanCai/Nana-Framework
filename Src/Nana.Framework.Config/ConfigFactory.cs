using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Nana.Framework.Config
{
    public class ConfigFactory
    {
        /// <summary>
        /// 获得配置
        /// </summary>
        /// <typeparam name="T">配置类泛型</typeparam>
        /// <returns>配置实例</returns>
        public static T GetConfig<T>() where T : ConfigUnit
        {
            return Config.GetConfigUnit(typeof(T)) as T;
        }

        /// <summary>
        /// 强制从本地文件重新加载配置；
        /// 仅重新加载已经加载过的配置，因为没有加载过的配置在第一次使用时会去加载最新配置文件到内存；
        /// </summary>
        public static void LoadConfigManually()
        {
            Config.LoadConfigManually();
        }


        /// <summary>
        /// 获取当前应用内存中所有已经加载的ConfigUnit
        /// </summary>
        /// <returns></returns>
        public static IList<ConfigUnit> GetAllConfigUnitListInMemory()
        {
            return Config.GetAllConfigUnitListInMemory();
        }

        /// <summary>
        /// 获取当前应用对应的所有配置文件
        /// </summary>
        /// <returns></returns>
        public static List<FileInfo> GetAllConfigFileListOfCurrentApp()
        {
            return Config.GetAllConfigFileListOfCurrentApp();
        }
    }
}
