using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace TengDa.WF
{
    public class Current
    {
        public static bool isMessageBoxShow = false;
        /// <summary>
        /// 软件是否已运行，防止重复打开软件
        /// </summary>
        public static bool IsRun
        {
            get
            {
                string proName = System.IO.Path.GetFileName(System.Reflection.Assembly.GetEntryAssembly().GetName().Name);
                Process[] pro = Process.GetProcesses();
                int n = pro.Where(p => p.ProcessName.Equals(proName)).Count();
                return n > 1 ? true : false;
            }
        }
        /// <summary>
        /// 软件是否已启动运行，用于防止初始化控件时触发某些事件
        /// </summary>
        public static bool IsRunning { get; set; }

        /// <summary>
        /// 设备初始化是否完成
        /// </summary>
        public static bool IsTerminalInitFinished { get; set; } = false;

        public static User user = new User();
    }
}
