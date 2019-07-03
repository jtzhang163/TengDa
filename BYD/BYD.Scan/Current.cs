using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TengDa;

namespace BYD.Scan
{
    /// <summary>
    /// 系统当前运行时公用类
    /// </summary>
    public static class Current
    {
        public static RunStatus runStstus = RunStatus.未知;

        public static DateTime ChangeModeTime = TengDa.Common.DefaultTime;

        public static MES mes = MES.Mes;

        public static Option option = new Option();

        public static List<Yield> Yields = new List<Yield>();

        private static List<Line> lines = null;
        public static List<Line> Lines
        {
            get
            {
                if (lines == null)
                {
                    lines = Line.LineList.Where(o => o.ParentId == 0).ToList();
                }
                return lines;
            }
        }
    }
}