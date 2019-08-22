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
        /// MES用户名
        /// </summary>
        [DisplayName("MES用户名")]
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
        /// MES密码
        /// </summary>
        [DisplayName("MES密码")]
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

        protected string terminal = string.Empty;
        /// <summary>
        /// 工位
        /// </summary>
        [DisplayName("工位")]
        [Category("基本设置")]
        public string Terminal
        {
            get
            {
                return terminal;
            }
            set
            {
                if (terminal != value)
                {
                    UpdateDbField("Terminal", value);
                }
                terminal = value;
            }
        }


        protected string userId = string.Empty;
        /// <summary>
        /// 员工工号
        /// </summary>
        [DisplayName("员工工号")]
        [Category("基本设置")]
        public string UserId
        {
            get
            {
                return userId;
            }
            set
            {
                if (userId != value)
                {
                    UpdateDbField("UserId", value);
                }
                userId = value;
            }
        }

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
            this.terminal = rowInfo["Terminal"].ToString();
            this.userId = rowInfo["UserId"].ToString();
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


        private static AutoLineService _wsProxy = null;
        private static AutoLineService wsProxy
        {
            get
            {
                if (_wsProxy == null)
                {
                    _wsProxy = new AutoLineService()
                    {
                        Url = Current.mes.WebServiceUrl,
                        Timeout = 2000,
                    };


                    MySoapHelper header = new MySoapHelper();

                    header.userName = Current.mes.Username;
                    header.passWord = Current.mes.Password;

                    _wsProxy.MySoapHelperValue = header;

                }
                return _wsProxy;
            }
        }

        /// <summary>
        /// 电池数据上传
        /// </summary>
        public static MesResponse UploadBatteryInfo(MesRequest request)
        {
            MesResponse response = new MesResponse();
            try
            {
                var jsonResponse = wsProxy.PassStationCheck(request.Barcode, request.Flag, request.Terminal, request.UserId);
                response = JsonHelper.DeserializeJsonToObject<MesResponse>(jsonResponse);
            }
            catch (Exception ex)
            {
                response.Code = -1;
                response.RtMsg = ex.Message;
                LogHelper.WriteError(ex);
            }
            return response;
        }

        public static int CheckBattery(string code)
        {
            if (!Current.mes.IsEnable)
            {
                return 0;
            }
            var request = new MesRequest()
            {
                Barcode = code,
                Flag = "1",
                Terminal = Current.mes.Terminal,
                UserId = Current.mes.UserId
            };
            var response = UploadBatteryInfo(request);

            LogHelper.WriteInfo(string.Format("检验MES结果，条码：{0}, 结果：{1}", code, response.Code));
            return response.Code;
        }

        #endregion
    }

    public class MesRequest
    {
        /// <summary>
        /// 电芯条码
        /// </summary>
        public string Barcode { get; set; }

        /// <summary>
        /// 调用类型
        /// </summary>
        public string Flag { get; set; }

        /// <summary>
        /// 调用的工位
        /// </summary>
        public string Terminal { get; set; }

        /// <summary>
        /// 调用者（员工工号）
        /// </summary>
        public string UserId { get; set; }
    }

    public class MesResponse
    {
        /// <summary>
        /// 返回代码
        /// 0:成功 1: 验证失败 2：值不存在 3：发生错误
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// 返回信息
        /// 成功返回为检测的结果，失败返回对应的提示信息
        /// </summary>
        public string RtMsg { get; set; }
    }
}
