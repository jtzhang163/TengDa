using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using TengDa;
using TengDa.WF;

namespace Veken.Baking
{
    public class TechStandard4DB
    {
        private static string tableName = string.Empty;
        public static string TableName
        {
            get
            {
                if (string.IsNullOrEmpty(tableName))
                {
                    tableName = Config.DbTableNamePre + ".TechStandard";
                }
                return tableName;
            }
        }
        /// <summary>
        /// 增加多个，数据库一次插入多行
        /// </summary>
        /// <param name="addBatteries"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static bool Add(List<TechStandard> addTechStandards, int ClampId, out string msg)
        {
            if (addTechStandards.Count < 1)
            {
                msg = string.Empty;
                return true;
            }

            StringBuilder sb = new StringBuilder();

            foreach (TechStandard ts in addTechStandards)
            {
                sb.Append(string.Format("('{0}', '{1}', '{2}', '{3}', '{4}', {5}),", ts.INSPECTION_ITEM, ts.INSPECTION_DESC, ts.STANDARD, ts.UPPER_LIMIT, ts.LOWER_LIMIT, ClampId));
            }

            try
            {
                return Database.NonQuery(string.Format("INSERT INTO [dbo].[{0}] ([INSPECTION_ITEM], [INSPECTION_DESC], [STANDARD], [UPPER_LIMIT], [LOWER_LIMIT], [ClampId]) VALUES {1}", TableName, sb.ToString().TrimEnd(',')), out msg);
            }
            catch (Exception ex)
            {
                msg = ex.Message;

            }
            return false;
        }


        public static List<TechStandard> Get(int ClampId, out string msg)
        {
            List<TechStandard> techStandards = new List<TechStandard>();
            DataTable dt = Database.Query(string.Format("SELECT * FROM [dbo].[{0}] WHERE ClampId = {1}", TableName, ClampId), out msg);
            if (!string.IsNullOrEmpty(msg))
            {
                Error.Alert(msg);
                return techStandards;
            }

            if (dt == null || dt.Rows.Count == 0)
            {
                return techStandards;
            }

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TechStandard techStandard = new TechStandard();

                techStandard.INSPECTION_ITEM = dt.Rows[i]["INSPECTION_ITEM"].ToString();
                techStandard.INSPECTION_DESC = dt.Rows[i]["INSPECTION_DESC"].ToString();
                techStandard.STANDARD = dt.Rows[i]["STANDARD"].ToString();
                techStandard.UPPER_LIMIT = dt.Rows[i]["UPPER_LIMIT"].ToString();
                techStandard.LOWER_LIMIT = dt.Rows[i]["LOWER_LIMIT"].ToString();

                techStandards.Add(techStandard);
            }

            return techStandards;
        }
    }
}
