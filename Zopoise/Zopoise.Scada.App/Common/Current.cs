using InteractiveDataDisplay.WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using TengDa.Wpf;

namespace Zopoise.Scada.App
{
    public static class Current
    {
        public static AppViewModel App = new AppViewModel();

        public static Option Option = new Option();

        public static MainTabItemUC MainTabItemUC { get; set; }

        public static MainWindow MainWindow { get; set; }

        #region 系统设备

        private static InsulationTester tester = new InsulationTester();
        public static InsulationTester Tester
        {
            get
            {
                if (tester.Id < 1)
                {
                    tester = Context.InsulationContext.Testers.FirstOrDefault() ?? new InsulationTester();
                }
                return tester;
            }
        }


        private static Controller controller = new Controller();
        public static Controller Controller
        {
            get
            {
                if (controller.Id < 1)
                {
                    controller = Context.ControllerContext.Controllers.FirstOrDefault() ?? new Controller();
                }
                return controller;
            }
        }

        #endregion


        #region 定时器

        public static System.Timers.Timer TimerUpdateTime = new System.Timers.Timer() { Interval = 1000, AutoReset = true };

        public static System.Timers.Timer TimerCheckTesterInfo = new System.Timers.Timer() { Interval = Option.CheckTesterInfoInterval, AutoReset = true };

        public static System.Timers.Timer TimerCheckCollectorInfo = new System.Timers.Timer() { Interval = Option.CheckCollectorInfoInterval, AutoReset = true };

        public static System.Timers.Timer TimerCheckControllerInfo = new System.Timers.Timer() { Interval = Option.CheckControllerInfoInterval, AutoReset = true };

        public static System.Timers.Timer TimerCheckScanerInfo = new System.Timers.Timer() { Interval = Option.CheckScanerInfoInterval, AutoReset = true };

        public static System.Timers.Timer TimerCheckDataInfo = new System.Timers.Timer() { Interval = 100, AutoReset = true };

        public static System.Timers.Timer TimerCheckMesInfo = new System.Timers.Timer() { Interval = Option.CheckMesInfoInterval * 1000, AutoReset = true };

        #endregion

        public const int ShowDataCount = 100;

        private static List<double> showdataOrder = new List<double>();
        /// <summary>
        /// 图像X轴
        /// </summary>
        public static List<double> ShowDataOrder
        {
            get
            {
                if (showdataOrder.Count < ShowDataCount)
                {
                    for (int i = 0; i < ShowDataCount; i++)
                    {
                        showdataOrder.Add(i + 1);
                    }
                }
                return showdataOrder;
            }
        }

        private static List<double> showResistanceData = new List<double>();
        /// <summary>
        /// 电阻Y轴
        /// </summary>
        public static List<double> ShowResistanceData
        {
            get
            {
                if (showResistanceData.Count < 1)
                {
                    for (int j = 0; j < ShowDataCount; j++)
                    {
                        showResistanceData.Add(0);
                    }
                }
                return showResistanceData;
            }
            set
            {
                showResistanceData = value;
            }
        }

        private static List<double> showTemperatureData = new List<double>();
        /// <summary>
        /// 温度Y轴
        /// </summary>
        public static List<double> ShowTemperatureData
        {
            get
            {
                if (showTemperatureData.Count < 1)
                {
                    for (int j = 0; j < ShowDataCount; j++)
                    {
                        showTemperatureData.Add(0);
                    }
                }
                return showTemperatureData;
            }
            set
            {
                showTemperatureData = value;
            }
        }


        public static void AnimatedPlot()
        {

            if (AppCurrent.IsRunning && App.GraphShowMode == GraphShowMode.实时数据)
            {
                Action<MainTabItemUC> update = new Action<MainTabItemUC>(UpdateWindow);
                MainTabItemUC.Dispatcher.BeginInvoke(update, MainTabItemUC);
            }
        }

        private static void UpdateWindow(MainTabItemUC uc)
        {
            var lgResistance = (LineGraph)(uc.linesResistance).Children[0];
            lgResistance.Plot(ShowDataOrder, ShowResistanceData);

            var lgTemperature = (LineGraph)(uc.linesTemperature).Children[0];
            lgTemperature.Plot(ShowDataOrder, ShowTemperatureData);

        }

        //private static List<double>[] showCurrentsData = new List<double>[1];
        ///// <summary>
        ///// 电流Y轴
        ///// </summary>
        //public static List<double>[] ShowCurrentsData
        //{
        //    get
        //    {
        //        if (showCurrentsData[0] == null)
        //        {
        //            for (int i = 0; i < 1; i++)
        //            {
        //                showCurrentsData[i] = new List<double>();
        //                for (int j = 0; j < ShowDataCount; j++)
        //                {
        //                    showCurrentsData[i].Add(0);
        //                }
        //            }

        //        }
        //        return showCurrentsData;
        //    }
        //    set
        //    {
        //        showCurrentsData = value;
        //    }
        //}
    }
}
