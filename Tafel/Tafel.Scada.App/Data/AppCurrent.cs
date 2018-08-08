using System.Collections.Generic;
using System.Linq;

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
                            Company = "TengDa",
                            IsEnable = true,
                            PortName = "COM1",
                            BaudRate = 9600,
                            DataBits = 8,
                            Parity = System.IO.Ports.Parity.None,
                            StopBits = System.IO.Ports.StopBits.One
                        });
                        AppContext.InsulationContext.SaveChanges();
                    }
                }
                return insulationTester;
            }
        }

        #endregion

        #region
        public static System.Timers.Timer TimerUpdateTime = new System.Timers.Timer() { Interval = 1000, AutoReset = true };
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


        private static List<double>[] showCurrentsData = new List<double>[1];
        /// <summary>
        /// 电流Y轴
        /// </summary>
        public static List<double>[] ShowCurrentsData
        {
            get
            {
                if (showCurrentsData[0] == null)
                {
                    for (int i = 0; i < 1; i++)
                    {
                        showCurrentsData[i] = new List<double>();
                        for (int j = 0; j < ShowDataCount; j++)
                        {
                            showCurrentsData[i].Add(0);
                        }
                    }

                }
                return showCurrentsData;
            }
            set
            {
                showCurrentsData = value;
            }
        }
    }
}
