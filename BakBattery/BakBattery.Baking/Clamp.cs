using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using TengDa;
using TengDa.WF;

namespace BakBattery.Baking
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

        private bool isInUploaded;
        /// <summary>
        /// 入炉是否已上传MES
        /// </summary>
        [DisplayName("入炉是否已上传MES")]
        [Category("基本信息")]
        public bool IsInUploaded
        {
            get
            {
                return isInUploaded;
            }
            set
            {
                if (isInUploaded != value)
                {
                    UpdateDbField("IsInUploaded", value);
                }
                isInUploaded = value;
            }
        }

        private bool isOutUploaded;
        /// <summary>
        /// 出炉是否已上传MES
        /// </summary>
        [DisplayName("出炉是否已上传MES")]
        [Category("基本信息")]
        public bool IsOutUploaded
        {
            get
            {
                return isOutUploaded;
            }
            set
            {
                if (isOutUploaded != value)
                {
                    UpdateDbField("IsOutUploaded", value);
                }
                isOutUploaded = value;
            }
        }

        private bool isInFinished;
        /// <summary>
        /// 是否入炉已完成
        /// </summary>
        [DisplayName("是否入炉已完成")]
        [Category("基本信息")]
        public bool IsInFinished
        {
            get
            {
                return isInFinished;
            }
            set
            {
                if (isInFinished != value)
                {
                    UpdateDbField("IsInFinished", value);
                }
                isInFinished = value;
            }
        }

        private bool isOutFinished;
        /// <summary>
        /// 是否出炉已完成
        /// </summary>
        [DisplayName("是否出炉已完成")]
        [Category("基本信息")]
        public bool IsOutFinished
        {
            get
            {
                return isOutFinished;
            }
            set
            {
                if (isOutFinished != value)
                {
                    UpdateDbField("IsOutFinished", value);
                }
                isOutFinished = value;
            }
        }


        private float processTemperSet = -1;
        /// <summary>
        /// 工艺温度
        /// </summary>
        [ReadOnly(true), DisplayName("工艺温度")]
        [Category("基本信息")]
        public float ProcessTemperSet
        {
            get
            {
                return processTemperSet;
            }
            set
            {
                if (processTemperSet != value)
                {
                    UpdateDbField("ProcessTemperSet", value);
                }
                processTemperSet = value;
            }
        }


        private float[] tsSet = new float[Option.TemperatureSetPointCount];
        /// <summary>
        /// 温度设置数组值
        /// </summary>
        [ReadOnly(true), DisplayName("温度设置数组值")]
        [Category("基本信息")]
        public float[] TsSet
        {
            get
            {
                return tsSet;
            }
            set
            {
                if (tsSet != value)
                {
                    for (int i = 0; i < Option.TemperatureSetPointCount; i++)
                    {
                        UpdateDbField(string.Format("T{0:D2}Set", i + 1), value[i]);
                    }
                }
                tsSet = value;
            }
        }



        private float vacuumSet = -1;
        /// <summary>
        /// 真空设置值
        /// </summary>
        [ReadOnly(true), DisplayName("真空设置值")]
        [Category("基本信息")]
        public float VacuumSet
        {
            get
            {
                return vacuumSet;
            }
            set
            {
                if (vacuumSet != value)
                {
                    UpdateDbField("VacuumSet", value);
                }
                vacuumSet = value;
            }
        }


        private float yunFengTSet = -1;
        /// <summary>
        /// 运风温度设置
        /// </summary>
        [ReadOnly(true), DisplayName("运风温度设置")]
        [Category("基本信息")]
        public float YunFengTSet
        {
            get
            {
                return yunFengTSet;
            }
            set
            {
                if (yunFengTSet != value)
                {
                    UpdateDbField("YunFengTSet", value);
                }
                yunFengTSet = value;
            }
        }

        private int preheatTimeSet = -1;
        /// <summary>
        /// 预热时间设置
        /// </summary>
        [ReadOnly(true), DisplayName("预热时间设置")]
        [Category("基本信息")]
        public int PreheatTimeSet
        {
            get
            {
                return preheatTimeSet;
            }
            set
            {
                if (preheatTimeSet != value)
                {
                    UpdateDbField("PreheatTimeSet", value);
                }
                preheatTimeSet = value;
            }
        }

        private int bakingTimeSet = -1;
        /// <summary>
        /// Baking时间设置
        /// </summary>
        [ReadOnly(true), DisplayName("Baking时间设置")]
        [Category("基本信息")]
        public int BakingTimeSet
        {
            get
            {
                return bakingTimeSet;
            }
            set
            {
                if (bakingTimeSet != value)
                {
                    UpdateDbField("BakingTimeSet", value);
                }
                bakingTimeSet = value;
            }
        }

        private int breathingCycleSet = -1;
        /// <summary>
        /// 呼吸循环时间设置
        /// </summary>
        [ReadOnly(true), DisplayName("呼吸循环时间设置")]
        [Category("基本信息")]
        public int BreathingCycleSet
        {
            get
            {
                return breathingCycleSet;
            }
            set
            {
                if (breathingCycleSet != value)
                {
                    UpdateDbField("BreathingCycleSet", value);
                }
                breathingCycleSet = value;
            }
        }

        private DateTime bakingStartTime = TengDa.Common.DefaultTime;
        private DateTime bakingStopTime = TengDa.Common.DefaultTime;
        private DateTime inOvenTime = TengDa.Common.DefaultTime;
        private DateTime outOvenTime = TengDa.Common.DefaultTime;

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
            this.isInFinished = TengDa._Convert.StrToBool(rowInfo["IsInFinished"].ToString(), false);
            this.isOutFinished = TengDa._Convert.StrToBool(rowInfo["IsOutFinished"].ToString(), false);
            this.isInUploaded = TengDa._Convert.StrToBool(rowInfo["IsInUploaded"].ToString(), true);
            this.isOutUploaded = TengDa._Convert.StrToBool(rowInfo["IsOutUploaded"].ToString(), true);
            this.scanTime = TengDa._Convert.StrToDateTime(rowInfo["ScanTime"].ToString(), Common.DefaultTime);
            this.processTemperSet = TengDa._Convert.StrToFloat(rowInfo["ProcessTemperSet"].ToString(), -1);
            this.vacuumSet = TengDa._Convert.StrToFloat(rowInfo["VacuumSet"].ToString(), -1);
            for (int i = 0; i < Option.TemperatureSetPointCount; i++)
            {
                this.tsSet[i] = TengDa._Convert.StrToFloat(rowInfo[string.Format("T{0:D2}Set", i + 1)].ToString(), -1);
            }
            this.yunFengTSet = TengDa._Convert.StrToFloat(rowInfo["YunFengTSet"].ToString(), -1);
            this.preheatTimeSet = TengDa._Convert.StrToInt(rowInfo["PreheatTimeSet"].ToString(), -1);
            this.bakingTimeSet = TengDa._Convert.StrToInt(rowInfo["BakingTimeSet"].ToString(), -1);
            this.breathingCycleSet = TengDa._Convert.StrToInt(rowInfo["BreathingCycleSet"].ToString(), -1);
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
            var clamp = new Clamp(code);
            clamp.scanTime = DateTime.Now;
            return Add(clamp, out msg);
        }

        public static int Add(Clamp addClamp, out string msg)
        {
            return Database.Insert(string.Format("INSERT INTO [dbo].[{0}] ([Code], [UserId], [OvenStationId], [Location], [BakingStartTime], [BakingStopTime], [ScanTime], [InOvenTime], [OutOvenTime], [IsInFinished], [IsOutFinished], [IsInUploaded], [IsOutUploaded], [VacuumSet], [T01Set], [T02Set], [T03Set], [T04Set], [T05Set], [T06Set], [T07Set], [T08Set], [T09Set], [T10Set], [YunFengTSet], [PreheatTimeSet], [BakingTimeSet], [BreathingCycleSet], [ProcessTemperSet]) VALUES ('{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}', '{12}', '{13}', '{14}', '{15}', '{16}', '{17}', '{18}', '{19}', '{20}', '{21}', '{22}', '{23}', '{24}', '{25}', '{26}', '{27}', '{28}', '{29}')", TableName,
              addClamp.Code, TengDa.WF.Current.user.Id, addClamp.OvenStationId, addClamp.Location, 
              addClamp.BakingStartTime, addClamp.BakingStopTime, addClamp.ScanTime, addClamp.InOvenTime, addClamp.OutOvenTime,addClamp.IsInFinished,addClamp.IsOutFinished,addClamp.IsInUploaded,addClamp.IsOutUploaded,addClamp.VacuumSet,
              addClamp.TsSet[0], addClamp.TsSet[1], addClamp.TsSet[2], addClamp.TsSet[3], addClamp.TsSet[4], addClamp.TsSet[5], addClamp.TsSet[6], addClamp.TsSet[7], addClamp.TsSet[8], addClamp.TsSet[9], 
              addClamp.YunFengTSet, addClamp.PreheatTimeSet, addClamp.BakingTimeSet, addClamp.BreathingCycleSet, addClamp.ProcessTemperSet), out msg);
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
