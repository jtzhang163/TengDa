using System.Configuration;
using System.Diagnostics;
using System.Linq;

namespace TengDa.Wpf
{
    /// <summary>
    /// 当前程序
    /// </summary>
    public static class AppCurrent
    {
        public static string ConnectionString = ConfigurationManager.ConnectionStrings["DefaultDatabase"].ToString();

        public static bool isMessageBoxShow = false;
        /// <summary>
        /// 软件是否已运行，防止重复打开软件
        /// </summary>
        public static bool AppIsRun
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
        /// 系统是否已经启动运行
        /// </summary>
        public static bool IsRunning { get; set; }

        /// <summary>
        /// 设备初始化是否完成
        /// </summary>
        public static bool IsTerminalInitFinished { get; set; } = false;

        public static User User = new User();

        public static OperationViewModel Operation = new OperationViewModel();

        public static YieldNowViewModel YieldNow = new YieldNowViewModel();

        public static TipViewModel Tip = new TipViewModel();

    }
}
