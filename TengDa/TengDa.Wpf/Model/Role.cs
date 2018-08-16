using System.Collections.Generic;

namespace TengDa.Wpf
{
    /// <summary>
    /// 用户组别
    /// </summary>
    public class Role
    {

        public Role() : this(-1)
        {

        }

        public Role(int id)
        {
            Id = id;
        }      


        #region 属性
        public int Id { get; set; }
        /// <summary>
        /// 等级
        /// </summary>
        public int Level { get; set; }
        /// <summary>
        /// 用户组别名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 该用户组别下所有用户
        /// </summary>
        public virtual ICollection<User> Users { get; set; }
        #endregion

    }
}
