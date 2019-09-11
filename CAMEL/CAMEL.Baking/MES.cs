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
using System.Xml.Serialization;

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
        [ReadOnly(true), DisplayName("用户名称")]
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
        [ReadOnly(true), DisplayName("用户密码")]
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


        [ReadOnly(true), DisplayName("设备编号")]
        public string DeviceId { get; set; } = string.Empty;
        [ReadOnly(true), DisplayName("工位代码")]
        public string StationCode { get; set; } = string.Empty;
        [ReadOnly(true), DisplayName("工序代码")]
        public string ProcessCode { get; set; } = string.Empty;

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
        public static IdentityVerificationService IdentityVerificationProxy
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
        public static TrayBindingService TrayBindingProxy
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
        public static DeviceStatusRecordService DeviceStatusRecordProxy
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
        public static ProductionDataUploadService ProductionDataUploadProxy
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

        /// <summary>
        /// 身份验证
        /// </summary>
        /// <param name="xmlParams"></param>
        /// <returns></returns>
        public static string IdentityVerification(string xmlParams)
        {
            try
            {
                if (!Current.mes.IsPingSuccess)
                {
                    throw new Exception("无法连接到MES服务器：" + Current.mes.Host);
                }
                return IdentityVerificationProxy.IdentityVerification(xmlParams);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }        
        }

        /// <summary>
        /// 身份验证
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static bool IdentityVerification(out string msg)
        {
            try
            {
                if (!Current.mes.IsPingSuccess)
                {
                    throw new Exception("无法连接到MES服务器：" + Current.mes.Host);
                }
                var request = new IdentityVerificationRequest()
                {
                    AccountNumber = Current.mes.Username,
                    Password = Current.mes.Password
                };
                msg = string.Empty;
                var xmlRequest = XmlHelper.Serialize(request).Replace("<?xml version=\"1.0\" encoding=\"utf-8\"?>", "");
                var xmlResponse = IdentityVerificationProxy.IdentityVerification(xmlRequest);
                var response = XmlHelper.Deserialize<IdentityVerificationResponse>(xmlResponse);
                if (response.Result)
                {
                    Current.mes.DeviceId = response.DeviceId;
                    Current.mes.StationCode = response.StationCode;
                    Current.mes.ProcessCode = response.ProcessCode;
                    //LogHelper.WriteInfo(string.Format("MES身份验证OK，xmlRequest：{1}，xmlResponse：{2}", "", xmlRequest, xmlResponse));
                    //LogHelper.WriteInfo(string.Format("MES身份验证OK"));
                }
                else
                {
                    msg = "MES身份验证不通过，原因:" + response.Message;
                }
                return response.Result;
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex);
                msg = "MES身份验证异常报错:" + ex.Message;
            }
            return false;
        }

        public static string GetTrayBindingInfo(string xmlParams)
        {
            try
            {
                if (!Current.mes.IsPingSuccess)
                {
                    throw new Exception("无法连接到MES服务器：" + Current.mes.Host);
                }
                return TrayBindingProxy.GetTrayBindingInfo(xmlParams);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public static bool GetTrayBindingInfo(Clamp clamp, out List<Battery> batteries)
        {
            batteries = new List<Battery>();
            try
            {
                if (!Current.mes.IsPingSuccess)
                {
                    throw new Exception("无法连接到MES服务器：" + Current.mes.Host);
                }

                var request = new TrayBindingRequest()
                {
                    TrayBarcode = clamp.Code,
                    DeviceId = Current.mes.DeviceId
                };

                var xmlRequest = XmlHelper.Serialize(request).Replace("<?xml version=\"1.0\" encoding=\"utf-8\"?>", "");
                var xmlResponse = TrayBindingProxy.GetTrayBindingInfo(xmlRequest);
                var response = XmlHelper.Deserialize<TrayBindingResponse>(xmlResponse);
                if (response.Result)
                {
                    for (int i = 0; i < response.BarcodeGroup.Length; i++)
                    {
                        if (!string.IsNullOrEmpty(response.BarcodeGroup[i].Barcode))
                        {
                            batteries.Add(new Battery(response.BarcodeGroup[i].Barcode, Current.Feeder.Id, clamp.Id, response.BarcodeGroup[i].Point.ToString()));
                        }
                    }
                    //LogHelper.WriteInfo(string.Format("从MES获取夹具绑定的电池成功，xmlRequest：{0}，xmlResponse：{1}", xmlRequest, xmlResponse));
                    //LogHelper.WriteInfo(string.Format("从MES获取夹具绑定的电池成功"));
                }
                else
                {
                    LogHelper.WriteInfo(string.Format("从MES获取夹具绑定的电池失败，原因：{0}", response.Message));
                }
                clamp.IsDownloaded = true;
                return response.Result;
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex);
                LogHelper.WriteInfo(string.Format("烤箱状态上传MES报错：{0}", ex.Message));
            }
            return false;
        }

        public static string RecordDeviceStatus(string xmlParams)
        {
            try
            {
                if (!Current.mes.IsPingSuccess)
                {
                    throw new Exception("无法连接到MES服务器：" + Current.mes.Host);
                }
                return DeviceStatusRecordProxy.RecordDeviceStatus(xmlParams);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public static bool RecordDeviceStatus(List<Oven> ovens)
        {
            try
            {
                if (!Current.mes.IsPingSuccess)
                {
                    throw new Exception("无法连接到MES服务器：" + Current.mes.Host);
                }

                List<DeviceStatusRecordRequest._DeviceStatus> deviceStatusList = new List<DeviceStatusRecordRequest._DeviceStatus>();
                ovens.ForEach(o =>
                {
                    o.Floors.ForEach(f =>
                    {
                        deviceStatusList.Add(f.GetMesDeviceStatus());
                    });
                });

                var request = new DeviceStatusRecordRequest()
                {
                    DeviceId = Current.mes.DeviceId,
                    Operator = Current.mes.Username,
                    ProcessCode = Current.mes.ProcessCode,
                    StationCode = Current.mes.StationCode,
                    ProductionTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    DeviceStatuses = deviceStatusList.ToArray()
                };

                var xmlRequest = XmlHelper.Serialize(request).Replace("<?xml version=\"1.0\" encoding=\"utf-8\"?>", "");
                var xmlResponse = DeviceStatusRecordProxy.RecordDeviceStatus(xmlRequest);
                var response = XmlHelper.Deserialize<DeviceStatusRecordResponse>(xmlResponse);
                if (response.Result)
                {
                    //LogHelper.WriteInfo(string.Format("烤箱状态上传MES成功，xmlRequest：{0}，xmlResponse：{1}", xmlRequest, xmlResponse));
                    //LogHelper.WriteInfo(string.Format("烤箱状态上传MES成功"));
                }
                else
                {
                    LogHelper.WriteInfo(string.Format("烤箱状态上传MES失败，原因：{0}", response.Message));
                }
                return response.Result;
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex);
                LogHelper.WriteInfo(string.Format("烤箱状态上传MES报错：{0}", ex.Message));
            }
            return false;
        }

        public static string UploadSecondaryHighTempData(string xmlParams)
        {
            try
            {
                if (!Current.mes.IsPingSuccess)
                {
                    throw new Exception("无法连接到MES服务器：" + Current.mes.Host);
                }
                return ProductionDataUploadProxy.UploadSecondaryHighTempData(xmlParams);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public static bool UploadSecondaryHighTempData(Clamp clamp)
        {
            try
            {
                if (!Current.mes.IsPingSuccess)
                {
                    throw new Exception("无法连接到MES服务器：" + Current.mes.Host);
                }

                var batteries = clamp.Batteries;
                if(batteries.Count == 0)
                {
                    LogHelper.WriteInfo(string.Format("二次高温电池数据上传MES时，检测到{0}无绑定的电池数据，ID：{1}", clamp.Code, clamp.Id));
                    return true;
                }

                List<ProductionDataUploadRequest.TestData> testDataList = new List<ProductionDataUploadRequest.TestData>();
                batteries.ForEach(o =>
                {
                    testDataList.Add(new ProductionDataUploadRequest.TestData()
                    {
                        MoCode = clamp.MoCode,
                        TrayBarcode = clamp.Code,
                        Barcode = o.Code,
                        Position = o.Location,
                        Result = clamp.BakingResult,
                        BakingTime = clamp.BakingTime.ToString(),
                        BakingTemperature = clamp.Temperature.ToString("#0.00")
                    });
                });

                var request = new ProductionDataUploadRequest()
                {
                    DeviceId = Current.mes.DeviceId,
                    Operator = Current.mes.Username,
                    ProcessCode = Current.mes.ProcessCode,
                    StationCode = Current.mes.StationCode,
                    ProductionTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    TestDatas = testDataList.ToArray(),
                    Shift = clamp.Shift
                };

                var xmlRequest = XmlHelper.Serialize(request).Replace("<?xml version=\"1.0\" encoding=\"utf-8\"?>", "");
                var xmlResponse = ProductionDataUploadProxy.UploadSecondaryHighTempData(xmlRequest);
                var response = XmlHelper.Deserialize<ProductionDataUploadResponse>(xmlResponse);
                if (response.Result)
                {
                    //LogHelper.WriteInfo(string.Format("二次高温电池数据上传MES成功，xmlRequest：{0}，xmlResponse：{1}", xmlRequest, xmlResponse));
                    //LogHelper.WriteInfo(string.Format("二次高温电池数据上传MES成功"));
                }
                else
                {
                    LogHelper.WriteInfo(string.Format("二次高温电池数据上传MES失败，原因：{0} xmlRequest：{1}，xmlResponse：{2}", response.Message, xmlRequest, xmlResponse));
                }
                clamp.IsUploaded = true;
                return response.Result;
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex);
                LogHelper.WriteInfo(string.Format("烤箱状态上传MES报错：{0}", ex.Message));
            }
            return false;
        }
        #endregion
    }


    [XmlRoot("Request")]
    public class IdentityVerificationRequest
    {
        public string AccountNumber { get; set; }

        public string Password { get; set; }
    }

    [XmlRoot("Response")]
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

    [XmlRoot("Request")]
    public class TrayBindingRequest
    {
        public string TrayBarcode { get; set; }

        public string DeviceId { get; set; }
    }

    [XmlRoot("Response")]
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

    [XmlRoot("Request")]
    public class DeviceStatusRecordRequest
    {
        [XmlElement("")]
        public _DeviceStatus[] DeviceStatuses { get; set; }

        public string DeviceId { get; set; }

        public string StationCode { get; set; }

        public string ProcessCode { get; set; }

        public string ProductionTime { get; set; }

        public string Operator { get; set; }

        [XmlRoot("DeviceStatuses")]
        public class _DeviceStatus
        {
            public string DeviceStatus { get; set; }

            public string StatusDescription { get; set; }
        }
    }

    [XmlRoot("Response")]
    public class DeviceStatusRecordResponse
    {
        public string DeviceId { get; set; }
        public bool Result { get; set; }
        public string Message { get; set; }
        public int ExecutionTime { get; set; }
    }

    [XmlRoot("Request")]
    public class ProductionDataUploadRequest
    {
        [XmlElement("")]
        public TestData[] TestDatas { get; set; }

        public string DeviceId { get; set; }

        public string StationCode { get; set; }

        public string ProcessCode { get; set; }

        public string Shift { get; set; }

        public string ProductionTime { get; set; }

        public string Operator { get; set; }

        [XmlRoot("TestDatas")]
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

    [XmlRoot("Response")]
    public class ProductionDataUploadResponse
    {
        public string DeviceId { get; set; }
        public bool Result { get; set; }
        public string Message { get; set; }
        public int ExecutionTime { get; set; }
    }
}
