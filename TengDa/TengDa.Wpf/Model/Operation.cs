using System;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data.Entity;
using System.Threading.Tasks;

namespace TengDa.Wpf
{
    /// <summary>
    /// 操作相关
    /// </summary>
    public class OperationHelper
    {
        /// <summary>
        /// 提示信息
        /// </summary>
        /// <param name="tips"></param>
        public static void ShowTips(string tips)
        {
            ShowTips(tips, false);
        }

        /// <summary>
        /// 提示信息
        /// </summary>
        /// <param name="tips"></param>
        /// <param name="showBox">是否显示弹窗</param>
        public static void ShowTips(string tips, bool isShowMessageBox)
        {
            Current.Tip.Tips += string.Format("{0} {1}\r\n", DateTime.Now.ToString("HH:mm:ss"), tips);
            Context.OperationContext.Operations.Add(new OperationLog(tips));
            Context.OperationContext.SaveChanges();
            if (isShowMessageBox)
            {
                Tip.Alert(tips);
            }
        }
    }


    /// <summary>
    /// 操作日志
    /// </summary>
    public class OperationLog : Record
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// 操作内容
        /// </summary>
        [MaxLength(50)]
        [Required]
        public string Content { get; set; }

        public OperationLog() : this(string.Empty)
        {
        }

        public OperationLog(string content)
        {
            UserId = Current.User.Id;
            DateTime = DateTime.Now;
            Content = content;
        }

    }

    public class OperationContext : DbContext
    {
        private static string connectionString = ConfigurationManager.ConnectionStrings["DefaultDatabase"].ToString();
        public OperationContext() : base(connectionString)
        {
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OperationLog>().ToTable("t_operation_log");
        }
        public DbSet<OperationLog> Operations { get; set; }
    }
}
