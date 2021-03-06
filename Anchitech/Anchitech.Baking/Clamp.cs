﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using TengDa;
using TengDa.WF;

namespace Anchitech.Baking
{
    /// <summary>
    /// 料盒
    /// </summary>
    public class Clamp : TengDa.WF.Products.Product
    {
        #region 属性字段
        private static string tableName = string.Empty;
        public static string TableName
        {
            get
            {
                if (string.IsNullOrEmpty(tableName))
                {
                    tableName = Config.DbTableNamePre + ".Clamps";
                }
                return tableName;
            }
        }

        public string CompleteCode
        {
            get
            {
                return "ACBT" + this.Code;
            }
        }

        private int ovenStationId = -1;
        /// <summary>
        /// 烘烤所在炉腔工位Id
        /// </summary>
        [ReadOnly(true), DisplayName("烘烤所在炉腔工位Id")]
        public int OvenStationId
        {
            get
            {
                return ovenStationId;
            }
            set
            {
                if (ovenStationId != value)
                {
                    UpdateDbField("OvenStationId", value);
                }
                ovenStationId = value;
            }
        }
        /// <summary>
        /// 用户Id
        /// </summary>
        [ReadOnly(true)]
        public int UserId { get; set; }


        /// <summary>
        /// Baking开始时间
        /// </summary>
        [ReadOnly(true), DisplayName("开始烘烤时间")]
        [Category("基本信息")]
        public DateTime BakingStartTime
        {
            get
            {
                return bakingStartTime;
            }
            set
            {
                if (bakingStartTime != value)
                {
                    UpdateDbField("BakingStartTime", value);
                }
                bakingStartTime = value;
            }
        }

        /// <summary>
        /// Baking结束时间
        /// </summary>
        [ReadOnly(true), DisplayName("结束烘烤时间")]
        [Category("基本信息")]
        public DateTime BakingStopTime
        {
            get
            {
                return bakingStopTime;
            }
            set
            {
                if (bakingStopTime != value)
                {
                    UpdateDbField("BakingStopTime", value);
                }
                bakingStopTime = value;
            }
        }

        /// <summary>
        /// 入烤箱时间
        /// </summary>
        [ReadOnly(true), DisplayName("入烤箱时间")]
        [Category("基本信息")]
        public DateTime InOvenTime
        {
            get
            {
                return inOvenTime;
            }
            set
            {
                if (inOvenTime != value)
                {
                    UpdateDbField("InOvenTime", value);
                }
                inOvenTime = value;
            }
        }

        /// <summary>
        /// 出烤箱时间
        /// </summary>
        [ReadOnly(true), DisplayName("出烤箱时间")]
        [Category("基本信息")]
        public DateTime OutOvenTime
        {
            get
            {
                return outOvenTime;
            }
            set
            {
                if (outOvenTime != value)
                {
                    UpdateDbField("OutOvenTime", value);
                }
                outOvenTime = value;
            }
        }

        private bool isUploaded;
        /// <summary>
        /// 是否已上传MES
        /// </summary>
        [DisplayName("是否已上传MES")]
        [Category("基本信息")]
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

        private bool isFinished;
        /// <summary>
        /// 是否已完成
        /// </summary>
        [DisplayName("是否已完成")]
        [Category("基本信息")]
        public bool IsFinished
        {
            get
            {
                return isFinished;
            }
            set
            {
                if (isFinished != value)
                {
                    UpdateDbField("IsFinished", value);
                }
                isFinished = value;
            }
        }


        private float temperature = -1;
        /// <summary>
        /// 温度值
        /// </summary>
        [ReadOnly(true), DisplayName("温度值")]
        [Category("基本信息")]
        public float Temperature
        {
            get
            {
                return temperature;
            }
            set
            {
                if (temperature != value)
                {
                    UpdateDbField("Temperature", value);
                }
                temperature = value;
            }
        }


