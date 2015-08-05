using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;

namespace Nana.Framework.Utility
{
    /// <summary>
    /// 注册表相关辅助类
    /// add by min.jiang 2011-3-23
    /// </summary>
   public  class RegistryKeyHelp
    {
        /// <summary>  
        /// 开机启动项  
        /// </summary>  
        /// <param name=\"Started\">是否启动</param>  
        /// <param name=\"name\">启动值的名称</param>  
        /// <param name=\"path\">启动程序的路径</param>  
        public static  void RunWhenStart(bool Started, string name, string path)
        {
            RegistryKey HKLM = Registry.LocalMachine;
            RegistryKey Run = HKLM.CreateSubKey(@"SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run");
            if (Started == true)
            {
                try
                {
                    Run.SetValue(name, path);
                    HKLM.Close();
                }
                catch (Exception ex)
                {
                    throw ex;

                }
            }
            else
            {
                try
                {
                    Run.DeleteValue(name);
                    HKLM.Close();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}
