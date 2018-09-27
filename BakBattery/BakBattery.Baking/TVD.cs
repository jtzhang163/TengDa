using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TengDa;
using TengDa.WF;

namespace BakBattery.Baking
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
            sb.Append("([StationId], [T1], [T2], [T3], [T4], [T5], [T6], [T7], [T8], [V1], [RunMinutes], [Time], [UserId]) ");
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

            StringBuilder sbT = new StringBuilder();
            for (int i = 0; i < Option.TemperaturePointCount; i++)
            {
                sbT.Append(string.Format(", [T{0}]", (i + 1).ToString("D2")));
            }

            StringBuilder sb = new StringBuilder();
            sb.Append(string.Format("INSERT INTO [dbo].[{0}] ", TableName));
            sb.Append("([StationId]" + sbT + ",[V1], [RunMinutes], [Time], [UserId]) VALUES ");

            foreach (TVD addTVD in addTVDs)
            {
                StringBuilder sbT1 = new StringBuilder();
                for (int i = 0; i < Option.TemperaturePointCount; i++)
                {
                    sbT1.Append(string.Format(",{0}", addTVD.T[i]));
                }

                sb.Append(string.Format("({0} {1}, {2}, {3}, '{4}', {5}),", addTVD.FloorId, sbT1, addTVD.V1, addTVD.RunMinutes, DateTime.Now, addTVD.UserId));
            }

            return Database.NonQuery(sb.ToString().TrimEnd(','), out msg);
        }

        public static void Add()
        {
            List<TVD> TVDs = new List<TVD>();

            for (int i = 0; i < Current.ovens.Count; i++)
            {
                if (Current.ovens[i].IsAlive)
                {
                    for (int j = 0; j < Current.ovens[i].Floors.Count; j++)
                    {
                        if (Current.ovens[i].Floors[j].IsBaking)
                        {

                            TVD tvd = new TVD();
                            tvd.FloorId = Current.ovens[i].Floors[j].Id;
                            tvd.UserId = TengDa.WF.Current.user.Id;
                            tvd.RunMinutes = Current.ovens[i].Floors[j].RunMinutes;
                            tvd.T = Current.ovens[i].Floors[j].Temperatures;
                            tvd.V1 = Current.ovens[i].Floors[j].Vacuum;
                            TVDs.Add(tvd);

                        }
                    }
                }
            }
            string msg = string.Empty;
            if (!TVD.Add(TVDs, out msg))
            {
                Error.Alert(msg);
            }
        }
    }
}