        private float vacuum = -1;
        /// <summary>
        /// 真空值
        /// </summary>
        [ReadOnly(true), DisplayName("真空值")]
        [Category("基本信息")]
        public float Vacuum
        {
            get
            {
                return vacuum;
            }
            set
            {
                if (vacuum != value)
                {
                    UpdateDbField("Vacuum", value);
                }
                vacuum = value;
            }
        }

        private DateTime bakingStartTime = TengDa.Common.DefaultTime;
        private DateTime bakingStopTime = TengDa.Common.DefaultTime;
        private DateTime inOvenTime = TengDa.Common.DefaultTime;
        private DateTime outOvenTime = TengDa.Common.DefaultTime;


        private SampleInfo sampleInfo = SampleInfo.未知;
        /// <summary>
        /// 样品状态信息
        /// </summary>
        [DisplayName("样品状态信息")]
        public SampleInfo SampleInfo
        {
            get { return sampleInfo; }
            set
            {
                if (sampleInfo != value)
                {
                    UpdateDbField("SampleInfo", value);
                }
                sampleInfo = value;
            }
        }

        #endregion

        #region 构造方法
        public Clamp() : this(-1) { }

        public Clamp(int id)
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

        public Clamp(string code)
        {
            this.code = code;
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
            this.code = rowInfo["Code"].ToString().Trim();
            this.UserId = TengDa._Convert.StrToInt(rowInfo["UserId"].ToString(), -1);
            this.ovenStationId = TengDa._Convert.StrToInt(rowInfo["OvenStationId"].ToString(), -1);
            this.location = rowInfo["Location"].ToString().Trim();
            this.bakingStartTime = TengDa._Convert.StrToDateTime(rowInfo["BakingStartTime"].ToString(), Common.DefaultTime);
            this.bakingStopTime = TengDa._Convert.StrToDateTime(rowInfo["BakingStopTime"].ToString(), Common.DefaultTime);
            this.inOvenTime = TengDa._Convert.StrToDateTime(rowInfo["InOvenTime"].ToString(), Common.DefaultTime);
            this.outOvenTime = TengDa._Convert.StrToDateTime(rowInfo["OutOvenTime"].ToString(), Common.DefaultTime);
            this.isFinished = TengDa._Convert.StrToBool(rowInfo["IsFinished"].ToString(), false);
            this.isUploaded = TengDa._Convert.StrToBool(rowInfo["IsUploaded"].ToString(), true);
            this.scanTime = TengDa._Convert.StrToDateTime(rowInfo["ScanTime"].ToString(), Common.DefaultTime);
            this.temperature = TengDa._Convert.StrToFloat(rowInfo["Temperature"].ToString(), -1);
            this.vacuum = TengDa._Convert.StrToFloat(rowInfo["Vacuum"].ToString(), -1);
            this.sampleInfo = (SampleInfo)Enum.Parse(typeof(SampleInfo), rowInfo["SampleInfo"].ToString());
            this.Id = TengDa._Convert.StrToInt(rowInfo["Id"].ToString(), -1);
        }
        #endregion

        #region 该料盒下的电池列表
        private List<Battery> batteries = new List<Battery>();

        [Browsable(false)]
        public List<Battery> Batteries
        {
            get
            {
                if (this.Id > 0)
                {
                    string msg = string.Empty;
                    batteries = Battery.GetList(string.Format("SELECT * FROM [dbo].[{0}] WHERE ClampId = {1}", Battery.TableName, this.Id), out msg);
                }
                return batteries;
            }
        }
        #endregion

        #region 增删查改

