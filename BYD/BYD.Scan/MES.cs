using Microsoft.CSharp;
using System;
using System.Configuration;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Services.Description;
using TengDa;
using TengDa.Web;
using TengDa.WF;
using BYD.Scan.MesService;

namespace BYD.Scan
{
    public class MES : TengDa.WF.MES.MES
    {

        #region 字段属性
        protected string webServiceUrl = string.Empty;
        /// <summary>
        /// Web服务地址
        /// </summary>
        [DisplayName("Web服务地址")]
        [Category("基本设置")]
        public string WebServiceUrl
        {
            get
            {
                return webServiceUrl;
            }
            set
            {
                if (webServiceUrl != value)
                {
                    UpdateDbField("WebServiceUrl", value);
                }
                webServiceUrl = value;
            }
        }


        protected string username = string.Empty;
        /// <summary>
        /// 用户名
        /// </summary>
        [DisplayName("用户名")]
        [Category("基本设置")]
        public string Username
        {
            get
            {
                return username;
            }
            set
            {
                if (username != value)
                {
                    UpdateDbField("Username", value);
                }
                username = value;
            }
        }


        protected string password = string.Empty;
        /// <summary>
        /// 密码
        /// </summary>
        [DisplayName("密码")]
        [Category("基本设置")]
        public string Password
        {
            get
            {
                return password;
            }
            set
            {
                if (password != value)
                {
                    UpdateDbField("Password", value);
                }
                password = value;
            }
        }

        //private string username = string.Empty;
        //public string Username
        //{
        //    get
        //    {
        //        if (string.IsNullOrEmpty(username))
        //        {
        //            username = ConfigurationManager.AppSettings["mes_username"].ToString();
        //        }
        //        return username;
        //    }
        //}

        //private string password = string.Empty;
        //public string Password
        //{
        //    get
        //    {
        //        if (string.IsNullOrEmpty(password))
        //        {
        //            password = ConfigurationManager.AppSettings["mes_password"].ToString();
        //        }
        //        return password;
        //    }
        //}


        //private string site = string.Empty;
        //public string Site
        //{
        //    get
        //    {
        //        if (string.IsNullOrEmpty(site))
        //        {
        //            site = ConfigurationManager.AppSettings["mes_site"].ToString();
        //        }
        //        return site;
        //    }
        //}

        #endregion

        #region 构造方法

        public MES() : this(-1) { }

        public MES(int id)
        {
            if (id < 0)
            {
                this.Id = -1;
                return;
            }

            string msg = string.Empty;

            DataTable dt = Database.Query("SELECT * FROM [dbo].[" + TableName + "] WHERE Id = " + id, out msg);

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
            this.name = rowInfo["Name"].ToString();
            this.host = rowInfo["Host"].ToString();
            this.isEnable = Convert.ToBoolean(rowInfo["IsEnable"]);
            this.isOffline = Convert.ToBoolean(rowInfo["IsOffline"]);
            this.webServiceUrl = rowInfo["WebServiceUrl"].ToString();
            this.username = rowInfo["Username"].ToString();
            this.password = rowInfo["Password"].ToString();
        }
        #endregion

        #region 系统MES列表
        private static List<MES> mesList = new List<MES>();
        private static List<MES> MesList
        {
            get
            {
                string msg = string.Empty;

                DataTable dt = Database.Query("SELECT * FROM [dbo].[" + TableName + "]", out msg);
                if (!string.IsNullOrEmpty(msg))
                {
                    Error.Alert(msg);
                    return null;
                }

                if (dt == null || dt.Rows.Count == 0)
                {
                    mesList = null;
                }
                else
                {
                    mesList.Clear();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        MES mes = new MES();
                        mes.InitFields(dt.Rows[i]);
                        mesList.Add(mes);
                    }
                }
                return mesList;
            }
        }

        #endregion

        #region 获取

        public static MES Mes
        {
            get
            {
                return MesList.First();
            }
        }

        public static MES GetMES(string name, out string msg)
        {
            try
            {
                List<MES> list = (from mes in MesList where mes.Name == name select mes).ToList();
                if (list.Count() > 0)
                {
                    msg = string.Empty;
                    return list[0];
                }
                msg = string.Format("数据库不存在 {0}！", name);
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }

            return null;
        }

        #endregion

        #region MES方法


        private static EquipService _wsProxy = null;
        private static EquipService wsProxy
        {
            get
            {
                if (_wsProxy == null)
                {
                    _wsProxy = new EquipService()
                    {
                        Url = Current.mes.WebServiceUrl
                    };
                    //{
                    //    Credentials = new NetworkCredential(Current.mes.Username, Current.mes.Password, null),
                    //    PreAuthenticate = true,
                    //    Timeout = 2000,
                    //    Url = Current.mes.WebServiceUrl
                    //};
                }
                return _wsProxy;
            }
        }

        /// <summary>
        /// 烘烤数据上传
        /// </summary>
        /// <param name="clamps"></param>
        /// <returns></returns>
        public static void UploadBatteryInfo()
        {

        }



        /// <summary>
        /// 设备状态上传
        /// </summary>
        /// <param name="clamps"></param>
        /// <returns></returns>
        public static void UploadMachineStatus()
        {

        }
        #endregion
    }
}
