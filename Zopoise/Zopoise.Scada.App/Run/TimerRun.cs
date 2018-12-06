using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using TengDa.Wpf;

namespace Zopoise.Scada.App
{
    /// <summary>
    /// 定时器执行
    /// </summary>
    public class TimerRun
    {

        public void CheckTesterInfo(object sender, ElapsedEventArgs e)
        {
            if (AppCurrent.IsRunning && Current.Tester.IsEnabled)
            {

                Current.Tester.GetInfo();

            }
        }

        public void CheckCollectorInfo(object sender, ElapsedEventArgs e)
        {
            if (AppCurrent.IsRunning && Current.Collector.IsEnabled && Current.Tester.CollectorIsReadyCollect)
            {

                Current.Collector.GetInfo();

            }
        }

        public void CheckControllerInfo(object sender, ElapsedEventArgs e)
        {
            if (AppCurrent.IsRunning && Current.Controller.IsEnabled)
            {

                Current.Controller.GetInfo();

            }
        }

        public void CheckDataInfo(object sender, ElapsedEventArgs e)
        {
            if (AppCurrent.IsRunning)
            {

                InsulationData.Insert();

            }
        }
    }
}
