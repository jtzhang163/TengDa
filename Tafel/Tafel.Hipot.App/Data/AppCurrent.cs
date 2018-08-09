using InteractiveDataDisplay.WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using TengDa.Wpf;

namespace Tafel.Hipot.App
{
    public static class AppCurrent
    {
        public static AppViewModel AppViewModel = new AppViewModel();

        public static AppOption Option = new AppOption();

        public static MainWindow MainWindow { get; set; }

        #region 系统设备

        private static InsulationTester insulationTester = new InsulationTester();
        public static InsulationTester InsulationTester
        {
            get
            {

                if (insulationTester.Id < 1)
                {
                    insulationTester = AppContext.InsulationContext.InsulationTesters.FirstOrDefault() ?? new InsulationTester();
                    if (insulationTester.Id < 1)
                    {
                        AppContext.InsulationContext.InsulationTesters.Add(new InsulationTester
                        {
                            Name = "绝缘电阻测试仪",
                            Company = "日置(HIOKI)",
                            Model = "ST5520",
                            Number = "",
                            IsEnable = true,
                            PortName = "COM1",
                            BaudRate = 9600,
                            DataBits = 8,
                            Parity = System.IO.Ports.Parity.None,
                            StopBits = System.IO.Ports.StopBits.One,
                            IsPassiveReceiveSerialPort = true
                        });
                        AppContext.InsulationContext.SaveChanges();
                    }
                }
                return insulationTester;
            }
        }


        private static MES mes = new MES();
        public static MES Mes
        {
            get
            {
                if (mes.Id < 1)
                {
                    mes = AppContext.MesContext.MESs.FirstOrDefault() ?? new MES();
                    if (mes.Id < 1)
                    {
                        AppContext.MesContext.MESs.Add(new MES
                        {
                            Name = "MES",
                            Host = "192.168.1.1",
                            IsEnable = true
                        });
                        AppContext.MesContext.SaveChanges();
                    }
                }
                return mes;
            }
        }

        #endregion

        #region
        public static System.Timers.Timer TimerUpdateTime = new System.Timers.Timer() { Interval = 1000, AutoReset = true };

        public static System.Timers.Timer TimerCheckTesterInfo = new System.Timers.Timer() { Interval = AppCurrent.Option.CheckTesterInfoInterval, AutoReset = true };

        public static System.Timers.Timer TimerCheckMesInfo = new System.Timers.Timer() { Interval = AppCurrent.Option.CheckMesInfoInterval * 1000, AutoReset = true };

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

            if (Current.IsRunning && AppCurrent.AppViewModel.GraphShowMode == GraphShowMode.实时数据)
            {
                Action<MainWindow> updateWindow = new Action<MainWindow>(UpdateWindow);
                MainWindow.Dispatcher.BeginInvoke(updateWindow, MainWindow);
            }
        }

        private static void UpdateWindow(MainWindow window)
        {
            var lgResistance = (LineGraph)(window.linesResistance).Children[0];
            lgResistance.Plot(AppCurrent.ShowDataOrder, AppCurrent.ShowResistanceData);

            var lgVoltage = (LineGraph)(window.linesVoltage).Children[0];
            lgVoltage.Plot(AppCurrent.ShowDataOrder, AppCurrent.ShowVoltageData);

            var lgTemperature = (LineGraph)(window.linesTemperature).Children[0];
            lgTemperature.Plot(AppCurrent.ShowDataOrder, AppCurrent.ShowTemperatureData);

            var lgTimeSpan = (LineGraph)(window.linesTimeSpan).Children[0];
            lgTimeSpan.Plot(AppCurrent.ShowDataOrder, AppCurrent.ShowTimeSpanData);
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
