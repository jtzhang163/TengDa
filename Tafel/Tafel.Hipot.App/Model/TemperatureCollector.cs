using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading;
using TengDa.Wpf;

namespace Tafel.Hipot.App
{
    /// <summary>
    /// 温度采集器
    /// </summary>
    [DisplayName("温度采集器")]
    public class TemperatureCollector : SerialTerminal
    {
        private float temperature;
        [NotMapped, Browsable(false)]
        public float Temperature
        {
            get
            {
                return temperature;
            }
            set
            {
                SetProperty(ref temperature, value);
            }
        }


        public TemperatureCollector() : this(-1)
        {


        }
        public TemperatureCollector(int id)
        {
            this.Id = Id;
        }


        public void GetInfo()
        {

            var output = new byte[] { };

            string msg;
            //string input = "01 03 00 00 00 10 44 06";
            var input = new byte[] { 1, 3, 0, 0, 0, 16, 68, 6 };

            if (!this.GetInfo(input, out output, out msg))
            {
                // AppCurrent.Tip 
                OperationHelper.ShowTips(msg);
                return;
            }


            Current.Tester.CollectorIsReadyCollect = false;

            if (output.Length > 4)
            {
                this.Temperature = (output[3] * 256 + output[4]) / 755 * 25.7f;
            }
            else
            {
                this.Temperature = 25;
            }
                       

            InsulationData.Temperature = this.Temperature;

            this.RealtimeStatus = string.Format("获得温度：{0}", Temperature);


            Current.ShowTemperatureData.Add(this.Temperature);
            Current.ShowTemperatureData.RemoveAt(0);

            Current.AnimatedPlot();

            var t = new Thread(() =>
            {
                Thread.Sleep(1000);
                this.RealtimeStatus = "等待数据";
            });
            t.IsBackground = true;
            t.Start();

        }
    }
}
