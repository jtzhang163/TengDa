﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using TengDa;
using TengDa.WF;

namespace CAMEL.Baking
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
        //public float V1 { get; set; }
        public float[] T = new float[Option.TemperaturePointCount];

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
            sb.Append("([FloorId]" + sbT + ", [RunMinutes], [Time], [UserId]) VALUES ");

            foreach (TVD addTVD in addTVDs)
            {
                StringBuilder sbT1 = new StringBuilder();
                for (int i = 0; i < Option.TemperaturePointCount; i++)
                {
                    sbT1.Append(string.Format(",{0}", addTVD.T[i]));
                }

                sb.Append(string.Format("({0} {1}, {2}, '{3}', {4}),", addTVD.FloorId, sbT1, addTVD.RunMinutes, DateTime.Now, addTVD.UserId));
            }

            return Database.NonQuery(sb.ToString().TrimEnd(','), 60, out msg);
        }

        public static void Save()
        {
            List<TVD> tvds = new List<TVD>();

            for (int i = 0; i < Current.ovens.Count; i++)
            {
                if (Current.ovens[i].IsAlive)
                {
                    for (int j = 0; j < Current.ovens[i].Floors.Count; j++)
                    {
                        if (Current.ovens[i].Floors[j].IsBaking)
                        {
                            //for(int k = 0;k< Current.ovens[i].Floors[j].Stations.Count; k++)
                            //{
                            TVD tvd = new TVD();
                            tvd.FloorId = Current.ovens[i].Floors[j].Id;
                            tvd.UserId = TengDa.WF.Current.user.Id;
                            tvd.RunMinutes = Current.ovens[i].Floors[j].RunMinutes;
                            tvd.T = Current.ovens[i].Floors[j].Temperatures;
                            tvds.Add(tvd);
                            //}
                        }
                    }
                }
            }
            string msg = string.Empty;
            if (!TVD.Add(tvds, out msg))
            {
                Error.Alert(msg);
            }
        }

        public static int GetCount(out string msg)
        {
            DataTable dt = Database.Query(string.Format("SELECT COUNT(*) FROM [dbo].[{0}];", TableName), out msg);
            if (dt.Rows.Count > 0)
            {
                return TengDa._Convert.StrToInt(dt.Rows[0][0].ToString(), -1);
            }
            return 0;
        }

        /// <summary>
        /// 删除很久之前数据库中的温度数据，保留最近的100000条数据
        /// </summary>
        public static bool DeleteLongAgo(out string msg)
        {
            return Database.NonQuery(string.Format("DELETE FROM dbo.[{0}] WHERE Id <= ((SELECT MAX(Id) from dbo.[{0}]) - 100000)", TableName), 60, out msg);
        }
    }
}
