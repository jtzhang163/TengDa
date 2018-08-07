using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace TengDa.Wpf
{
    /// <summary>
    /// 当前程序
    /// </summary>
    public static class Current
    {
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

        public static void AddOperation(string content)
        {
            Context.OperationContext.Operations.Add(new Operation(content));

            try
            {
                Task task = new Task(() => { Context.OperationContext.SaveChanges(); });
                task.Start();
                task.Wait();
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex);
            }
        }

        public static UserGroup UserGroup => Context.UserContext.UserGroups.FirstOrDefault(g => g.Id == User.GroupId) ?? new UserGroup();

        public static List<User> Users => Context.UserContext.Users.ToList();

        public static List<UserGroup> UserGroups => Context.UserContext.UserGroups.ToList();

        public static List<Operation> Operations => Context.OperationContext.Operations.ToList();

        public static UserOperationViewModel UserOperationViewModel = new UserOperationViewModel();

        public static RealtimeYieldViewModel RealtimeYieldViewModel = new RealtimeYieldViewModel();

        public static TipViewModel TipViewModel = new TipViewModel();
    }
}
