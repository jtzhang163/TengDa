using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;
using TengDa;
using TengDa.WF;

namespace BYD.Scan
{
    //上传MES温度真空数据类
    public class UploadTVD : Service
    {

        #region 字段属性

        private static string tableName = string.Empty;
        public static string TableName
        {
            get
            {
                if (string.IsNullOrEmpty(tableName))
                {
                    tableName = Config.DbTableNamePre + ".UploadTVD";
                }
                return tableName;
            }
        }

        public int ClampId { get; set; } = -1;

        /// <summary>
        /// 真空
        /// </summary>
        public float V1 { get; set; }
        /// <summary>
        /// 温度数组
        /// </summary>
        public float[] T = new float[Option.TemperaturePointCount];

        /// <summary>
        /// 参数标志
        /// 0：表示为最新数据  
        /// 1：表示历史数据(包含连续几次发送失败之后的数据)
        /// </summary>
        [DisplayName("参数标志")]
        public int ParameterFlag { get; set; } = -1;

        /// <summary>
        /// 设备状态
        /// 运行：0 
        /// 关机：1
        /// 闲置：2  
        /// 故障：3  
        /// </summary>
        [DisplayName("设备状态")]
        public int DeviceStatus { get; set; } = -1;

        /// <summary>
        /// 采集时间
        /// </summary>
        [DisplayName("采集时间")]
        public DateTime CollectorTime { get; set; } = Common.DefaultTime;

        private bool isUploaded = false;
        /// <summary>
        /// 已上传
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

        #endregion

        #region 构造方法

        public UploadTVD() : this(-1) { }

        public UploadTVD(int id)
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
            this.ClampId = TengDa._Convert.StrToInt(rowInfo["ClampId"].ToString(), -1);
            this.T[0] = TengDa._Convert.StrToFloat(rowInfo["T01"].ToString(), -1);
            this.T[1] = TengDa._Convert.StrToFloat(rowInfo["T02"].ToString(), -1);
            this.T[2] = TengDa._Convert.StrToFloat(rowInfo["T03"].ToString(), -1);
            this.T[3] = TengDa._Convert.StrToFloat(rowInfo["T04"].ToString(), -1);
            this.T[4] = TengDa._Convert.StrToFloat(rowInfo["T05"].ToString(), -1);
            this.T[5] = TengDa._Convert.StrToFloat(rowInfo["T06"].ToString(), -1);
            this.T[6] = TengDa._Convert.StrToFloat(rowInfo["T07"].ToString(), -1);
            this.T[7] = TengDa._Convert.StrToFloat(rowInfo["T08"].ToString(), -1);
            this.V1 = TengDa._Convert.StrToFloat(rowInfo["V1"].ToString(), -1);
            this.ParameterFlag = TengDa._Convert.StrToInt(rowInfo["ParameterFlag"].ToString(), -1);
            this.DeviceStatus = TengDa._Convert.StrToInt(rowInfo["DeviceStatus"].ToString(), -1);
            this.CollectorTime = TengDa._Convert.StrToDateTime(rowInfo["CollectorTime"].ToString(), Common.DefaultTime);
            this.isUploaded = TengDa._Convert.StrToBool(rowInfo["IsUploaded"].ToString(), false);
        }
        #endregion

        #region 方法

        /// <summary>
        /// 多条数据一次插入
        /// </summary>
        /// <param name="datas"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static bool Add(List<UploadTVD> datas, out string msg)
        {
            if (datas == null || datas.Count < 1)
            {
                msg = string.Empty;
                return true;
            }

            StringBuilder sb = new StringBuilder();
            sb.Append(string.Format("INSERT INTO [dbo].[{0}] ([ClampId], [T01], [T02], [T03], [T04], [T05], [T06], [T07], [T08], [V1], [ParameterFlag], [DeviceStatus], [CollectorTime], [IsUploaded]) VALUES ", TableName));

            foreach (var data in datas)
            {
                sb.Append(string.Format("({0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11}, '{12}', '{13}'),",
                    data.ClampId,
                    data.T[0], data.T[1], data.T[2], data.T[3], data.T[4], data.T[5], data.T[6], data.T[7],
                    data.V1,
                    data.ParameterFlag,
                    data.DeviceStatus,
                    data.CollectorTime,
                    data.IsUploaded));
            }

            return Database.NonQuery(sb.ToString().TrimEnd(','), out msg);
        }

        /// <summary>
        /// 根据SQL语句查询数据
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static List<UploadTVD> GetList(string sql, out string msg)
        {
            var list = new List<UploadTVD>();
            var dt = Database.Query(sql, out msg);

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
                    var data = new UploadTVD();
                    data.InitFields(dt.Rows[i]);
                    list.Add(data);
                }
            }
            return list;
        }

        #endregion
    }
}