        public static List<Clamp> GetList(string sql, out string msg)
        {
            List<Clamp> list = new List<Clamp>();
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
                    Clamp clamp = new Clamp();
                    clamp.InitFields(dt.Rows[i]);
                    list.Add(clamp);
                }
            }
            return list;
        }

        public static int Add(string code, out string msg)
        {
            if (string.IsNullOrEmpty(code))
            {
                code = "6666";
            }
            var clamp = new Clamp(code);
            clamp.scanTime = DateTime.Now;
            clamp.sampleInfo = SampleInfo.无样品;
            return Add(clamp, out msg);
        }

        public static int Add(Clamp addClamp, out string msg)
        {
            return Database.Insert(string.Format("INSERT INTO [dbo].[{0}] ([Code], [UserId], [OvenStationId], [Location], [BakingStartTime], [BakingStopTime], [ScanTime], [InOvenTime], [OutOvenTime], [IsFinished], [IsUploaded], [Temperature], [Vacuum], [SampleInfo]) VALUES ('{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}', '{12}', '{13}', '{14}')", TableName,
              addClamp.Code, TengDa.WF.Current.user.Id, addClamp.OvenStationId, addClamp.Location, addClamp.BakingStartTime, addClamp.BakingStopTime, addClamp.ScanTime, addClamp.InOvenTime, addClamp.OutOvenTime, addClamp.IsFinished, addClamp.IsUploaded, addClamp.Temperature, addClamp.Vacuum, addClamp.SampleInfo), out msg);
        }

        public static bool Delete(Clamp delClamp, out string msg)
        {
            return Database.NonQuery(string.Format("DELETE FROM [dbo].[{0}] WHERE [Code] = '{1}'", TableName, delClamp.Code), out msg);
        }

        public static bool Update(Clamp newClamp, out string msg)
        {
            msg = "";
            return true;
            //return Database.NonQuery(string.Format("UPDATE [dbo].[{0}] SET [EnterTime] = '{1}', [OutTime] = '{2}', [BakingStartTime] = '{3}', [BakingStopTime] = '{4}', [Vacuum] = {5}, [Temperature] = {6}, [IsUploaded] = '{7}', [IsFinished] = '{8}' WHERE [Id] = {9}", TableName, newClamp.EnterTime, newClamp.OutTime, newClamp.BakingStartTime, newClamp.BakingStopTime, newClamp.Vacuum, newClamp.Temperature, newClamp.IsUploaded, newClamp.IsFinished, newClamp.Id), out msg);
        }

        public static bool SetWaterCont(Floor floor, float waterCont)
        {
            string msg = "";

            for (int i = 0; i < floor.Stations.Count; i++)
            {
                string sql = string.Format("UPDATE dbo.[{0}] SET WaterContent = {1} WHERE Id in (SELECT TOP 1 Id FROM dbo.[{0}] WHERE OvenStationId = {2} AND BakingStopTime > '2010-01-01' ORDER BY BakingStopTime DESC)", TableName, waterCont, floor.Stations[i].Id);
                if (!Database.NonQuery(sql, out msg))
                {
                    Error.Alert(msg);
                    return false;
                }
            }
            return true;
        }
        #endregion

        public static int GetCount(out string msg)
        {
            DataTable dt = Database.Query(string.Format("SELECT COUNT(*) FROM [dbo].[{0}];", TableName), out msg);
            if (dt.Rows.Count > 0)
            {
                return TengDa._Convert.StrToInt(dt.Rows[0][0].ToString(), -1);
            }
            return 0;
        }

        /// <summary>
        /// 保留最近的10000条数据
        /// </summary>
        public static bool DeleteLongAgo(out string msg)
        {
            return Database.NonQuery(string.Format("DELETE FROM dbo.[{0}] WHERE Id <= ((SELECT MAX(Id) from dbo.[{0}]) - 10000)", TableName), 60, out msg);
        }
    }

    /// <summary>
    /// 样品状态信息
    /// </summary>
    public enum SampleInfo
    {
        未知,
        无样品,
        有样品
    }

    public enum ClampStatus
    {
        未知,
        满夹具,
        空夹具,
        无夹具,
        异常
    }

    public enum ClampOri
    {
        未知,
        A,
        B
    }
}
