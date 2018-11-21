using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using TengDa;
using TengDa.WF;

namespace Soundon.Dispatcher
{
    public class Alarm
    {
        #region 属性
        private static string tableName = string.Empty;
        public static string TableName
        {
            get
            {
                if (string.IsNullOrEmpty(tableName))
                {
                    tableName = Config.DbTableNamePre + ".Alarm";
                }
                return tableName;
            }
        }

        /// <summary>
        /// Id
        /// </summary>
        [ReadOnly(true)]
        public int Id { get; set; }

        /// <summary>
        /// 字地址
        /// </summary>
        [ReadOnly(true)]
        public string WordAdd { get; set; }

        /// <summary>
        /// 位地址1
        /// </summary>
        [ReadOnly(true)]
        public string BitAdd1 { get; set; }

        /// <summary>
        /// 位地址2
        /// </summary>
        [ReadOnly(true)]
        public string BitAdd2 { get; set; }

        /// <summary>
        /// 报警文本
        /// </summary>
        [ReadOnly(true)]
        public string AlarmStr { get; set; }

        /// <summary>
        /// 腔体次序
        /// </summary>
        [ReadOnly(true)]
        public int FloorNum { get; set; }
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
            this.WordAdd = rowInfo["WordAdd"].ToString().Trim();
            this.BitAdd1 = rowInfo["BitAdd1"].ToString().Trim();
            this.BitAdd2 = rowInfo["BitAdd2"].ToString().Trim();
            this.AlarmStr = rowInfo["AlarmStr"].ToString().Trim();
            this.FloorNum = TengDa._Convert.StrToInt(rowInfo["FloorNum"].ToString(), -1);
        }
        #endregion

        private static List<Alarm> alarms = new List<Alarm>();

        public static List<Alarm> Alarms
        {
            get
            {
                if (alarms.Count < 1)
                {
                    string msg = string.Empty;
                    DataTable dt = Database.Query(string.Format("SELECT * FROM [dbo].[{0}]", TableName), out msg);

                    if (!string.IsNullOrEmpty(msg))
                    {
                        Error.Alert(msg);
                        return alarms;
                    }

                    if (dt == null || dt.Rows.Count == 0)
                    {
                        return alarms;
                    }
                    else
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            Alarm alarm = new Alarm();
                            alarm.InitFields(dt.Rows[i]);
                            alarms.Add(alarm);
                        }
                    }
                }

                return alarms;
            }
        }
    }

    public class AlarmLog
    {
        #region 属性
        private static string tableName = string.Empty;
        public static string TableName
        {
            get
            {
                if (string.IsNullOrEmpty(tableName))
                {
                    tableName = Config.DbTableNamePre + ".AlarmLog";
                }
                return tableName;
            }
        }

        /// <summary>
        /// Id
        /// </summary>
        [ReadOnly(true)]
        public int Id { get; set; }

        public int AlarmId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [ReadOnly(true)]
        public AlarmType AlarmType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [ReadOnly(true)]
        public int TypeId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [ReadOnly(true)]
        public int UserId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [ReadOnly(true)]
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [ReadOnly(true)]
        public DateTime StopTime { get; set; }
        #endregion

        public int Clamp1Id { get; set; }

        public int Clamp2Id { get; set; }

        #region 添加报警/结束报警
        /// <summary>
        /// 增加多个，数据库一次插入多行
        /// </summary>
        /// <param name="addBatteries"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static bool Add(List<AlarmLog> addAlarmLogs, out string msg)
        {
            if (addAlarmLogs.Count < 1)
            {
                msg = string.Empty;
                return true;
            }

            StringBuilder sb = new StringBuilder();

            foreach (AlarmLog alarmLog in addAlarmLogs)
            {
                sb.Append(string.Format("({0}, '{1}', {2}, {3}, GETDATE(), '2000-01-01 00:00:00', {4}, {5}),", alarmLog.AlarmId, alarmLog.AlarmType, alarmLog.TypeId, TengDa.WF.Current.user.Id, alarmLog.Clamp1Id, alarmLog.Clamp2Id));
            }

            return Database.NonQuery(string.Format("INSERT INTO [dbo].[{0}] ([AlarmId], [AlarmType], [TypeId], [UserId], [StartTime], [StopTime], [Clamp1Id], [Clamp2Id]) VALUES {1}", TableName, sb.ToString().TrimEnd(',')), out msg);
        }

        public static void StopAll()
        {
            string msg = string.Empty;
            Database.NonQuery(string.Format("UPDATE [dbo].[{0}] SET [StopTime] = GETDATE() WHERE [StopTime] < '2001-01-01'", TableName), out msg);
            if (!string.IsNullOrEmpty(msg))
            {
                LogHelper.WriteError(msg);
            }
        }

        public static bool Stop(AlarmType alarmType, int alarmId, int typeId, out string msg)
        {
            return Database.NonQuery(string.Format("UPDATE [dbo].[{0}] SET [StopTime] = GETDATE() WHERE [StopTime] < '2001-01-01' AND [AlarmId] = {1} AND [AlarmType] = '{2}' AND [TypeId] = {3}", TableName, alarmId, alarmType, typeId), out msg);
        }

        #endregion
    }

    public enum AlarmType
    {
        Floor,
        Oven
    }
}
