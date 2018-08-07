using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using TengDa;
using TengDa.WF;

namespace Veken.Baking
{
    /// <summary>
    /// 料盒
    /// </summary>
    public class Clamp :TengDa.WF.Products.Product
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
        /// 层Id
        /// </summary>
        [ReadOnly(true)]
        public int FloorId { get; set; }
        /// <summary>
        /// 用户Id
        /// </summary>
        [ReadOnly(true)]
        public int UserId { get; set; }

        /// <summary>
        /// 不良Id
        /// </summary>
        [Browsable(false)]
        public int ErrorId
        {
            get
            {
                return errorId;
            }
            set
            {
                if (errorId != value)
                {
                    UpdateDbField("ErrorId", value);
                }
                errorId = value;
            }
        }
        private int errorId = -1;

        [Browsable(false)]
        public ClampError clampError
        {
            get
            {
                return new ClampError(ErrorId);
            }
        }

        /// <summary>
        /// 不良代码
        /// </summary>
        [Description("不良代码 OK:无不良 HK001:环境温湿度不合格 HK002:露点超标 HK003:来料外观/尺寸不良 HK004:烘箱温度不合格 HK005:烘箱真空度不合格 HK006:电芯水分超标 HK007:烘烤时间不足")]
        [DisplayName("不良代码")]
        [Category("基本信息")]
        public ErrorCode errorCode
        {
            get
            {
                return (ErrorCode)Enum.Parse(typeof(ErrorCode), clampError.ErrorCode);
            }
            set
            {
                ErrorId = (from e in ClampError.ErrorList where e.ErrorCode == value.ToString() select e).ToList()[0].Id;
            }
        }

        /// <summary>
        /// 入腔时间
        /// </summary>
        [ReadOnly(true),Description("入腔时间")]
        [DisplayName("入腔时间")]
        [Category("基本信息")]
        public DateTime EnterTime { get; set; }
        /// <summary>
        /// 出腔时间
        /// </summary>
        [ReadOnly(true),Description("出腔时间")]
        [DisplayName("出腔时间")]
        [Category("基本信息")]
        public DateTime OutTime { get; set; }
        /// <summary>
        /// Baking开始时间
        /// </summary>
        [ReadOnly(true), Description("开始烘烤时间")]
        [DisplayName("开始烘烤时间")]
        [Category("基本信息")]
        public DateTime BakingStartTime { get; set; }
        /// <summary>
        /// Baking结束时间
        /// </summary>
        [ReadOnly(true), Description("结束烘烤时间")]
        [DisplayName("结束烘烤时间")]
        [Category("基本信息")]
        public DateTime BakingStopTime { get; set; }
        /// <summary>
        /// 真空度（用于离线时上传MES）
        /// </summary>
        [ReadOnly(true), Description("上传MES的真空度")]
        [DisplayName("真空度")]
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
        [ReadOnly(true), Description("上传MES的温度")]
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
        /// 已上传
        /// </summary>
        [Description("是否已上传MES")]
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
        /// 型号
        /// </summary>
        [Description("型号(从MES中获得)")]
        [DisplayName("型号")]
        [Category("基本信息")]
        public string TechNo
        {
            get
            {
                return techNo;
            }
            set
            {
                if (techNo != value)
                {
                    UpdateDbField("TechNo", value);
                }
                techNo = value;
            }
        }

        public bool isFinished;
        public bool isUploaded;
        private float temperature;
        private float vacuum;

        public string techNo = string.Empty;


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
            this.EnterTime = TengDa._Convert.StrToDateTime(rowInfo["EnterTime"].ToString(), DateTime.Parse("2000-01-01 00:00:00"));
            this.OutTime = TengDa._Convert.StrToDateTime(rowInfo["OutTime"].ToString(), DateTime.Parse("2000-01-01 00:00:00"));
            this.BakingStartTime = TengDa._Convert.StrToDateTime(rowInfo["BakingStartTime"].ToString(), DateTime.Parse("2000-01-01 00:00:00"));
            this.BakingStopTime = TengDa._Convert.StrToDateTime(rowInfo["BakingStopTime"].ToString(), DateTime.Parse("2000-01-01 00:00:00"));
            this.FloorId = TengDa._Convert.StrToInt(rowInfo["FloorId"].ToString(), -1);
            this.location = rowInfo["Location"].ToString().Trim();
            this.vacuum = System.Convert.ToSingle(rowInfo["Vacuum"]);
            this.temperature = System.Convert.ToSingle(rowInfo["Temperature"]);
            this.isUploaded = Convert.ToBoolean(rowInfo["IsUploaded"]);
            this.isFinished = Convert.ToBoolean(rowInfo["IsFinished"]);
            this.errorId = TengDa._Convert.StrToInt(rowInfo["ErrorId"].ToString(), 1);
            this.techNo = rowInfo["TechNo"].ToString().Trim();
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
                if (this.Id > 0 && batteries.Count < 1)
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

        public static int Add(Clamp addClamp, out string msg)
        {
            return Database.Insert(string.Format("INSERT INTO [dbo].[{0}] ([Code], [UserId], [EnterTime], [OutTime], [FloorId], [Location], [BakingStartTime], [BakingStopTime], [Vacuum], [Temperature], [IsUploaded], [IsFinished], [ErrorId], [TechNo]) VALUES ('{1}', {2}, '{3}', '{4}', {5}, '{6}', '{7}', '{8}', {9}, {10}, '{11}', '{12}', 1, '{13}')", TableName, addClamp.Code, TengDa.WF.Current.user.Id, addClamp.EnterTime, addClamp.OutTime, addClamp.FloorId, addClamp.Location, addClamp.BakingStartTime, addClamp.BakingStopTime, addClamp.Vacuum, addClamp.Temperature, addClamp.IsUploaded, addClamp.IsFinished, addClamp.TechNo), out msg);
        }

        public static bool Delete(Clamp delClamp, out string msg)
        {
            return Database.NonQuery(string.Format("DELETE FROM	[dbo].[{0}] WHERE [Code] = '{1}'", TableName, delClamp.Code), out msg);
        }

        public static bool Update(Clamp newClamp, out string msg)
        {
            return Database.NonQuery(string.Format("UPDATE [dbo].[{0}] SET [EnterTime] = '{1}', [OutTime] = '{2}', [BakingStartTime] = '{3}', [BakingStopTime] = '{4}', [Vacuum] = {5}, [Temperature] = {6}, [IsUploaded] = '{7}', [IsFinished] = '{8}' WHERE [Id] = {9}", TableName, newClamp.EnterTime, newClamp.OutTime, newClamp.BakingStartTime, newClamp.BakingStopTime, newClamp.Vacuum, newClamp.Temperature, newClamp.IsUploaded, newClamp.IsFinished, newClamp.Id), out msg);
        }     
        #endregion
    }
}
