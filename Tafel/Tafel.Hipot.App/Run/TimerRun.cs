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

        public void CheckTesterInfo(object sender, ElapsedEventArgs e)
        {
            if (AppCurrent.IsRunning && Current.Tester.IsEnabled)
            {

                Current.Tester.GetInfo();

            }
        }

        public void CheckCollectorInfo(object sender, ElapsedEventArgs e)
        {
            if (AppCurrent.IsRunning && Current.Collector.IsEnabled)
            {

                Current.Collector.GetInfo();

            }
        }

        public void CheckCoolerInfo(object sender, ElapsedEventArgs e)
        {
            if (AppCurrent.IsRunning && Current.Cooler.IsEnabled)
            {

                Current.Cooler.GetInfo();

            }
        }

        public void CheckScanerInfo(object sender, ElapsedEventArgs e)
        {
            if (AppCurrent.IsRunning && Current.Scaner.IsEnabled && Current.Cooler.IsReadyScan) 
            {

                Current.Scaner.GetInfo();

            }
        }

        public void CheckDataInfo(object sender, ElapsedEventArgs e)
        {
            if (AppCurrent.IsRunning)
            {

                InsulationData.Insert();

            }
        }

        public void CheckMesInfo(object sender, ElapsedEventArgs e)
        {
            if (AppCurrent.IsRunning && Current.Mes.IsEnabled)
            {
                MES.UploadMachineInfo("S");
                MES.Upload();

            }
        }
    }
}
