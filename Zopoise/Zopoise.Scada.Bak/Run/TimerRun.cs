using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using TengDa.Wpf;

namespace Zopoise.Scada.Bak
{
    /// <summary>
    /// 定时器执行
    /// </summary>
    public class TimerRun
    {
        /// <summary>
        /// PLC定时通信执行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void PlcCommunicate(object sender, ElapsedEventArgs e)
        {
            if (AppCurrent.IsRunning && Current.Plc.IsEnabled)
            {
                Console.WriteLine("Plc Communicate");
            }
        }

        /// <summary>
        /// 通信器定时通信执行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void CommunicatorCommunicate(object sender, ElapsedEventArgs e)
        {
            if (AppCurrent.IsRunning && Current.Communicator.IsEnabled && Current.Testers.Count(t => t.IsEnabled) > 0)
            {
                List<Tester> enableTester = Current.Testers.Where(t => t.IsEnabled && t.Id >= cummunicateTesterId).ToList();
                enableTester.AddRange(Current.Testers.Where(t => t.IsEnabled && t.Id < cummunicateTesterId));

                cummunicateTesterId = enableTester.FirstOrDefault().Id;

                string output = string.Empty;
                string msg = string.Empty;
                if (Current.Communicator.GetInfo(Current.GetTester(cummunicateTesterId).CommunicateString.Trim(), out output, out msg))
                {
                    Console.WriteLine("OK + " + output);
                    var outputs = output.Split('-');
                    if (outputs.Length > 10)
                    {
                        var currents = new float[Tester.CurrentCount];
                        for (int i = 0; i < Tester.CurrentCount; i++)
                        {
                            currents[i] = TengDa._Convert.StrToFloat(outputs[i + 3], 0);
                        }
                        Current.GetTester(cummunicateTesterId).Currents = currents;
                        Current.GetTester(cummunicateTesterId).Voltage = TengDa._Convert.StrToFloat(outputs[9], 0);

                        using (var data = new CurrentVoltageDataContext())
                        {
                            data.CurrentVoltageDatas.Add(new CurrentVoltageData()
                            {
                                UserId = AppCurrent.User.Id,
                                TesterId = Current.GetTester(cummunicateTesterId).Id,
                                Voltage = Current.GetTester(cummunicateTesterId).Voltage,
                                Currents = Current.GetTester(cummunicateTesterId).Currents,
                                CurrentType = Current.GetTester(cummunicateTesterId).CurrentType
                            });
                            data.SaveChanges();
                        }

                    }



                    Current.ShowVoltageData.Add(Current.GetTester(cummunicateTesterId).Voltage);
                    Current.ShowVoltageData.RemoveAt(0);
                    for (int i = 0; i < Tester.CurrentCount; i++)
                    {
                        Current.ShowCurrentsData[i].Add(Current.GetTester(cummunicateTesterId).Currents[i]);
                        Current.ShowCurrentsData[i].RemoveAt(0);
                    }

                }
                else
                {
                    Console.WriteLine("NG + " + msg);
                }
                cummunicateTesterId++;

            }
        }

        public long cummunicateTesterId = -1;
    }
}
