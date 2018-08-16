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
            if (AppCurrent.IsRunning && Current.Tester.IsEnable)
            {

                Current.Tester.GetInfo();

            }
        }

        public void CheckMesInfo(object sender, ElapsedEventArgs e)
        {
            if (AppCurrent.IsRunning && Current.Mes.IsEnable)
            {

                MES.Upload();

            }
        }
    }
}
