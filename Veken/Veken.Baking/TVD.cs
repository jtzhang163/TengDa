using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TengDa;
using TengDa.WF;

namespace Veken.Baking
{
    /// <summary>
    /// 真空温度
    /// </summary>
    public class TVD
    {
        private static string tableName = string.Empty;
        public static string TableName
        {
            get
            {
                if (string.IsNullOrEmpty(tableName))
                {
                    tableName = Config.DbTableNamePre + ".TVD";
                }
                return tableName;
            }
        }
        public int Id { get; set; }
        public int FloorId { get; set; }
        public int RunMinutes { get; set; }
        public int UserId { get; set; }
        public float V1 { get; set; }
        public float[] T = new float[Option.TemperaturePointCount];

        public static int Add(TVD addTVD, out string msg)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(string.Format("INSERT INTO [dbo].[{0}] ", TableName));
            sb.Append("([FloorId], [T1], [T2], [T3], [T4], [T5], [T6], [T7], [T8], [V1], [RunMinutes], [Time], [UserId]) ");
            sb.Append(string.Format("VALUES ({0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, '{11}', {12})",
               addTVD.FloorId,
               addTVD.T[0], addTVD.T[1], addTVD.T[2], addTVD.T[3], addTVD.T[4], addTVD.T[5], addTVD.T[6], addTVD.T[7],
               addTVD.V1, addTVD.RunMinutes, DateTime.Now, addTVD.UserId));
            return Database.Insert(sb.ToString(), out msg);
        }
        /// <summary>
        /// 多条数据一次插入
        /// </summary>
        /// <param name="addTVDs"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static bool Add(List<TVD> addTVDs, out string msg)
        {
            if (addTVDs.Count < 1)
            {
                msg = string.Empty;
                return true;
            }

            StringBuilder sb = new StringBuilder();
            sb.Append(string.Format("INSERT INTO [dbo].[{0}] ", TableName));
            sb.Append("([FloorId], [T1], [T2], [T3], [T4], [T5], [T6], [T7], [T8], [V1], [RunMinutes], [Time], [UserId]) VALUES ");

            foreach (TVD addTVD in addTVDs)
            {
                sb.Append(string.Format("({0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, '{11}', {12}),",
                   addTVD.FloorId,
                   addTVD.T[0], addTVD.T[1], addTVD.T[2], addTVD.T[3], addTVD.T[4], addTVD.T[5], addTVD.T[6], addTVD.T[7],
                   addTVD.V1, addTVD.RunMinutes, DateTime.Now, addTVD.UserId));
            }

            return Database.NonQuery(sb.ToString().TrimEnd(','), out msg);
        }
    }
}
