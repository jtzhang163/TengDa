﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TengDa;

namespace Anchitech.Baking
{
    /// <summary>
    /// 系统当前运行时公用类
    /// </summary>
    public static class Current
    {
        public static RunStatus runStstus = RunStatus.未知;

        private static TaskMode taskMode = TaskMode.未知;
        public static TaskMode TaskMode
        {
            get
            {
                return taskMode;
            }
            set
            {
                if (taskMode != value)
                {
                    ChangeModeTime = DateTime.Now;
                }
                taskMode = value;
            }
        }

        public static DateTime ChangeModeTime = TengDa.Common.DefaultTime;

        public static List<Oven> ovens = new List<Oven>();

        public static Feeder Feeder = new Feeder(1);

        public static List<Blanker> blankers = new List<Blanker>();

        public static Robot Robot = new Robot(1);

        public static Cacher Cacher = new Cacher(1);

        public static Transfer Transfer = new Transfer(1);

        public static MES mes = MES.Mes;

        public static Option option = new Option();

        public static CurrentTask Task = new CurrentTask(1);

        public static List<Yield> Yields = new List<Yield>();
    }
}