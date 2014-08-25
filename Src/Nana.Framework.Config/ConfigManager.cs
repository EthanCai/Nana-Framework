using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nana.Framework.Config
{
    public class ConfigManager
    {

        public static T LoadFile<T>(string filePath) where T : IConfigUnit
        {
            string content = File.ReadAllText(filePath);
            var result = Activator.CreateInstance<T>();
            result.Load(content);

            ConfigUnitPool.Instance.Set(typeof(T).FullName, result);
            ConfigFileWatcherManager.Instance.InitFileWatcher(filePath, typeof(T));

            return result;
        }

        public static T Get<T>() where T : IConfigUnit
        {
            string fullName = typeof (T).FullName;
            return (T)ConfigUnitPool.Instance.Get(fullName);
        }

        public static void RegChangeHandler<T>(IConfigChangeHandler handler) where T : IConfigUnit
        {
            ConfigFileWatcherManager.Instance.RegisterConfigChangeHandler(typeof(T), handler);
        }

        public static void Clear()
        {
            ConfigFileWatcherManager.Instance.Clear();
            ConfigUnitPool.Instance.Clear();
        }
    }
}
