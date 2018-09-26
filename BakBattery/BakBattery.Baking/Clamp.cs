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
        /// 夹具品质Id
        /// </summary>
        [Browsable(false)]
        public int QualityStatusId
        {
            get
            {
                return qualityStatusId;
            }
            set
            {
                if (qualityStatusId != value)
                {
                    UpdateDbField("QualityStatusId", value);
                }
                qualityStatusId = value;
            }
        }

        [Browsable(false)]
        public ClampQualityStatus ClampQualityStatus
        {
            get
            {
                return new ClampQualityStatus(QualityStatusId);
            }
        }

        /// <summary>
        /// 品质状态
        /// </summary>
        [Description("品质状态 OK:无不良 HK001:环境温湿度不合格 ...")]
        [DisplayName("品质状态")]
        [Category("基本信息")]
        public QualityStatus QualityStatus
        {
            get
            {
                return (QualityStatus)Enum.Parse(typeof(QualityStatus), ClampQualityStatus.Code);
            }
            set
            {
                QualityStatusId = (from s in ClampQualityStatus.StatusList where s.Code == value.ToString() select s).ToList()[0].Id;
            }
        }

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
        /// 真空度（用于离线时上传MES）
        /// </summary>
        [ReadOnly(true), DisplayName("上传MES的真空度")]
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

        /// <summary>
        /// 温度（用于离线时上传MES）
        /// </summary>
        [ReadOnly(true), Description("温度")]
        [DisplayName("温度")]
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

        /// <summary>
        /// 出烤箱温度
        /// </summary>
        [ReadOnly(true), Description("出烤箱温度")]
        [DisplayName("出烤箱温度")]
        [Category("基本信息")]
        public float OutOvenTemp
        {
            get
            {
                return outOvenTemp;
            }
            set
            {
                if (outOvenTemp != value)
                {
                    UpdateDbField("OutOvenTemp", value);
                }
                outOvenTemp = value;
            }
        }

        /// <summary>
        /// 极片水分
        /// </summary>
        [ReadOnly(true), DisplayName("极片水分")]
        [Category("基本信息")]
        public float WaterContent
        {
            get
            {
                return waterContent;
            }
            set
            {
                if (waterContent != value)
                {
                    UpdateDbField("WaterContent", value);
                }
                waterContent = value;
            }
        }

        /// <summary>
        /// 夹具扭力
        /// </summary>
        [ReadOnly(true), DisplayName("夹具扭力")]
        [Category("基本信息")]
        public float ClampTorsion
        {
            get
            {
                return clampTorsion;
            }
            set
            {
                if (clampTorsion != value)
                {
                    UpdateDbField("ClampTorsion", value);
                }
                clampTorsion = value;
            }
        }


        /// <summary>
        /// 已完成Baking
        /// </summary>
        [Description("是否已完成(已出炉，可上传MES)")]
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

        /// <summary>
        /// 已上传
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

        protected bool isUploaded;
        private int qualityStatusId = 1;
        private int ovenStationId;
        protected bool isFinished;
        private float temperature;
        private float outOvenTemp;
        private float waterContent;
        private float clampTorsion = 5.5f;
        private float vacuum;
        private DateTime bakingStartTime = TengDa.Common.DefaultTime;
        private DateTime bakingStopTime = TengDa.Common.DefaultTime;

        public const int BatteryCount = 48;


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
            this.bakingStartTime = TengDa._Convert.StrToDateTime(rowInfo["BakingStartTime"].ToString(), Common.DefaultTime);
            this.bakingStopTime = TengDa._Convert.StrToDateTime(rowInfo["BakingStopTime"].ToString(), Common.DefaultTime);
            this.ovenStationId = TengDa._Convert.StrToInt(rowInfo["OvenStationId"].ToString(), -1);
            this.location = rowInfo["Location"].ToString().Trim();
            this.vacuum = System.Convert.ToSingle(rowInfo["Vacuum"]);
            this.temperature = System.Convert.ToSingle(rowInfo["Temperature"]);
            this.isFinished = TengDa._Convert.StrToBool(rowInfo["IsFinished"].ToString(), false);
            this.isUploaded = TengDa._Convert.StrToBool(rowInfo["IsUploaded"].ToString(), true);
            this.qualityStatusId = TengDa._Convert.StrToInt(rowInfo["QualityStatusId"].ToString(), 1);
            this.scanTime = TengDa._Convert.StrToDateTime(rowInfo["ScanTime"].ToString(), Common.DefaultTime);
            this.outOvenTemp = TengDa._Convert.StrToFloat(rowInfo["OutOvenTemp"].ToString(), 0);
            this.waterContent = TengDa._Convert.StrToFloat(rowInfo["WaterContent"].ToString(), 0);
            this.clampTorsion = TengDa._Convert.StrToFloat(rowInfo["ClampTorsion"].ToString(), 0);
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
            return Add(new Clamp(code), out msg);
        }

        public static int Add(Clamp addClamp, out string msg)
        {
            return Database.Insert(string.Format("INSERT INTO [dbo].[{0}] ([Code], [UserId], [OvenStationId], [Location], [BakingStartTime], [BakingStopTime], [Vacuum], [Temperature], [IsFinished], [QualityStatusId], [ScanTime], [OutOvenTemp], [WaterContent], [ClampTorsion], [IsUploaded]) VALUES ('{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}', '{12}', '{13}', '{14}', 'false')", TableName,
              addClamp.Code, TengDa.WF.Current.user.Id, addClamp.OvenStationId, addClamp.Location, addClamp.BakingStartTime, addClamp.BakingStopTime, addClamp.Vacuum, addClamp.Temperature, addClamp.IsFinished,
              1, DateTime.Now, addClamp.OutOvenTemp, addClamp.WaterContent, addClamp.ClampTorsion), out msg);
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
