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
using TengDa;
using TengDa.WF;

namespace BakBattery.Baking
{
    //上传MES数据类
    public class UploadData : Service
    {

        #region 字段属性

        private static string tableName = string.Empty;
        public static string TableName
        {
            get
            {
                if (string.IsNullOrEmpty(tableName))
                {
                    tableName = Config.DbTableNamePre + ".UploadData";
                }
                return tableName;
            }
        }

        protected string parameterName = string.Empty;
        /// <summary>
        /// 参数名称
        /// </summary>
        [DisplayName("参数名称")]
        public string ParameterName
        {
            get
            {
                return parameterName;
            }
            set
            {
                if (parameterName != value)
                {
                    UpdateDbField("ParameterName", value);
                }
                parameterName = value;
            }
        }


        protected string parameterUnit = string.Empty;
        /// <summary>
        /// 参数单位
        /// </summary>
        [DisplayName("参数单位")]
        public string ParameterUnit
        {
            get
            {
                return parameterUnit;
            }
            set
            {
                if (parameterUnit != value)
                {
                    UpdateDbField("ParameterUnit", value);
                }
                parameterUnit = value;
            }
        }


        protected string parameterValue = string.Empty;
        /// <summary>
        /// 参数值
        /// </summary>
        [DisplayName("参数值")]
        public string ParameterValue
        {
            get
            {
                return parameterValue;
            }
            set
            {
                if (parameterValue != value)
                {
                    UpdateDbField("ParameterValue", value);
                }
                parameterValue = value;
            }
        }


        protected int parameterFlag = -1;
        /// <summary>
        /// 参数标志
        /// 0：表示为最新数据  
        /// 1：表示历史数据(包含连续几次发送失败之后的数据)
        /// </summary>
        [DisplayName("参数标志")]
        public int ParameterFlag
        {
            get
            {
                return parameterFlag;
            }
            set
            {
                if (parameterFlag != value)
                {
                    UpdateDbField("ParameterFlag", value);
                }
                parameterFlag = value;
            }
        }

        protected int deviceStatus = -1;
        /// <summary>
        /// 设备状态
        /// 运行：0 
        /// 关机：1
        /// 闲置：2  
        /// 故障：3  
        /// </summary>
        [DisplayName("设备状态")]
        public int DeviceStatus
        {
            get
            {
                return deviceStatus;
            }
            set
            {
                if (deviceStatus != value)
                {
                    UpdateDbField("DeviceStatus", value);
                }
                deviceStatus = value;
            }
        }

        protected DateTime collectorTime = Common.DefaultTime;
        /// <summary>
        /// 采集时间
        /// </summary>
        [DisplayName("采集时间")]
        public DateTime CollectorTime
        {
            get
            {
                return collectorTime;
            }
            set
            {
                if (collectorTime != value)
                {
                    UpdateDbField("CollectorTime", value);
                }
                collectorTime = value;
            }
        }

        protected bool isUploaded = false;
        /// <summary>
        /// 是否已上传MES
        /// </summary>
        [DisplayName("已上传")]
        public bool IsUploaded
        {
            get
            {
                return isUploaded;
            }
            set
            {
                if (isUploaded != value)
                {
                    UpdateDbField("IsUploaded", value);
                }
                isUploaded = value;
            }
        }

        protected bool isEnabled = false;
        /// <summary>
        /// 是否启用
        /// </summary>
        [DisplayName("是否启用")]
        public bool IsEnabled
        {
            get
            {
                return isEnabled;
            }
            set
            {
                if (isEnabled != value)
                {
                    UpdateDbField("IsEnabled", value);
                }
                isEnabled = value;
            }
        }


        public int UserId { get; set; } = -1;

        #endregion

        #region 构造方法

        public UploadData() : this(-1) { }

        public UploadData(int id)
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
            this.parameterName = rowInfo["ParameterName"].ToString();
            this.parameterUnit = rowInfo["ParameterUnit"].ToString();
            this.parameterValue = rowInfo["ParameterValue"].ToString();
            this.parameterFlag = TengDa._Convert.StrToInt(rowInfo["ParameterFlag"].ToString(), -1);
            this.deviceStatus = TengDa._Convert.StrToInt(rowInfo["DeviceStatus"].ToString(), -1);
            this.collectorTime = TengDa._Convert.StrToDateTime(rowInfo["CollectorTime"].ToString(), Common.DefaultTime);
            this.isEnabled = Convert.ToBoolean(rowInfo["IsEnabled"]);
            this.isUploaded = Convert.ToBoolean(rowInfo["IsUploaded"]);
            this.UserId = TengDa._Convert.StrToInt(rowInfo["UserId"].ToString(), -1);
        }
        #endregion
       
        #region 方法


        #endregion
    }
}
