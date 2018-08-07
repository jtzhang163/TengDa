using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using TengDa;
using TengDa.WF;
using VekenDll;

namespace Veken.Baking
{
    public class MES : TengDa.WF.MES.MES
    {

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
            this.IsEnable = Convert.ToBoolean(rowInfo["IsEnable"]);
            this.isOffline = Convert.ToBoolean(rowInfo["IsOffline"]);
        }
        #endregion

        #region 系统层列表
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

        public static bool GetInfo(string resource_id, string basket_no, out string tech_no, out List<Cell> cells, out List<TechStandard> techStandards, out string msg)
        {
            msg = string.Empty;
            try
            {
                string flag = Current.option.InOvenCheck ? "N" : "Y";
                DataAbstr.special = flag;
                DataAbstr obj = DataAbstr.GetInstance();
                string[] cellInfos = obj.GetData(resource_id, basket_no);
                if (cellInfos[0] == "OK")
                {
                    cells = JsonHelper.DeserializeJsonToList<Cell>(cellInfos[1]);
                    tech_no = cellInfos[2];
                    string jsonTechStandard = obj.GetTechnologyStandard(tech_no);
                    techStandards = JsonHelper.DeserializeJsonToList<TechStandard>(jsonTechStandard);
                    return true;
                }
                else
                {
                    msg = cellInfos[0];
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }

            cells = new List<Cell>();
            techStandards = new List<TechStandard>();
            tech_no = string.Empty;
            return false;
        }

        public static bool UploadCellInfo(Clamp clamp)
        {
            Floor floor = null;
            for (int i = 0; i < Current.ovens.Count; i++)
            {
                for (int j = 0; j < Current.ovens[i].Floors.Count; j++)
                {
                    floor = Current.ovens[i].Floors[j];
                }
            }

            string cz_date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string timeSpan = (floor.RunMinutesSet / 60).ToString();
            string startTimeS = clamp.BakingStartTime < DateTime.Parse("2010-01-01") ? "" : clamp.BakingStartTime.ToString("yyyy-MM-dd HH:mm:ss");
            string stopTimeS = clamp.BakingStopTime < DateTime.Parse("2010-01-01") ? "" : clamp.BakingStopTime.ToString("yyyy-MM-dd HH:mm:ss");
            string outTimeS = clamp.OutTime < DateTime.Parse("2010-01-01") ? "" : clamp.OutTime.ToString("yyyy-MM-dd HH:mm:ss");
            string vacuumS = clamp.Vacuum.ToString();
            string techNo = clamp.TechNo;

            try
            {
                DataAbstr obj = DataAbstr.GetInstance();
                foreach (Battery battery in clamp.Batteries)
                {
                    string sfc = battery.Code;
                    string[] data = new string[11] { clamp.Code, Current.option.TemperatureSet, timeSpan, clamp.Temperature.ToString(), vacuumS, "", startTimeS, stopTimeS, "", outTimeS, techNo };

                    string cz_user = string.IsNullOrEmpty(TengDa.WF.Current.user.Number) ? "" : TengDa.WF.Current.user.Name;
                    if (obj.UploadData_F(sfc, floor.Number, cz_date, cz_user, clamp.clampError.Id == 1 ? "OK" : "NG", clamp.clampError.Id == 1 ? "" : clamp.clampError.ErrorCode, data, "") != -1)
                    {
                        return false;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Error.Alert(ex);
            }
            return false;
        }

        public static bool UploadMachineInfo(MachineInfo info)
        {
            string cz_date = info.Time.ToString("yyyy-MM-dd HH:mm:ss");
            string[] data = new string[6] { info.ActivationRate, info.FinalProductsRate, info.FailureRate, info.UtilizationRate, info.ErrorCode, info.ErrorDescription };
            string status = info.machineStatus.ToString();
            string resource_id = (from f in Floor.FloorList where f.Id == info.FloorId select f).ToList()[0].Number;

            try
            {
                DataAbstr obj = DataAbstr.GetInstance();
                if (obj.UploadData_S(resource_id, cz_date, status, data) == -1)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                Error.Alert(ex);
            }
            return false;
        }

        public static bool GetUserName(string userNumber, out string userName, out string msg)
        {
            msg = string.Empty;
            try
            {
                userName = VekenDll.Util.StringUtil.GetUserName(userNumber);
                if (!string.IsNullOrEmpty(userName))
                {
                    return true;
                }
                else
                {
                    msg = "MES中不存在用户工号：" + userNumber;
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError("工号 = " + userNumber + " 异常" + ex.ToString());
                msg = ex.Message;
            }
            userName = string.Empty;
            return false;
        }
        #endregion
    }
}