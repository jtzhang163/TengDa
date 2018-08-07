using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using TengDa.Wpf;

namespace Zopoise.Scada.App
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
      if (Current.IsRunning && AppCurrent.Plc.IsEnable)
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
      if (Current.IsRunning && AppCurrent.Communicator.IsEnable && AppCurrent.Testers.Count(t => t.IsEnable) > 0)
      {
        List<Tester> enableTester = AppCurrent.Testers.Where(t => t.IsEnable && t.Id >= cummunicateTesterId).ToList();
        enableTester.AddRange(AppCurrent.Testers.Where(t => t.IsEnable && t.Id < cummunicateTesterId));

        cummunicateTesterId = enableTester.FirstOrDefault().Id;

        string output = string.Empty;
        string msg = string.Empty;
        if (AppCurrent.Communicator.GetInfo(AppCurrent.GetTester(cummunicateTesterId).CommunicateString.Trim(), out output, out msg))
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
            AppCurrent.GetTester(cummunicateTesterId).Currents = currents;
            AppCurrent.GetTester(cummunicateTesterId).Voltage = TengDa._Convert.StrToFloat(outputs[9], 0);

            using (var data = new CurrentVoltageDataContext())
            {
              data.CurrentVoltageDatas.Add(new CurrentVoltageData()
              {
                UserId = Current.User.Id,
                TesterId = AppCurrent.GetTester(cummunicateTesterId).Id,
                Voltage = AppCurrent.GetTester(cummunicateTesterId).Voltage,
                Currents = AppCurrent.GetTester(cummunicateTesterId).Currents,
                CurrentType = AppCurrent.GetTester(cummunicateTesterId).CurrentType
              });
              data.SaveChanges();
            }

          }



          AppCurrent.ShowVoltageData.Add(AppCurrent.GetTester(cummunicateTesterId).Voltage);
          AppCurrent.ShowVoltageData.RemoveAt(0);
          for (int i = 0; i < Tester.CurrentCount; i++)
          {
            AppCurrent.ShowCurrentsData[i].Add(AppCurrent.GetTester(cummunicateTesterId).Currents[i]);
            AppCurrent.ShowCurrentsData[i].RemoveAt(0);
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
