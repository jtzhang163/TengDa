using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TengDa;

namespace Veken.Baking
{
    /// <summary>
    /// 系统当前运行时公用类
    /// </summary>
    public static class Current
    {
        public static RunStatus runStstus = RunStatus.未知;
        public static List<Oven> ovens = new List<Oven>();
        public static Scaner scaner = new Scaner(1);
        public static MES mes = new MES();
        public static Option option = new Option();
        public static bool IsInOvenFormShow = false;
        public static bool IsOutOvenFormShow = false;
    }
}
