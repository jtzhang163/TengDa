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
using CAMEL.Baking.MesService;
using CAMEL.Baking.IdentityVerificationWebService;
using CAMEL.Baking.TrayBindingWebService;
using CAMEL.Baking.DeviceStatusRecordWebService;
using CAMEL.Baking.ProductionDataUploadWebService;

namespace CAMEL.Baking
{
    public class MES : TengDa.WF.MES.MES
    {

        #region 字段属性


        private string webServiceUrl_IdentityVerification = string.Empty;
        /// <summary>
        /// 身份验证接口地址
        /// </summary>
        [DisplayName("身份验证接口地址")]
        [Category("基本设置")]
        public string WebServiceUrl_IdentityVerification
        {
            get
            {
                return webServiceUrl_IdentityVerification;
            }
            set
            {
                if (webServiceUrl_IdentityVerification != value)
                {
                    UpdateDbField("WebServiceUrl_IdentityVerification", value);
                }
                webServiceUrl_IdentityVerification = value;
            }
        }


        private string webServiceUrl_TrayBinding = string.Empty;
        /// <summary>
        /// 电芯与托盘信息查询接口地址
        /// </summary>
        [DisplayName("电芯与托盘信息查询接口地址")]
        [Category("基本设置")]
        public string WebServiceUrl_TrayBinding
        {
            get
            {
                return webServiceUrl_TrayBinding;
            }
            set
            {
                if (webServiceUrl_TrayBinding != value)
                {
                    UpdateDbField("WebServiceUrl_TrayBinding", value);
                }
                webServiceUrl_TrayBinding = value;
            }
        }


        private string webServiceUrl_DeviceStatusRecord = string.Empty;
        /// <summary>
        /// 记录设备状态接口地址
        /// </summary>
        [DisplayName("记录设备状态接口地址")]
        [Category("基本设置")]
        public string WebServiceUrl_DeviceStatusRecord
        {
            get
            {
                return webServiceUrl_DeviceStatusRecord;
            }
            set
            {
                if (webServiceUrl_DeviceStatusRecord != value)
                {
                    UpdateDbField("WebServiceUrl_DeviceStatusRecord", value);
                }
                webServiceUrl_DeviceStatusRecord = value;
            }
        }

        private string webServiceUrl_ProductionDataUpload = string.Empty;
        /// <summary>
        /// 二次高温数据上传接口地址
        /// </summary>
        [DisplayName("二次高温数据上传接口地址")]
        [Category("基本设置")]
        public string WebServiceUrl_ProductionDataUpload
        {
            get
            {
                return webServiceUrl_ProductionDataUpload;
            }
            set
            {
                if (webServiceUrl_ProductionDataUpload != value)
                {
                    UpdateDbField("WebServiceUrl_ProductionDataUpload", value);
                }
                webServiceUrl_ProductionDataUpload = value;
            }
        }



        private string username = string.Empty;
        public string Username
        {
            get
            {
                if (string.IsNullOrEmpty(username))
                {
                    username = ConfigurationManager.AppSettings["mes_username"].ToString();
                }
                return username;
            }
        }

        private string password = string.Empty;
        public string Password
        {
            get
            {
                if (string.IsNullOrEmpty(password))
                {
                    password = ConfigurationManager.AppSettings["mes_password"].ToString();
                }
                return password;
            }
        }


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
            this.webServiceUrl_IdentityVerification = rowInfo["WebServiceUrl_IdentityVerification"].ToString();
            this.webServiceUrl_TrayBinding = rowInfo["WebServiceUrl_TrayBinding"].ToString();
            this.webServiceUrl_DeviceStatusRecord = rowInfo["WebServiceUrl_DeviceStatusRecord"].ToString();
            this.webServiceUrl_ProductionDataUpload = rowInfo["WebServiceUrl_ProductionDataUpload"].ToString();
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


        private static IdentityVerificationService identityVerificationProxy = null;
        private static IdentityVerificationService IdentityVerificationProxy
        {
            get
            {
                if (identityVerificationProxy == null)
                {
                    identityVerificationProxy = new IdentityVerificationService()
                    {
                        Url = Current.mes.WebServiceUrl_IdentityVerification
                    };
                }
                return identityVerificationProxy;
            }
        }

