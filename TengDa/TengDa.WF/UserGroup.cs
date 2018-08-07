using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace TengDa.WF
{
    /// <summary>
    /// 用户组别
    /// </summary>
    public class UserGroup
    {
        #region 属性
        public int Id { get; private set; }

        public int Level { get; private set; }

        public string Name { get; private set; }
        #endregion

        #region 构造函数
        public UserGroup() : this(-1) { }

        public UserGroup(int id)
        {
            if (id < 0)
            {
                this.Id = -1;
                return;
            }

            DataTable dt = null;

            string msg = string.Empty;

            dt = Database.Query("SELECT * FROM [dbo].[TengDa.UserGroup] WHERE Id = " + id, out msg);

            if (!string.IsNullOrEmpty(msg))
            {
                Error.Alert(msg);
                LogHelper.WriteError(msg);
                return;
            }

            if (dt == null || dt.Rows.Count == 0) { return; }

            Init(dt.Rows[0]);

            //释放资源
            dt.Dispose();
        }

        public UserGroup(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                this.Id = -1;
                return;
            }

            DataTable dt = null;

            string msg = string.Empty;

            dt = Database.Query(string.Format("SELECT * FROM [dbo].[TengDa.UserGroup] WHERE [Name] = '{0}'", name), out msg);

            if (!string.IsNullOrEmpty(msg))
            {
                Error.Alert(msg);
                return;
            }

            if (dt == null || dt.Rows.Count == 0) { return; }

            Init(dt.Rows[0]);

            //释放资源
            dt.Dispose();
        }

        #endregion

        #region 初始化方法

        public void Init(DataRow rowInfo)
        {
            if (rowInfo == null)
            {
                this.Id = -1;
                return;
            }
            InitFields(rowInfo);
        }

        protected void InitFields(DataRow rowInfo)
        {
            this.Id = TengDa._Convert.StrToInt(rowInfo["Id"].ToString(), -1);
            this.Name = rowInfo["Name"].ToString();
            this.Level = TengDa._Convert.StrToInt(rowInfo["Level"].ToString(), -1);
        }
        #endregion

        public static List<UserGroup> UserGroups
        {
            get
            {
                List<UserGroup> userGroups = new List<UserGroup>();
                string msg = string.Empty;
                DataTable dt = Database.Query(string.Format("SELECT * FROM [dbo].[TengDa.UserGroup]"), out msg);

                if (!string.IsNullOrEmpty(msg))
                {
                    Error.Alert(msg);
                    return userGroups;
                }

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    UserGroup userGroup = new UserGroup();
                    userGroup.InitFields(dt.Rows[i]);
                    userGroups.Add(userGroup);
                }

                //释放资源
                dt.Dispose();
                return userGroups;
            }
        }
    }
}
