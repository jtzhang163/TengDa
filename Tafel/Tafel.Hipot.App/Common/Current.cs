using InteractiveDataDisplay.WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using TengDa.Wpf;

namespace Tafel.Hipot.App
{
    public static class Current
    {
        public static AppViewModel App = new AppViewModel();

        public static Option Option = new Option();

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

        private static TemperatureCollector collector = new TemperatureCollector();
        public static TemperatureCollector Collector
        {
            get
            {
                if (collector.Id < 1)
                {
                    collector = Context.CollectorContext.Collectors.FirstOrDefault() ?? new TemperatureCollector();
                }
                return collector;
            }
        }


        private static Cooler cooler = new Cooler();
        public static Cooler Cooler
        {
            get
            {
                if (cooler.Id < 1)
                {
                    cooler = Context.CoolerContext.Coolers.FirstOrDefault() ?? new Cooler();
                }
                return cooler;
            }
        }

        private static Scaner scaner = new Scaner();
        public static Scaner Scaner
        {
            get
            {
                if (scaner.Id < 1)
                {
                    scaner = Context.ScanerContext.Scaners.FirstOrDefault() ?? new Scaner();
                }
                return scaner;
            }
        }


        private static MES mes = new MES();
        public static MES Mes
        {
            get
            {
                if (mes.Id < 1)
                {
                    mes = Context.MesContext.MESs.FirstOrDefault() ?? new MES();
                }
                return mes;
            }
        }

        #endregion

        #region
        public static System.Timers.Timer TimerUpdateTime = new System.Timers.Timer() { Interval = 1000, AutoReset = true };

        public static System.Timers.Timer TimerCheckTesterInfo = new System.Timers.Timer() { Interval = Option.CheckTesterInfoInterval, AutoReset = true };

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

        private static List<double> showVoltageData = new List<double>();
        /// <summary>
        /// 电压Y轴
        /// </summary>
        public static List<double> ShowVoltageData
        {
            get
            {
                if (showVoltageData.Count < 1)
                {
                    for (int j = 0; j < ShowDataCount; j++)
                    {
                        showVoltageData.Add(0);
                    }
                }
                return showVoltageData;
            }
            set
            {
                showVoltageData = value;
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

        private static List<double> showTimeSpanData = new List<double>();
        /// <summary>
        /// 测试时长Y轴
        /// </summary>
        public static List<double> ShowTimeSpanData
        {
            get
            {
                if (showTimeSpanData.Count < 1)
                {
                    for (int j = 0; j < ShowDataCount; j++)
                    {
                        showTimeSpanData.Add(0);
                    }
                }
                return showTimeSpanData;
            }
            set
            {
                showTimeSpanData = value;
            }
        }


        public static void AnimatedPlot()
        {

            if (AppCurrent.IsRunning && App.GraphShowMode == GraphShowMode.实时数据)
            {
                Action<MainWindow> updateWindow = new Action<MainWindow>(UpdateWindow);
                MainWindow.Dispatcher.BeginInvoke(updateWindow, MainWindow);
            }
        }

        private static void UpdateWindow(MainWindow window)
        {
            var lgResistance = (LineGraph)(window.linesResistance).Children[0];
            lgResistance.Plot(ShowDataOrder, ShowResistanceData);

            var lgVoltage = (LineGraph)(window.linesVoltage).Children[0];
            lgVoltage.Plot(ShowDataOrder, ShowVoltageData);

            var lgTemperature = (LineGraph)(window.linesTemperature).Children[0];
            lgTemperature.Plot(ShowDataOrder, ShowTemperatureData);

            var lgTimeSpan = (LineGraph)(window.linesTimeSpan).Children[0];
            lgTimeSpan.Plot(ShowDataOrder, ShowTimeSpanData);
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
