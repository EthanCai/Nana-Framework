using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nana.Framework.Config
{
    class ConfigFileWatcherManager
    {
        private static ConfigFileWatcherManager instance = new ConfigFileWatcherManager();

        private ConfigFileWatcherManager() { }

        public static ConfigFileWatcherManager Instance
        {
            get { return instance; }
        }

        private Dictionary<string, Type> _watchTypeDictionary = new Dictionary<string, Type>();
        private Dictionary<string, FileWatcherHelper> _fileWatcherDictionary =
            new Dictionary<string, FileWatcherHelper>();
        private Dictionary<Type, IConfigChangeHandler> _typeHandlerMapping =
            new Dictionary<Type, IConfigChangeHandler>();

        public void InitFileWatcher(string filePath, Type configUnitType)
        {
            this._watchTypeDictionary[filePath] = configUnitType;

            string fileName = Path.GetFileName(filePath);
            string dirPath = Path.GetDirectoryName(filePath);
            FileWatcherHelper fileWatcher = new FileWatcherHelper(
                dirPath, fileName, new ConfigFileWatch(filePath));
            fileWatcher.Init();

            this._fileWatcherDictionary[filePath] = fileWatcher;
        }

        public void RegisterConfigChangeHandler(Type configUnitType, IConfigChangeHandler handler)
        {
            this._typeHandlerMapping[configUnitType] = handler;
        }

        public void Clear()
        {
            foreach (var key in _fileWatcherDictionary.Keys)
            {
                _fileWatcherDictionary[key].Dispose();
            }

            _fileWatcherDictionary.Clear();
            _watchTypeDictionary.Clear();
            _typeHandlerMapping.Clear();
        }

        class ConfigFileWatch : IFileWatcher
        {
            private string _filePath;

            public ConfigFileWatch(string filePath)
            {
                this._filePath = filePath;
            }

            public void Excute()
            {
                Type configUnitType = ConfigFileWatcherManager.Instance._watchTypeDictionary[this._filePath];

                string content = File.ReadAllText(this._filePath);

                var configUnit = ConfigUnitPool.Instance.Get(configUnitType.FullName);
                configUnit.Load(content);

                // 触发IConfigChangeHandler
                if (ConfigFileWatcherManager.Instance._typeHandlerMapping.ContainsKey(configUnitType))
                {
                    var changeHandler = ConfigFileWatcherManager.Instance._typeHandlerMapping[configUnitType];
                    changeHandler.OnChange(configUnit);
                }
            }
        }
    }
}