        private static TrayBindingService trayBindingProxy = null;
        private static TrayBindingService TrayBindingProxy
        {
            get
            {
                if (trayBindingProxy == null)
                {
                    trayBindingProxy = new TrayBindingService()
                    {
                        Url = Current.mes.WebServiceUrl_TrayBinding
                    };
                }
                return trayBindingProxy;
            }
        }

        private static DeviceStatusRecordService deviceStatusRecordProxy = null;
        private static DeviceStatusRecordService DeviceStatusRecordProxy
        {
            get
            {
                if (deviceStatusRecordProxy == null)
                {
                    deviceStatusRecordProxy = new DeviceStatusRecordService()
                    {
                        Url = Current.mes.WebServiceUrl_DeviceStatusRecord
                    };
                }
                return deviceStatusRecordProxy;
            }
        }

        private static ProductionDataUploadService productionDataUploadProxy = null;
        private static ProductionDataUploadService ProductionDataUploadProxy
        {
            get
            {
                if (productionDataUploadProxy == null)
                {
                    productionDataUploadProxy = new ProductionDataUploadService()
                    {
                        Url = Current.mes.WebServiceUrl_ProductionDataUpload
                    };
                }
                return productionDataUploadProxy;
            }
        }


        public static string IdentityVerification(string xmlParams)
        {
            var retXmlString = "";

            IdentityVerificationProxy.IdentityVerification(xmlParams);

            return retXmlString;
        }

        #endregion
    }


    public class CommonRequest
    {
        public object Request { get; set; }
    }

    public class CommonResponse
    {
        public object Response { get; set; }
    }

    public class IdentityVerificationRequest
    {
        public string AccountNumber { get; set; }

        public string Password { get; set; }
    }

    public class IdentityVerificationResponse
    {
        public string AccountNumber { get; set; }

        public string ProcessCode { get; set; }

        public string ProcessName { get; set; }
        public string StationCode { get; set; }
        public string StationName { get; set; }
        public string DeviceId { get; set; }
        public bool Result { get; set; }
        public string Message { get; set; }

        public int ExecutionTime { get; set; }
    }


    public class TrayBindingRequest
    {
        public string TrayBarcode { get; set; }

        public string DeviceId { get; set; }
    }

    public class TrayBindingResponse
    {
        public BarcodeInfo[] BarcodeGroup { get; set; }

        public string DeviceId { get; set; }
        public bool Result { get; set; }
        public string Message { get; set; }

        public int ExecutionTime { get; set; }

        public class BarcodeInfo
        {
            public int Point { get; set; }
            public string Barcode { get; set; }
        }
    }


    public class DeviceStatusRecordRequest
    {
        public string DeviceStatus { get; set; }

        public string StatusDescription { get; set; }

        public string DeviceId { get; set; }

        public string StationCode { get; set; }

        public string ProcessCode { get; set; }

        public DateTime ProductionTime { get; set; }

        public string Operator { get; set; }
    }


    public class DeviceStatusRecordResponse
    {

        public string DeviceId { get; set; }
        public bool Result { get; set; }
        public string Message { get; set; }
        public int ExecutionTime { get; set; }
    }


    public class ProductionDataUploadRequest
    {
        public TestData[] TestDatas { get; set; }

        public string DeviceId { get; set; }

        public string StationCode { get; set; }

        public string ProcessCode { get; set; }

        public string Shift { get; set; }

        public DateTime ProductionTime { get; set; }

        public string Operator { get; set; }

        public class TestData
        {
            public string MoCode { get; set; }
            public string TrayBarcode { get; set; }
            public string Barcode { get; set; }
            public string Position { get; set; }
            public string Result { get; set; }
            public string BakingTime { get; set; }
            public string BakingTemperature { get; set; }
        }
    }


    public class ProductionDataUploadResponse
    {

        public string DeviceId { get; set; }
        public bool Result { get; set; }
        public string Message { get; set; }
        public int ExecutionTime { get; set; }
    }



















    public class BakingMesData
    {
        public string Barcode { get; set; }
        public string MachineCode { get; set; }
        public string TrayNo { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public float Temperature { get; set; }
        public float Vacuum { get; set; }
    }

    public class MachineStatusData
    {
        public string MachCode { get; set; }
        public string MachStatus { get; set; }
        public string StepProdLotNo { get; set; }
        public string MachTrouble { get; set; }
    }

}
