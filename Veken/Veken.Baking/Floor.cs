using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using TengDa;
using System.ComponentModel;
using System.Drawing;
using TengDa.WF;

namespace Veken.Baking
{
    public class Floor : TengDa.WF.Terminals.Terminal
    {

        #region 属性字段

        private static string tableName = string.Empty;
        public static string TableName
        {
            get
            {
                if (string.IsNullOrEmpty(tableName))
                {
                    tableName = Config.DbTableNamePre + ".Floors";
                }
                return tableName;
            }
        }

        [ReadOnly(true), Description("已运行时间，单位：min")]
        [DisplayName("已运行时间")]
        public int RunMinutes
        {
            get
            {
                if (IsBaking)
                {
                    return runMinutes;
                }
                return 0;
            }
            set
            {
                runMinutes = value;
            }
        }
        private int runMinutes = 0;

        [ReadOnly(true), Description("总运行时间设置，单位：min")]
        [DisplayName("总运行时间设置")]
        public int RunMinutesSet { get; set; }

        public float[] Temperatures = new float[Option.TemperaturePointCount];
        [ReadOnly(true), Description("真空度，单位：Pa")]
        [DisplayName("真空度")]
        public float Vacuum { get; set; }

        [ReadOnly(true), Description("是否正在烘烤")]
        [DisplayName("是否正在烘烤")]
        public bool IsBaking { get; set; }

        [ReadOnly(true), Description("是否正在自动运行")]
        [DisplayName("是否正在自动运行")]
        public RunMode Runmode { get; set; } = RunMode.未运行;

        public int OvenId { get; private set; }

        private string clampIds = string.Empty;
        [ReadOnly(true)]
        public string ClampIds
        {
            get { return clampIds; }
            set
            {
                string msg = string.Empty;
                if (Database.NonQuery(string.Format("UPDATE [dbo].[{0}] SET ClampIds = '{1}' WHERE Id = {2}", Floor.TableName, value, this.Id), out msg))
                {
                    clampIds = value;
                }
            }
        }

        public FloorStatus PreFloorStatus;

        public FloorStatus floorstatus = FloorStatus.空腔;
        [ReadOnly(true)]
        [DisplayName("当前腔体状态")]
        public FloorStatus floorStatus
        {
            get
            {
                if (floorstatus != PreFloorStatus)
                {
                    UpdateDbField("FloorStatus", floorstatus);
                }
                return floorstatus;
            }
            set
            {
                if(floorstatus != value)
                {
                    UpdateDbField("FloorStatus", value);
                }
                floorstatus = value;
            }
        }
        #endregion

        #region 该腔体下料盒列表
        private List<Clamp> clamps = null;
        [Browsable(false)]
        public List<Clamp> Clamps
        {
            get
            {
                if (clamps == null)
                {
                    List<Clamp> list = new List<Clamp>();
                    string[] clampIds = ClampIds.Split(',');
                    for (int i = 0; i < clampIds.Length; i++)
                    {
                        if (!string.IsNullOrEmpty(clampIds[i]))
                        {
                            Clamp clamp = new Clamp(TengDa._Convert.StrToInt(clampIds[i], -1));
                            list.Add(clamp);
                        }
                    }
                    clamps = list;
                }
                return clamps;
            }
        }

        #endregion

        #region 绘制温度曲线相关

        public List<float>[] sampledDatas = new List<float>[Option.TemperaturePointCount];//采样数据1

        private string curveIndexs = string.Empty;
        [ReadOnly(true),Description("是否启用绘制曲线")]
        public string CurveIndexs
        {
            get
            {
                return curveIndexs;
            }
            set
            {
                if(curveIndexs != value)
                {
                    this.UpdateDbField("CurveIndexs", value);
                }
                curveIndexs = value;
            }
        }

        #endregion

        #region 系统腔体列表
        private static List<Floor> floorList = new List<Floor>();
        public static List<Floor> FloorList
        {
            get
            {
                if(floorList.Count < 1)
                {
                    string msg = string.Empty;

                    DataTable dt = Database.Query("SELECT * FROM [dbo].[" + TableName + "]", out msg);
                    if (!string.IsNullOrEmpty(msg))
                    {
                        Error.Alert(msg);
                        return null;
                    }

                    if (dt != null && dt.Rows.Count > 0)
                    {

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            Floor floor = new Floor();
                            floor.InitFields(dt.Rows[i]);
                            floorList.Add(floor);
                        }
                    }

                }

                return floorList;
            }
        }

        #endregion

        #region 获取腔体

        public static Floor GetFloor(string name, out string msg)
        {
            try
            {
                List<Floor> list = (from floor in FloorList where floor.Name == name select floor).ToList();
                if (list.Count() > 0)
                {
                    msg = string.Empty;
                    return list[0];
                }
                msg = string.Format("数据库不存在名称为 {0} 的烤箱！", name);
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }

            return null;
        }

        #endregion

        #region 构造方法

        public Floor() : this(-1) { }

        public Floor(int id)
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
            this.clampIds = rowInfo["ClampIds"].ToString();
            this.company = rowInfo["Company"].ToString();
            this.model = rowInfo["Model"].ToString();
            this.number = rowInfo["Number"].ToString();
            this.curveIndexs = rowInfo["CurveIndexs"].ToString();
            this.floorstatus = (FloorStatus)Enum.Parse(typeof(FloorStatus), rowInfo["FloorStatus"].ToString());
            this.PreFloorStatus = this.floorstatus;

            for (int k = 0; k < Option.TemperaturePointCount; k++)
            {
                this.sampledDatas[k] = new List<float>();
            }
        }
        #endregion
    }

    public enum FloorStatus
    {
        空腔,
        待烘烤,
        烘烤中,
        待出腔,
        未定义
    }

    public enum RunMode
    {
        未运行,
        自动,
        手动
    }
}
