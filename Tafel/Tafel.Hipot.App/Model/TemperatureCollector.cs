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
            if (!IsGetNewData)
            {
                return;
            }

            if (string.IsNullOrEmpty(ReceiveString))
            {
                return;
            }

            if (ReceiveString.Length < 24)
            {
                TengDa.LogHelper.WriteError("测试仪传输的数据异常：" + ReceiveString);
                return;
            }

            this.Temperature = TengDa._Convert.StrToFloat(ReceiveString.Substring(6, 5), 0);

            this.RealtimeStatus = string.Format("获得数据完成，温度：{0}", Temperature);


            Current.ShowTemperatureData.Add(this.Temperature);
            Current.ShowTemperatureData.RemoveAt(0);

            Current.AnimatedPlot();

            Context.InsulationContext.DataLogs.Add(new InsulationDataLog()
            {
                User = AppCurrent.User,
                TesterId = Current.Tester.Id,
                IsUploaded = false,
                Temperature = this.Temperature,
                DateTime = DateTime.Now
            });
            Context.InsulationContext.SaveChanges();
            IsGetNewData = false;

            AppCurrent.YieldNow.FeedingOK++;

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
