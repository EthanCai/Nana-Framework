using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Configuration;
using System.Web;

namespace Nana.Framework.Utility
{
    /// <summary>
    /// 日志帮助类
    /// </summary>
    public class TxtLogHelper
    {
        #region singleton

        private static TxtLogHelper _instance = new TxtLogHelper();

        public static TxtLogHelper Instance
        {
            get { return _instance; }
        }

        #endregion

        /// <summary>
        /// 日志文件根目录
        /// </summary>
        private string _logRootPath = "";

        private const string TXT_LOG_PATH = "TxtLogPath";

        private TxtLogHelper()
        {
            _logRootPath = ConfigurationManager.AppSettings[TXT_LOG_PATH];

            if (string.IsNullOrEmpty(_logRootPath))
            {
                if (HttpContext.Current != null)
                {
                    _logRootPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @".\log\txt_log");
                }
                else
                {
                    _logRootPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\log\txt_log");
                }
            }

            if (!Directory.Exists(_logRootPath))
            {
                Directory.CreateDirectory(_logRootPath);
            }
        }

        /// <summary>
        /// 根据当前时间取得日志文件名
        /// </summary>
        private string LogFile
        {
            get
            {
                if (!Directory.Exists(_logRootPath))
                {
                    Directory.CreateDirectory(_logRootPath);
                }
                return Path.Combine(_logRootPath, string.Format("TxtLog_{0}.txt", DateTime.Now.ToString("yyyyMMdd-HH")));
            }
        }

        public void Log(string message, EnumConfigLogLevel level)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(DateTime.Now.ToString() + "  ");
            sb.Append(level.ToString() + "  ");
            sb.Append(message + "  ");
            FileAppend(LogFile, sb.ToString());
        }

        /// <summary>
        /// 追加文件
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <param name="content">内容</param>
        private static void FileAppend(string path, string content)
        {
            FileStream fs = null;
            StreamWriter sw = null;
            
            try
            {
                if (!File.Exists(path))
                {
                    fs = File.Open(path, FileMode.OpenOrCreate);
                    sw = new StreamWriter(fs, System.Text.Encoding.GetEncoding("utf-8"));
                    sw.WriteLine(content);
                }
                else
                {
                    fs = File.Open(path, FileMode.Append);
                    sw = new StreamWriter(fs, System.Text.Encoding.GetEncoding("utf-8"));
                    sw.WriteLine(content);
                }
            }
            finally
            {
                if (sw != null)
                {
                    sw.Close();
                    sw.Dispose();
                }
                if (fs != null)
                {
                    fs.Close();
                    fs.Dispose();
                }
            }
        }
    }

    public enum EnumConfigLogLevel
    {
        Debug = 10,
        Info = 20,
        Warn = 30,
        Error = 40,
        Fatal = 50,
    }
}
