using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace TengDa.Wpf
{
    /// <summary>
    /// 日志型数据基类
    /// </summary>
    public abstract class Record : Service
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public int UserId { get; set; }

        [NotMapped]
        public virtual User User
        {
            get => Context.UserContext.Users.SingleOrDefault(u => u.Id == UserId) ?? new User();
            set => UserId = value.Id;
        }

        /// <summary>
        /// 记录时间
        /// </summary>
        public DateTime DateTime { get; set; }

        protected Record()
        {
            this.DateTime = DateTime.Now;
        }
    }
}
