using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using TengDa;
using TengDa.WF;

namespace Tafel.ScanSystem
{
    public static class BatteryMatrix
    {
        private static string tableName = string.Empty;
        private static string TableName
        {
            get
            {
                if (string.IsNullOrEmpty(tableName))
                {
                    tableName = Config.DbTableNamePre + ".BatteryMatrix";
                }
                return tableName;
            }
        }

        public static void DbClearAll()
        {
            string msg = string.Empty;
            if (!Database.NonQuery(string.Format("UPDATE [dbo].[{0}] SET [Col1] = '', [Col2] = '', [Col3] = ''", TableName), out msg))
            {
                Error.Alert(msg);
            }
        }

        public static DataTable GetDataTable()
        {
            string msg = string.Empty;
            DataTable dt = Database.Query("SELECT * FROM [dbo].[" + TableName + "]", out msg);
            if (!string.IsNullOrEmpty(msg))
            {
                Error.Alert(msg);
            }
            return dt;
        }

        public static void Update(int i, int j, int k, string code)
        {
            string msg = string.Empty;
            if (!Database.NonQuery(string.Format("UPDATE [dbo].[{0}] SET [Col{1}] = '{2}' WHERE ClampId = {3} AND RowId = {4}", TableName, k + 1, code, i + 1, j + 1), out msg))
            {
                Error.Alert(msg);
            }
        }
    }
}
