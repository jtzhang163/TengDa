using Microsoft.CSharp;
using System;
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

namespace BakBattery.Baking
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
        /// MES上传公用方法
        /// </summary>
        /// <param name="uploadDatas"></param>
        /// <returns></returns>
        public static async Task<bool> UploadAsync(List<UploadData> uploadDatas)
        {
            if (uploadDatas.Count < 1)
            {
                return true;
            }
            string val = "";
            try
            {
                var values = new List<KeyValuePair<string, string>>();
                val = JsonHelper.SerializeObjectList<UploadData>(uploadDatas);
                values.Add(new KeyValuePair<string, string>("data", val));
                string s = await HttpHelper.HttpPostAsync(Current.mes.WebServiceUrl, values);
                MesResponse ret = JsonHelper.DeserializeJsonToObject<MesResponse>(s);
                if (ret.status == 0)
                {
                    LogHelper.WriteInfo("上传MES成功，data：" + val);
                    return true;
                }
                LogHelper.WriteError("上传MES失败，msg：" + ret.msg);
                LogHelper.WriteError("data：" + val);
                return false;
            }
            catch (Exception ex)
            {
                LogHelper.WriteError("上传MES出现错误，msg：" + ex.ToString());
                LogHelper.WriteError("val:" + val);
            }
            return false;
        }

        /// <summary>
        /// 上传真空温度数据
        /// </summary>
        /// <param name="uploadTVDs"></param>
        /// <returns></returns>
        public static async System.Threading.Tasks.Task UploadTvdInfoAsync(List<UploadTVD> uploadTVDs)
        {
            for (int i = 0; i < uploadTVDs.Count; i++)
            {
                try
                {
                    var uploadTVD = uploadTVDs[i];
                    var clamp = new Clamp(uploadTVD.ClampId);
                    var station = Station.StationList.FirstOrDefault(s => s.Id == clamp.OvenStationId);
                    var floor = station.GetFloor();
                    var oven = floor.GetOven();

                    bool isClampUploadSuccess = true;

                    foreach (var battery in clamp.Batteries)
                    {
                        var uploadDatas = new List<UploadData>();
                        var uploadData = new UploadData();

                        uploadData.batch_number = battery.Code.Split('-')[0];
                        uploadData.parameter_flag = uploadTVD.ParameterFlag.ToString();
                        uploadData.device_status = uploadTVD.DeviceStatus.ToString();
                        uploadData.collector_time = uploadTVD.CollectorTime.ToString("yyyy-MM-dd HH:mm:ss");

                        uploadData.materialCodeInfo = new List<MaterialCodeInfo>();
                        uploadData.materialCodeInfo.Add(new MaterialCodeInfo()
                        {
                            materialCodeName = "电芯编号",
                            materialCode = battery.Code
                        });

                        uploadData.deviceCodeInfo = new List<DeviceCodeInfo>();
                        uploadData.deviceCodeInfo.Add(new DeviceCodeInfo()
                        {
                            deviceCodeName = "机台号",
                            deviceCode = oven.Number
                        });
                        uploadData.deviceCodeInfo.Add(new DeviceCodeInfo()
                        {
                            deviceCodeName = "烤箱编号",
                            deviceCode = station.Number
                        });
                        uploadData.deviceCodeInfo.Add(new DeviceCodeInfo()
                        {
                            deviceCodeName = "料框编号",
                            deviceCode = clamp.Code
                        });

                        uploadData.deviceParamData = new List<DeviceParamData>();
                        for (int k = 0; k < Option.TemperaturePointCount; k++)
                        {
                            uploadData.deviceParamData.Add(new DeviceParamData
                            {
                                parameter_name = Current.option.TemperNames[k] + "实际值",
                                parameter_unit = "℃",
                                parameter_value = uploadTVD.T[k].ToString()
                            });
                        }

                        uploadData.deviceParamData.Add(new DeviceParamData
                        {
                            parameter_name = "真空度实际值",
                            parameter_unit = "Pa",
                            parameter_value = uploadTVD.V1.ToString()
                        });

                        uploadDatas.Add(uploadData);
                        if (!await UploadAsync(uploadDatas))
                        {
                            isClampUploadSuccess = false;
                        }
                    }

                    if (isClampUploadSuccess)
                    {
                        uploadTVD.IsUploaded = true;
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.WriteError("上传MES出现错误，方法：UploadTvdInfoAsync 原因：" + ex.ToString());
                }
            }
        }

        /// <summary>
        /// 入炉数据上传
        /// </summary>
        /// <param name="clamps"></param>
        /// <returns></returns>
        public static async System.Threading.Tasks.Task UploadInOvenAsync(List<Clamp> clamps)
        {
            for (int i = 0; i < clamps.Count; i++)
            {
                try
                {
                    var clamp = clamps[i];
                    var station = Station.StationList.FirstOrDefault(s => s.Id == clamp.OvenStationId);
                    var floor = station.GetFloor();
                    var oven = floor.GetOven();

                    bool isClampUploadSuccess = true;

                    foreach (var battery in clamp.Batteries)
                    {
                        var uploadDatas = new List<UploadData>();

                        var uploadData = new UploadData();

                        uploadData.batch_number = battery.Code.Split('-')[0];
                        uploadData.parameter_flag = "0";
                        uploadData.device_status = "0";
                        uploadData.collector_time = clamp.InOvenTime.ToString("yyyy-MM-dd HH:mm:ss");

                        uploadData.materialCodeInfo = new List<MaterialCodeInfo>();
                        uploadData.materialCodeInfo.Add(new MaterialCodeInfo()
                        {
                            materialCodeName = "电芯编号",
                            materialCode = battery.Code
                        });

                        uploadData.deviceCodeInfo = new List<DeviceCodeInfo>();
                        uploadData.deviceCodeInfo.Add(new DeviceCodeInfo()
                        {
                            deviceCodeName = "机台号",
                            deviceCode = oven.Number
                        });
                        uploadData.deviceCodeInfo.Add(new DeviceCodeInfo()
                        {
                            deviceCodeName = "烤箱编号",
                            deviceCode = station.Number
                        });
                        uploadData.deviceCodeInfo.Add(new DeviceCodeInfo()
                        {
                            deviceCodeName = "料框编号",
                            deviceCode = clamp.Code
                        });

                        uploadData.deviceParamData = new List<DeviceParamData>();

                        uploadData.deviceParamData.Add(new DeviceParamData
                        {
                            parameter_name = "工艺温度",
                            parameter_unit = "℃",
                            parameter_value = clamp.ProcessTemperSet.ToString()
                        });

                        for (int k = 0; k < Option.TemperatureSetPointCount; k++)
                        {
                            uploadData.deviceParamData.Add(new DeviceParamData
                            {
                                parameter_name = Current.option.TemperSetNames[k],
                                parameter_unit = "℃",
                                parameter_value = clamp.TsSet[k].ToString()
                            });
                        }

                        uploadData.deviceParamData.Add(new DeviceParamData
                        {
                            parameter_name = "真空度设定值",
                            parameter_unit = "Pa",
                            parameter_value = clamp.VacuumSet.ToString()
                        });

                        uploadData.deviceParamData.Add(new DeviceParamData
                        {
                            parameter_name = "热风循环温度",
                            parameter_unit = "℃",
                            parameter_value = clamp.YunFengTSet.ToString()
                        });

                        uploadData.deviceParamData.Add(new DeviceParamData
                        {
                            parameter_name = "进烤箱时间",
                            parameter_unit = "",
                            parameter_value = clamp.InOvenTime.ToString("yyyy-MM-dd HH:mm:ss")
                        });

                        uploadData.deviceParamData.Add(new DeviceParamData
                        {
                            parameter_name = "预热时间设置",
                            parameter_unit = "min",
                            parameter_value = clamp.PreheatTimeSet.ToString()
                        });

                        uploadData.deviceParamData.Add(new DeviceParamData
                        {
                            parameter_name = "烘烤时间设置",
                            parameter_unit = "min",
                            parameter_value = clamp.BakingTimeSet.ToString()
                        });

                        uploadData.deviceParamData.Add(new DeviceParamData
                        {
                            parameter_name = "一段呼吸周期设置",
                            parameter_unit = "min",
                            parameter_value = clamp.BreathingCycleSet1.ToString()
                        });

                        uploadData.deviceParamData.Add(new DeviceParamData
                        {
                            parameter_name = "二段呼吸周期设置",
                            parameter_unit = "min",
                            parameter_value = clamp.BreathingCycleSet2.ToString()
                        });

                        uploadDatas.Add(uploadData);
                        if (!await UploadAsync(uploadDatas))
                        {
                            isClampUploadSuccess = false;
                            break;
                        }
                    }

                    if (isClampUploadSuccess)
                    {
                        clamp.IsInUploaded = true;
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.WriteError("上传MES出现错误，方法：UploadInOvenAsync 原因：" + ex.ToString());
                }
            }
        }

        /// <summary>
        /// 出炉数据上传
        /// </summary>
        /// <param name="clamps"></param>
        /// <returns></returns>
        public static async System.Threading.Tasks.Task UploadOutOvenAsync(List<Clamp> clamps)
        {
            for (int i = 0; i < clamps.Count; i++)
            {
                try
                {
                    var clamp = clamps[i];
                    var station = Station.StationList.FirstOrDefault(s => s.Id == clamp.OvenStationId);
                    var floor = station.GetFloor();
                    var oven = floor.GetOven();

                    bool isClampUploadSuccess = true;

                    foreach (var battery in clamp.Batteries)
                    {
                        var uploadDatas = new List<UploadData>();

                        var uploadData = new UploadData();

                        uploadData.batch_number = battery.Code.Split('-')[0];
                        uploadData.parameter_flag = "0";
                        uploadData.device_status = "0";
                        uploadData.collector_time = clamp.OutOvenTime.ToString("yyyy-MM-dd HH:mm:ss");

                        uploadData.materialCodeInfo = new List<MaterialCodeInfo>();
                        uploadData.materialCodeInfo.Add(new MaterialCodeInfo()
                        {
                            materialCodeName = "电芯编号",
                            materialCode = battery.Code
                        });

                        uploadData.deviceCodeInfo = new List<DeviceCodeInfo>();
                        uploadData.deviceCodeInfo.Add(new DeviceCodeInfo()
                        {
                            deviceCodeName = "机台号",
                            deviceCode = oven.Number
                        });
                        uploadData.deviceCodeInfo.Add(new DeviceCodeInfo()
                        {
                            deviceCodeName = "烤箱编号",
                            deviceCode = station.Number
                        });
                        uploadData.deviceCodeInfo.Add(new DeviceCodeInfo()
                        {
                            deviceCodeName = "料框编号",
                            deviceCode = clamp.Code
                        });

                        uploadData.deviceParamData = new List<DeviceParamData>();
                        uploadData.deviceParamData.Add(new DeviceParamData
                        {
                            parameter_name = "出烤箱时间",
                            parameter_unit = "",
                            parameter_value = clamp.OutOvenTime.ToString("yyyy-MM-dd HH:mm:ss")
                        });

                        uploadDatas.Add(uploadData);
                        if(!await UploadAsync(uploadDatas))
                        {
                            isClampUploadSuccess = false;
                            break;
                        }
                    }

                    if (isClampUploadSuccess)
                    {
                        clamp.IsOutUploaded = true;
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.WriteError("上传MES出现错误，方法：UploadOutOvenAsync 原因：" + ex.ToString());
                }
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
}
