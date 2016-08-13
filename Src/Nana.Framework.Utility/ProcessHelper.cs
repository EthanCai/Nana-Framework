using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Nana.Framework.Utility
{
    /// <summary>
    /// 线程的一些基本操作类
    /// add by min.jiang 2011-3-22
    /// </summary>
    public class ProcessHelper
    {
        /// <summary>
        /// Start Process
        /// </summary>
        /// <param name="processName"></param>
        public static void StartProcess(string processName, bool waitfor, ProcessWindowStyle windowStyle)
        {
            Process proc = new Process();
            try
            {


                proc.StartInfo.FileName = processName;

                proc.StartInfo.WindowStyle = windowStyle;

                proc.Start();

                if (waitfor)
                    proc.WaitForExit();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                proc.Close();
            }

        }

        /// <summary>
        /// Kill Process...
        /// </summary>
        /// <param name="processName"></param>
        public static void KillProcess(string processName)
        {
            try
            {
                foreach (Process proc in Process.GetProcessesByName(processName))
                {
                    if (!proc.CloseMainWindow())
                    {
                        if (proc != null)
                            proc.Kill();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 指定进度是否处理激活状态
        /// </summary>
        /// <param name="processName"></param>
        /// <returns></returns>
        public static bool ProcessIsAlive(string processName)
        {
            try
            {
                foreach (Process proc in Process.GetProcessesByName(processName))
                {
                    if (!proc.CloseMainWindow())
                    {
                        if (proc != null)
                            return true;
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return false;
        }
        /// <summary>
        /// 运行命令，返回控制台内容
        /// </summary>
        /// <param name="Arguments">参数</param>
        /// <param name="FileName">命令名称</param>
        /// <returns>控制台内容</returns>
        public static string RunCMD(string Arguments, string FileName)
        {
            System.Diagnostics.ProcessStartInfo Info = new System.Diagnostics.ProcessStartInfo();
            Info.Arguments = Arguments;  
            Info.FileName = FileName;
            Info.RedirectStandardOutput = true;
            Info.UseShellExecute = false;
            Info.CreateNoWindow = true;
            Process pro = Process.Start(Info);
            pro.WaitForExit();
            return pro.StandardOutput.ReadToEnd();
        }

        /// <summary>
        /// 执行DOS命令，返回DOS命令的输出
        /// </summary>
        /// <param name="dosCommand">dos命令</param>
        /// <param name="milliseconds">等待命令执行的时间（单位：毫秒），如果设定为0，则无限等待</param>
        /// <returns>返回输出，如果发生异常，返回空字符串</returns>
        public static string Execute(string dosCommand, int milliseconds)
        {
            string output = "";     //输出字符串
            if (dosCommand != null && dosCommand != "")
            {
                Process process = new Process();     //创建进程对象
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.FileName = "cmd.exe";      //设定需要执行的命令
                startInfo.Arguments = "/C " + dosCommand;   //设定参数，其中的“/C”表示执行完命令后马上退出
                startInfo.UseShellExecute = false;     //不使用系统外壳程序启动
                startInfo.RedirectStandardInput = false;   //不重定向输入
                startInfo.RedirectStandardOutput = true;   //重定向输出
                startInfo.CreateNoWindow = true;     //不创建窗口
                process.StartInfo = startInfo;
                try
                {
                    if (process.Start())       //开始进程
                    {
                        if (milliseconds == 0)
                            process.WaitForExit();     //这里无限等待进程结束
                        else
                            process.WaitForExit(milliseconds);  //这里等待进程结束，等待时间为指定的毫秒
                        output = process.StandardOutput.ReadToEnd();//读取进程的输出
                    }
                }
                catch
                {
                }
                finally
                {
                    if (process != null)
                        process.Close();
                }
            }
            return output;
        }
    }
}
