using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Nana.Framework.Common
{
    /// <summary>
    /// WinformHelper
    /// </summary>
    public class WinformHelper
    {
        #region SetFullScreenWin32API
        [DllImport("user32.dll", EntryPoint = "ShowWindow")]
        private static extern int ShowWindow(int hwnd, int nCmdShow);
        private const int SW_SHOW = 5;
        private const int SW_HIDE = 0;

        [DllImport("user32.dll", EntryPoint = "SystemParametersInfo")]
        private static extern int SystemParametersInfo(int uAction, int uParam, ref Rectangle lpvParam, int fuWinIni);
        private const int SPIF_UPDATEINIFILE = 0x1;
        private const int SPI_SETWORKAREA = 47;
        private const int SPI_GETWORKAREA = 48;

        [DllImport("user32.dll", EntryPoint = "FindWindow")]
        private static extern int FindWindow(string lpClassName, string lpWindowName);
        #endregion
        /// <summary>
        /// 设置全屏/取消全屏
        /// </summary>
        /// <param name="fullscreen">true:全屏/false:恢复</param>
        /// <param name="rectOld">设置的时候此参数返回原始尺寸，恢复时用此参数设置恢复</param>
        /// <returns>操作结果</returns>
        private static bool SetFullScreen(bool fullscreen, ref Rectangle rectOld)
        {
            int Hwnd = 0;
            Hwnd = FindWindow("Shell_TrayWnd", null);
            if (Hwnd == 0) return false;
            if (fullscreen)
            {
                ShowWindow(Hwnd, SW_HIDE);
                Rectangle rectFull = Screen.PrimaryScreen.Bounds;
                SystemParametersInfo(SPI_GETWORKAREA, 0, ref rectOld, SPIF_UPDATEINIFILE);//get
                SystemParametersInfo(SPI_SETWORKAREA, 0, ref rectFull, SPIF_UPDATEINIFILE);//set
            }
            else
            {
                ShowWindow(Hwnd, SW_SHOW);
                SystemParametersInfo(SPI_SETWORKAREA, 0, ref rectOld, SPIF_UPDATEINIFILE);
            }
            return true;
        }
        /// <summary>
        /// 设置全屏/取消全屏
        /// </summary>
        /// <param name="form">窗体</param>
        /// <param name="fullScreen">全屏</param>
        public static void SetFullScreen(Form form, bool fullScreen)
        {
            Rectangle rect = new Rectangle();
            WinformHelper.SetFullScreen(fullScreen, ref rect);
            if (fullScreen)
            {
                form.FormBorderStyle = FormBorderStyle.None;
                form.WindowState = FormWindowState.Maximized;
            }
            else
            {
                form.FormBorderStyle = FormBorderStyle.Sizable;
                form.WindowState = FormWindowState.Normal;
            }
        }
    }
}
