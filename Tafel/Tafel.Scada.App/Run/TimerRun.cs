using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using TengDa.Wpf;

namespace Tafel.Hipot.App
{
    /// <summary>
    /// 定时器执行
    /// </summary>
    public class TimerRun
    {

        public void InsulationTesterCommunicate(object sender, ElapsedEventArgs e)
        {
            if (Current.IsRunning && AppCurrent.InsulationTester.IsEnable)
            {
                Console.WriteLine("Plc Communicate");
            }
        }
    }
}
