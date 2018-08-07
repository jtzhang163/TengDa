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
    public class MachineInfo
    {
        #region 属性
        private static string tableName = string.Empty;
        public static string TableName
        {
            get
            {
                if (string.IsNullOrEmpty(tableName))
                {
                    tableName = Config.DbTableNamePre + ".MachineInfo";
                }
                return tableName;
            }
        }


        public int Id { get; set; }

        public int FloorId { get; set; }

        public MachineStatus machineStatus { get; set; }

        /// <summary>
        /// 稼动率
        /// </summary>
        public string ActivationRate { get; set; }

        /// <summary>
        /// 成品率
        /// </summary>
        public string FinalProductsRate { get; set; }

        /// <summary>
        /// 故障率
        /// </summary>
        public string FailureRate { get; set; }

        /// <summary>
        /// 开动率
        /// </summary>
        public string UtilizationRate { get; set; }

        /// <summary>
        /// 故障代码
        /// </summary>
        public string ErrorCode { get; set; }

        /// <summary>
        /// 故障描述
        /// </summary>
        public string ErrorDescription { get; set; }

        /// <summary>
        /// 时间
        /// </summary>
        public DateTime Time { get; set; }

        /// <summary>
        /// 是否已经上传
        /// </summary>
        public bool IsUploaded { get; set; }

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
            this.FloorId = TengDa._Convert.StrToInt(rowInfo["FloorId"].ToString(), -1);
            this.machineStatus = (MachineStatus)Enum.Parse(typeof(MachineStatus), rowInfo["MachineStatus"].ToString());
            this.ActivationRate = rowInfo["ActivationRate"].ToString();
            this.FinalProductsRate = rowInfo["FinalProductsRate"].ToString();
            this.FailureRate = rowInfo["FailureRate"].ToString();
            this.UtilizationRate = rowInfo["UtilizationRate"].ToString();
            this.ErrorCode = rowInfo["ErrorCode"].ToString();
            this.ErrorDescription = rowInfo["ErrorDescription"].ToString();
            this.Time = TengDa._Convert.StrToDateTime(rowInfo["Time"].ToString(), DateTime.Parse("2000-01-01"));
            this.IsUploaded = System.Convert.ToBoolean(rowInfo["IsUploaded"]);
        }
        #endregion

        #region 添加/设置已上传

        public static List<MachineInfo> GetList(string sql, out string msg)
        {
            List<MachineInfo> list = new List<MachineInfo>();
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
                    MachineInfo machineInfo = new MachineInfo();
                    machineInfo.InitFields(dt.Rows[i]);
                    list.Add(machineInfo);
                }
            }
            return list;
        }

        /// <summary>
        /// 增加多个，数据库一次插入多行
        /// </summary>
        /// <param name="addBatteries"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static bool Add(List<MachineInfo> addMachineInfos, out string msg)
        {
            if (addMachineInfos.Count < 1)
            {
                msg = string.Empty;
                return true;
            }

            StringBuilder sb = new StringBuilder();

            foreach (MachineInfo m in addMachineInfos)
            {
                sb.Append(string.Format("({0}, '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', GETDATE(), 'False'),",
                    m.FloorId, m.machineStatus, m.ActivationRate, m.FinalProductsRate,
                    m.FailureRate, m.UtilizationRate, m.ErrorCode, m.ErrorDescription));
            }

            return Database.NonQuery(string.Format("INSERT INTO [dbo].[{0}] ([FloorId], [MachineStatus], [ActivationRate], [FinalProductsRate], [FailureRate], [UtilizationRate], [ErrorCode], [ErrorDescription], [Time], [IsUploaded]) VALUES {1}", TableName, sb.ToString().TrimEnd(',')), out msg);
        }

        public static List<MachineInfo> SelectNotUploaded()
        {
            string msg = string.Empty;
            List<MachineInfo> list = GetList(string.Format("SELECT * FROM [dbo].[{0}] WHERE [IsUploaded] = 'False'", TableName), out msg);
            if (!string.IsNullOrEmpty(msg))
            {
                LogHelper.WriteError(msg);
            }
            return list;
        }

        public static bool UploadFinished(int id, out string msg)
        {
            return Database.NonQuery(string.Format("UPDATE [dbo].[{0}] SET [IsUploaded] = 'True' WHERE [Id] = {1}", TableName, id), out msg);
        }

        #endregion
    }
    /// <summary>
    /// 设备状态  A工作 B待机 C故障 D关机
    /// </summary>
    public enum MachineStatus
    {
        A,
        B, 
        C,
        D
    }
}
