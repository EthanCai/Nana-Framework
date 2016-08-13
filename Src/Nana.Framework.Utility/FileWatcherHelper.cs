using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace Nana.Framework.Utility
{
    /// <summary>
    /// IFileWatcher接口
    /// </summary>
    public interface IFileWatcher
    {
        /// <summary>
        /// Excute
        /// </summary>
        void Excute();
    }

    /// <summary>
    /// 文件监听帮助类
    /// </summary>
    public class FileWatcherHelper
    {
        private FileSystemWatcher _watcher;
        private string _dirPath;
        private string _fileName;
        private IFileWatcher _excute;
        private object _lock = new object();
        private Dictionary<string, DateTime> _dic;

        /// <summary>
        /// FileWatcherHelper
        /// </summary>
        /// <param name="dirPath">dirPath</param>
        /// <param name="fileName">fileName</param>
        /// <param name="execute">execute</param>
        public FileWatcherHelper(string dirPath, string fileName, IFileWatcher execute)
        {
            _dirPath = dirPath;
            _fileName = fileName;
            _excute = execute;
            _dic = new Dictionary<string, DateTime>();
        }

        /// <summary>
        /// Init
        /// </summary>
        public void Init()
        {
            _watcher = new FileSystemWatcher(_dirPath, _fileName);
            _watcher.EnableRaisingEvents = true;
            _watcher.Changed += new FileSystemEventHandler(watcher_Changed);
        }

        private void watcher_Changed(object sender, FileSystemEventArgs e)
        {
            Thread.Sleep(100);
            lock (_lock)
            {
                try
                {
                    DateTime lastModifyTime = File.GetLastWriteTime(e.FullPath);
                    if ((_dic.Count != 0 && _dic[e.FullPath.ToLower()] < lastModifyTime) || _dic.Count == 0)
                    {
                        _excute.Excute();
                    }
                    _dic[e.FullPath.ToLower()] = lastModifyTime;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}
