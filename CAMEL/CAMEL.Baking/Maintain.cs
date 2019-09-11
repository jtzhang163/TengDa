using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading.Tasks;
using TengDa.WF;
using TengDa;
using System.ComponentModel;

namespace CAMEL.Baking
{
    public class Maintain : Service
    {
        private static string tableName = string.Empty;
        public static string TableName
        {
            get
            {
                if (string.IsNullOrEmpty(tableName))
                {
                    tableName = Config.DbTableNamePre + ".Maintain";
                }
                return tableName;
            }
        }
        private string name;
        [ReadOnly(true), DisplayName("名称")]
        public string Name
        {
            set
            {
                if (value != name)
                {
                    UpdateDbField("Name", value);
                }
                name = value;
            }
            get
            {
                return name;
            }

        }
        private DateTime clocktime;
        [ReadOnly(true), DisplayName("上一次清理时间")]
        public DateTime ClocKtime
        {
            set
            {
                if (clocktime != value)
                {
                    UpdateDbField("Clocktime", value);
                }
                clocktime = value;
            }
            get { return clocktime; }
        }
        private int tipNumber;
        [DisplayName("维护周期/天")]
        public int TipNumber
        {
            set
            {
                if (tipNumber != value)
                {
                    UpdateDbField("TipNumber", value);
                }
                tipNumber = value;
            }
            get
            {
                return tipNumber;
            }
        }
        private bool isCleared;
        [ReadOnly(true), DisplayName("是否清理")]
        public bool IsCleared
        {
            set
            {
                if (value != isCleared)
                {
                    UpdateDbField("IsCleared", value);
                }
                isCleared = value;
            }
            get
            {
                return isCleared;
            }
        }

        private static List<Maintain> _MaintainsList = new List<Maintain>();
        public static List<Maintain> MaintainsList
        {
            get
            {
                string msg = string.Empty;
                if (_MaintainsList.Count < 1)
                {
                    DataTable data = Database.Query("SELECT * FROM [dbo].[" + TableName + "]", out msg);
                    if (!string.IsNullOrEmpty(msg))
                    {
                        Error.Alert(msg);
                        return null;
                    }
                    if (data != null && data.Rows.Count > 0)
                        for (int i = 0; i < data.Rows.Count; i++)
                        {
                            Maintain maintain = new Maintain();
                            maintain.InitFields(data.Rows[i]);
                            _MaintainsList.Add(maintain);
                        }
                }

                return _MaintainsList;
            }
        }

        private void InitFields(DataRow row)
        {
            this.Id = TengDa._Convert.StrToInt(row["Id"].ToString(), -1);
            this.name = row["Name"].ToString();
            this.clocktime = TengDa._Convert.StrToDateTime(row["Clocktime"].ToString(), TengDa.Common.DefaultTime);
            this.isCleared = TengDa._Convert.StrToBool(row["IsCleared"].ToString(), true);
            this.TipNumber = TengDa._Convert.StrToInt(row["TipNumber"].ToString(), -1);
        }
    }
}
