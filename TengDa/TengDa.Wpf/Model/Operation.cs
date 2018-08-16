using System;
using System.ComponentModel.DataAnnotations;

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
            AppCurrent.Tip.Tips += string.Format("{0} {1}\r\n", DateTime.Now.ToString("HH:mm:ss"), tips);
            var log = new OperationLog
            {
                User = AppCurrent.User,
                DateTime = DateTime.Now,
                Content = tips
            };
            Context.OperationContext.Operations.Add(log);
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
        /// 操作内容
        /// </summary>
        [MaxLength(200)]
        public string Content { get; set; }

        public OperationLog() : this(string.Empty)
        {
        }

        public OperationLog(string content)
        {
            User = AppCurrent.User;
            DateTime = DateTime.Now;
            Content = content;
        }

    }

}
