using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using TengDa.Encrypt;

namespace TengDa.WF
{
    /// <summary>
    /// 用户
    /// </summary>
    public class User
    {
        #region 属性定义
        public int Id { get; private set; }

        private string name = string.Empty;
        /// <summary>
        /// 用户名称
        /// </summary>
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                //if (Regex.IsMatch(value, RegexString.UserName))
                //{
                name = value;
                //}
                //else
                //{
                //    Error.Alert("用户名不符合规则：【" + RegexString.UserName + "】");
                //}
            }
        }

        private string password = string.Empty;
        /// <summary>
        /// 用户登录密码
        /// </summary>
        public string Password
        {
            get
            {
                return password;
            }
            set
            {
                //if (Regex.IsMatch(value, RegexString.Password))
                //{
                password = Base64.EncodeBase64(value);
                //}
                //else
                //{
                //    Error.Alert("密码不符合规则：【" + RegexString.Password + "】");
                //}
            }
        }

        /// <summary>
        /// 用户所属类别
        /// </summary>
        public int GroupId { get; private set; }
        public UserGroup Group
        {
            get
            {
                return new UserGroup(GroupId);
            }
            set
            {
                GroupId = new UserGroup(value.Name).Id;
            }
        }

        public DateTime RegisterTime { get; private set; }
        public DateTime LastLoginTime { get; private set; }
        public int LoginTimes { get; private set; }
        public bool IsEnable { get; set; }

        private string number = string.Empty;
        public string Number
        {
            get
            {
                return number;
            }

            set
            {
                number = value;
            }
        }
        #endregion

        #region 构造函数

        public User() : this(-1) { }

        public User(int id)
        {
            if (id < 0)
            {
                this.Id = -1;
                return;
            }

            string msg = string.Empty;
            DataTable dt = Database.Query("SELECT * FROM [dbo].[TengDa.Users] WHERE Id = " + id, out msg);

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

        public User(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                this.Id = -1;
                return;
            }

            string msg = string.Empty;
            DataTable dt = Database.Query(string.Format("SELECT * FROM [dbo].[TengDa.Users] WHERE [Name] = '{0}'", name), out msg);

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

        #region 初始化
        protected void Init(DataRow rowInfo)
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
            this.Number = rowInfo["Number"].ToString();
            this.Password = Base64.DecodeBase64(rowInfo["Password"].ToString());
            this.GroupId = TengDa._Convert.StrToInt(rowInfo["GroupId"].ToString(), -1);
            this.RegisterTime = TengDa._Convert.StrToDateTime(rowInfo["RegisterTime"].ToString(), TengDa.Common.DefaultTime);
            this.LastLoginTime = TengDa._Convert.StrToDateTime(rowInfo["LastLoginTime"].ToString(), TengDa.Common.DefaultTime);
            this.LoginTimes = TengDa._Convert.StrToInt(rowInfo["LoginTimes"].ToString(), 0);
            this.IsEnable = Convert.ToBoolean(rowInfo["IsEnable"]);
        }

        #endregion

        #region 增删查改

        public static List<User> GetList(string sql, out string msg)
        {
            List<User> list = new List<User>();
            DataTable dt = Database.Query(sql, out msg);

            if (!string.IsNullOrEmpty(msg))
            {
                Error.Alert(msg);
                return list;
            }

            if (dt == null || dt.Rows.Count == 0)
            {
                return list;
            }
            else
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    User user = new User();
                    user.InitFields(dt.Rows[i]);
                    list.Add(user);
                }
            }
            return list;
        }

        public static bool Login(string name, string password, out User outUser, out string msg)
        {
            try
            {
                List<User> list = GetList(string.Format("SELECT * FROM [dbo].[TengDa.Users] WHERE [Name] = '{0}' AND [Password] = '{1}'", name, Base64.EncodeBase64(password)), out msg);
                if (list.Count() > 0)
                {
                    outUser = list[0];
                    if (outUser.IsEnable)
                    {
                        return Database.NonQuery(string.Format("UPDATE [dbo].[TengDa.Users] SET [LastLoginTime] = getdate(),[LoginTimes] = [LoginTimes] + 1 WHERE [Name] = '{0}'", outUser.Name), out msg);
                    }
                    else
                    {
                        msg = "该用户尚未审核";
                    }
                }
                else
                {
                    msg = "用户名或密码错误";
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }

            outUser = null;
            return false;
        }

        public static bool MesLogin2(string number, string password, out User outUser, out string msg)
        {
            try
            {
                List<User> list = GetList(string.Format("SELECT * FROM [dbo].[TengDa.Users] WHERE [Number] = '{0}' AND [Password] = '{1}'", number, Base64.EncodeBase64(password)), out msg);
                if (list.Count() > 0)
                {
                    outUser = list[0];
                    return Database.NonQuery(string.Format("UPDATE [dbo].[TengDa.Users] SET [LastLoginTime] = getdate(),[LoginTimes] = [LoginTimes] + 1 WHERE [Number] = '{0}'", outUser.Number), out msg);
                }
                else
                {
                    msg = "用户工号错误！";
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }

            outUser = null;
            return false;
        }

        public static bool MesLogin(string number, out User outUser, out string msg)
        {
            try
            {
                List<User> list = GetList(string.Format("SELECT * FROM [dbo].[TengDa.Users] WHERE [Number] = '{0}'", number), out msg);
                if (list.Count() > 0)
                {
                    outUser = list[0];
                    return Database.NonQuery(string.Format("UPDATE [dbo].[TengDa.Users] SET [LastLoginTime] = getdate(),[LoginTimes] = [LoginTimes] + 1 WHERE [Number] = '{0}'", outUser.Number), out msg);
                }
                else
                {
                    msg = "用户工号错误！";
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }

            outUser = null;
            return false;
        }

        public static bool Add(User addUser, out string msg)
        {
            try
            {
                List<User> list = GetList(string.Format("SELECT * FROM [dbo].[TengDa.Users] WHERE [Name] = '{0}'", addUser.Name), out msg);
                if (list.Count() > 0)
                {
                    msg = "该用户已存在！ Name：" + addUser.Name;
                    return false;
                }

                return Database.NonQuery(string.Format("INSERT INTO [dbo].[TengDa.Users] ([Name], [Password], [GroupId], [RegisterTime], [LastLoginTime], [LoginTimes], [IsEnable], [Number]) VALUES ('{0}', '{1}', {2}, '{3}', '2000-01-01 00:00:00', 0, '{4}', '{5}')", addUser.Name, addUser.Password, addUser.GroupId, DateTime.Now, addUser.IsEnable, addUser.Number), out msg);
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return false;
            }
        }

        public static bool Delete(User delUser, out string msg)
        {
            return Database.NonQuery(string.Format("DELETE FROM	[dbo].[TengDa.Users] WHERE [Name] = '{0}'", delUser.Name), out msg);
        }

        public static bool Update(User newUser, out string msg)
        {
            return Database.NonQuery(string.Format("UPDATE [dbo].[TengDa.Users] SET GroupId = {0} WHERE [Name] = '{1}'", newUser.GroupId, newUser.Name), out msg);
        }

        public static bool Verify(User user, out string msg)
        {
            return Database.NonQuery(string.Format("UPDATE [dbo].[TengDa.Users] SET [IsEnable] = 'true' WHERE [Name] = '{0}'", user.Name), out msg);
        }
        #endregion
    }
}
