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
using Soundon.Dispatcher.MesWebService;
using Soundon.Dispatcher.ExecutingWebReference;

namespace Soundon.Dispatcher
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


        private string site = string.Empty;
        public string Site
        {
            get
            {
                if (string.IsNullOrEmpty(site))
                {
                    site = ConfigurationManager.AppSettings["mes_site"].ToString();
                }
                return site;
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

        /// <summary>
        /// 上传真空温度数据
        /// </summary>
        /// <param name="uploadTVDs"></param>
        /// <returns></returns>
        public static void UploadTvdInfo(List<UploadTVD> uploadTVDs)
        {

        }

        private static ExecutingServiceService _wsProxy = null;
        private static ExecutingServiceService wsProxy
        {
            get
            {
                if (_wsProxy == null)
                {
                    _wsProxy = new ExecutingServiceService
                    {
                        Credentials = new NetworkCredential(Current.mes.Username, Current.mes.Password, null),
                        PreAuthenticate = true,
                        Timeout = 2000,
                        Url = Current.mes.WebServiceUrl
                    };
                }
                return _wsProxy;
            }
        }

        /// <summary>
        /// 入炉数据上传
        /// </summary>
        /// <param name="clamps"></param>
        /// <returns></returns>
        public static void UploadInOven(List<Clamp> clamps)
        {

            for (int i = 0; i < clamps.Count; i++)
            {
                var clamp = clamps[i];
                var station = Station.StationList.FirstOrDefault(s => s.Id == clamp.OvenStationId);
                var floor = station.GetFloor();
                var oven = floor.GetOven();

                var sfcDatas = new List<SfcData>();
                foreach (var battery in clamp.Batteries)
                {
                    sfcDatas.Add(new SfcData() { SFC = battery.Code });
                }

                var response = new executeResponse();
                try
                {
                    var request = new executingServiceRequest()
                    {
                        site = Current.mes.Site,
                        serviceCode = "GetSfcListAndContainerIdOfBakeService",
                        data = JsonHelper.SerializeObject(new ExecuteData()
                        {
                            RESOURCE = oven.Number,
                            ACTION = "S",
                            CONTAINER_ID = clamp.Code,
                            IS_PROCESS_LOT = "N",
                            SFC_LIST = sfcDatas.ToArray(),
                            SFC = ""
                        })
                    };
                    response = wsProxy.execute(new execute() { pRequest = request });
                }
                catch (System.Exception ex)
                {
                    response.@return.status = "error";
                    response.@return.message = ex.Message;
                    LogHelper.WriteError(ex);
                }

                if (response.@return.status.ToLower().Contains("true"))
                {
                    clamp.IsInUploaded = true;
                }
            }
        }

        /// <summary>
        /// 出炉数据上传
        /// </summary>
        /// <param name="clamps"></param>
        /// <returns></returns>
        public static void UploadOutOven(List<Clamp> clamps)
        {

            for (int i = 0; i < clamps.Count; i++)
            {
                var clamp = clamps[i];
                var station = Station.StationList.FirstOrDefault(s => s.Id == clamp.OvenStationId);
                var floor = station.GetFloor();
                var oven = floor.GetOven();

                var sfcDatas = new List<SfcData>();
                foreach (var battery in clamp.Batteries)
                {
                    sfcDatas.Add(new SfcData() { SFC = battery.Code });
                }

                var response = new executeResponse();

                try
                {
                    var request = new executingServiceRequest()
                    {
                        site = Current.mes.Site,
                        serviceCode = "UntieContainerIdOfBakeService",
                        data = JsonHelper.SerializeObject(new ExecuteData()
                        {
                            RESOURCE = oven.Number,
                            CONTAINER_ID = clamp.Code,
                            NC_SFC_LIST = sfcDatas.ToArray()
                        })
                    };
                    response = wsProxy.execute(new execute() { pRequest = request });
                }
                catch (System.Exception ex)
                {
                    response.@return.status = "error";
                    response.@return.message = ex.Message;
                    LogHelper.WriteError(ex);
                }

                if (response.@return.status.ToLower().Contains("true"))
                {
                    clamp.IsOutUploaded = true;
                }
            }
        }



        /// <summary>
        /// 烘烤NG数据上传
        /// </summary>
        /// <param name="clamps"></param>
        /// <returns></returns>
        public static void UploadNgData(List<Clamp> clamps)
        {

            for (int i = 0; i < clamps.Count; i++)
            {
                var clamp = clamps[i];
                var station = Station.StationList.FirstOrDefault(s => s.Id == clamp.OvenStationId);
                var floor = station.GetFloor();
                var oven = floor.GetOven();

                var sfcDatas = new List<SfcData>();
                foreach (var battery in clamp.Batteries)
                {
                    sfcDatas.Add(new SfcData() { SFC = battery.Code });
                }

                var response = new executeResponse();

                try
                {
                    var request = new executingServiceRequest()
                    {
                        site = Current.mes.Site,
                        serviceCode = "BakeReworkUnbundlingService",
                        data = JsonHelper.SerializeObject(new ExecuteData()
                        {
                            RESOURCE = oven.Number,
                            CONTAINER_ID = clamp.Code,
                            NC_SFC_LIST = sfcDatas.ToArray()
                        })
                    };
                    response = wsProxy.execute(new execute() { pRequest = request });
                }
                catch (System.Exception ex)
                {
                    response.@return.status = "error";
                    response.@return.message = ex.Message;
                    LogHelper.WriteError(ex);
                }

                //if (response.@return.status.ToLower().Contains("true"))
                //{
                //    clamp.IsOutUploaded = true;
                //}
            }
        }


        /// <summary>
        /// 设备状态上传
        /// </summary>
        /// <param name="clamps"></param>
        /// <returns></returns>
        public static void UploadMachineStatus()
        {
            var machStatuss = new List<MachStatus>();
            for (int i = 0; i < Current.ovens.Count; i++)
            {
                var oven = Current.ovens[i];
                machStatuss.Add(new MachStatus() { RESOURCE = oven.Number, STATUS = "1" });
            }

            var response = new executeResponse();
            try
            {
                var request = new executingServiceRequest()
                {
                    site = Current.mes.Site,
                    serviceCode = "ResourceStatusChangeService",
                    data = JsonHelper.SerializeObject(new ExecuteData()
                    {
                        RESOURCE_LIST = machStatuss.ToArray()
                    })
                };
                response = wsProxy.execute(new execute() { pRequest = request });
            }
            catch (System.Exception ex)
            {
                response.@return.status = "error";
                response.@return.message = ex.Message;
                LogHelper.WriteError(ex);
            }

        }

        #endregion
    }

    /// <summary>
    /// 调用MES后返回数据的对象类型
    /// </summary>
    public class MesResponse
    {
        /// <summary>
        /// 状态
        /// 0：成功
        /// </summary>
        public int status { get; set; }
        public string msg { get; set; }
        public string errorMsg { get; set; }
        public string map { get; set; }
        public string data { get; set; }
    }

    public class ExecuteData
    {
        public string RESOURCE { get; set; }
        public string ACTION { get; set; }
        public string CONTAINER_ID { get; set; }
        public string IS_PROCESS_LOT { get; set; }
        public SfcData[] SFC_LIST { get; set; }
        public SfcData[] NC_SFC_LIST { get; set; }
        public string SFC { get; set; }
        public MachStatus[] RESOURCE_LIST { get; set; }
    }

    public class SfcData
    {
        public string SFC { get; set; }
    }

    public class MachStatus
    {
        public string RESOURCE { get; set; }
        public string STATUS { get; set; }
    }
}
