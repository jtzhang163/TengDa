using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace CAMEL.RGV.Touchscreen
{
    public class Win32API
    {
        /// <summary>
        /// 打开系统输入法
        /// </summary>
        /// <param name="hhwnd"></param>
        /// <param name="msg"></param>
        /// <param name="wparam"></param>
        /// <param name="lparam"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        private static extern bool PostMessage(int hhwnd, uint msg, IntPtr wparam, IntPtr lparam);
        /// <summary>
        /// 关闭系统输入法
        /// </summary>
        /// <param name="pwszKLID"></param>
        /// <param name="Flags"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        private static extern IntPtr LoadKeyboardLayout(string pwszKLID, uint Flags);
        /// <summary>
        /// 数字软盘
        /// </summary>
        /// <param name="bvk"></param>
        /// <param name="bScan"></param>
        /// <param name="dwFlags"></param>
        /// <param name="dwExtraInfo"></param>
        [DllImport("user32.dll", EntryPoint = "keybd_event", SetLastError = true)]
        public static extern void keybd_event(byte bvk, byte bScan, uint dwFlags, uint dwExtraInfo);
        /// <summary>
        /// 窗口焦点
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="nIndex"></param>
        /// <param name="dwNewLong"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern int SetWindowLong(IntPtr hWnd, int nIndex, IntPtr dwNewLong);
    }
}
