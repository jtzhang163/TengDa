using System.Collections.Generic;
using System.Linq;

namespace Zopoise.Scada.Bak
{
    public static class Current
    {
        public static AppViewModel App = new AppViewModel();

        public static AppOption Option = new AppOption();

        #region 系统设备

        private static List<Tester> testers = new List<Tester>();
        public static List<Tester> Testers
        {
            get
            {
                if (testers.Count < 1)
                {
                    testers = AppContext.TesterContext.Testers.ToList();
                    if (testers.Count < 1)
                    {
                        for (var i = 0; i < Tester.TesterCount; i++)
                        {
                            testers.Add(new Tester
                            {
                                Name = string.Format("工装板{0}", (i + 1).ToString("D3")),
                                Company = "Tengda",
                                CommunicateString = string.Format("{0}-001-{0}-**-#", (i + 1).ToString("D3"))
                            });
                        }
                        testers.ForEach(t => AppContext.TesterContext.Testers.Add(t));
                        AppContext.TesterContext.SaveChanges();
                    }
                }
                return testers;
            }
        }

        private static Plc plc = new Plc();
        public static Plc Plc
        {
            get
            {

                if (plc.Id < 1)
                {
                    plc = AppContext.PlcContext.Plcs.FirstOrDefault() ?? new Plc();
                    if (plc.Id < 1)
                    {
                        AppContext.PlcContext.Plcs.Add(new Plc
                        {
                            Name = "PLC",
                            Company = "Mitsubishi",
                            IsEnabled = true,
                            Model = "FX5u",
                            IP = "127.0.0.1",
                            Port = 1000
                        });
                        AppContext.PlcContext.SaveChanges();
                    }
                }
                return plc;
            }

        }

        private static Communicator communicator = new Communicator();
        public static Communicator Communicator
        {
            get
            {

                if (communicator.Id < 1)
                {
                    communicator = AppContext.CommunicatorContext.Communicators.FirstOrDefault() ?? new Communicator();
                    if (communicator.Id < 1)
                    {
                        AppContext.CommunicatorContext.Communicators.Add(new Communicator
                        {
                            Name = "通信器",
                            Company = "TengDa",
                            IsEnabled = true,
                            PortName = "COM1",
                            BaudRate = 9600,
                            DataBits = 8,
                            Parity = System.IO.Ports.Parity.None,
                            StopBits = System.IO.Ports.StopBits.One
                        });
                        AppContext.CommunicatorContext.SaveChanges();
                    }
                }
                return communicator;
            }
        }

        #endregion

        #region
        public static System.Timers.Timer TimerCommunicateWithPlc = new System.Timers.Timer() { Interval = Current.Option.PlcCommunicateInterval, AutoReset = true };
        public static System.Timers.Timer TimerCommunicateWithCommunicator = new System.Timers.Timer() { Interval = Current.Option.CommunicatorCommunicateInterval, AutoReset = true };
        public static System.Timers.Timer TimerUpdateTime = new System.Timers.Timer() { Interval = 1000, AutoReset = true };
        #endregion

        public static Tester GetTester(long id)
        {
            return Testers.FirstOrDefault(t => t.Id == id) ?? new Tester();
        }

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


        private static List<double>[] showCurrentsData = new List<double>[Tester.CurrentCount];
        /// <summary>
        /// 电流Y轴
        /// </summary>
        public static List<double>[] ShowCurrentsData
        {
            get
            {
                if (showCurrentsData[0] == null)
                {
                    for (int i = 0; i < Tester.CurrentCount; i++)
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
