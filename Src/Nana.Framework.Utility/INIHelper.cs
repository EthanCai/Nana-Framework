using System;
using System.Collections.Generic;
using System.IO;

namespace Nana.Framework.Utility
{
    /// <summary>
    /// INI文件帮助类
    /// </summary>
    public class INIHelper
    {
        private string _iniPath;
        private Dictionary<string, string> _dic;
        /// <summary>
        /// INIHelper
        /// </summary>
        /// <param name="iniPath">iniPath</param>
        public INIHelper(string iniPath)
        {
            if (string.IsNullOrEmpty(iniPath)) throw new Exception("INI文件路径不能为空!");
            _iniPath = iniPath;
            if (_dic == null)
            {
                _dic = new Dictionary<string, string>();
                INI2Dic();
            }
        }

        #region Dic2INI/INI2Dic
        /// <summary>
        /// 将DIC保存为INI文件
        /// </summary>
        private void Dic2INI()
        {
            if (_dic.Count == 0) return;
            string dirPath = Path.GetDirectoryName(_iniPath);
            if (!Directory.Exists(dirPath)) Directory.CreateDirectory(dirPath);
            if (File.Exists(_iniPath)) File.Delete(_iniPath);
            FileStream fs = File.Open(_iniPath, FileMode.OpenOrCreate);
            StreamWriter sw = new StreamWriter(fs);
            foreach (KeyValuePair<string, string> obj in _dic)
            {
                sw.WriteLine("{0}={1}", obj.Key, obj.Value);
            }
            sw.Close();
            fs.Close();
        }
        /// <summary>
        /// 将INI恢复为DIC
        /// </summary>
        private void INI2Dic()
        {
            if (!File.Exists(_iniPath)) return;
            FileStream fs = File.Open(_iniPath, FileMode.Open);
            StreamReader sr = new StreamReader(fs);
            string temp = string.Empty;
            string[] arr;
            while (!string.IsNullOrEmpty(temp = sr.ReadLine()))
            {
                arr = temp.Split('=');
                if (!_dic.ContainsKey(arr[0]))
                {
                    _dic.Add(arr[0], arr[1]);
                }
            }
            sr.Close();
            fs.Close();
        }
        #endregion

        #region Get,Set
        /// <summary>
        /// Get
        /// </summary>
        /// <param name="key">key</param>
        /// <returns></returns>
        public string Get(string key)
        {
            return _dic[key];
        }
        /// <summary>
        /// Set
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">value</param>
        public void Set(string key, string value)
        {
            if (_dic.ContainsKey(key))
            {
                _dic[key] = value;
            }
            else
            {
                _dic.Add(key, value);
            }
            Dic2INI();
        }
        /// <summary>
        /// 获得所有值
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> GetAll()
        {
            return _dic;
        }
        #endregion
    }
}
