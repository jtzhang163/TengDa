using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading;
using TengDa.Wpf;

namespace Tafel.Hipot.App
{
    /// <summary>
    /// 绝缘电阻测试仪
    /// </summary>
    [DisplayName("绝缘电阻测试仪")]
    public class InsulationTester : SerialTerminal
    {
        private float resistance;
        [NotMapped,Browsable(false)]
        public float Resistance
        {
            get
            {
                return resistance;
            }
            set
            {
                SetProperty(ref resistance, value);
            }
        }

        private float voltage;
        [NotMapped, Browsable(false)]
        public float Voltage
        {
            get
            {
                return voltage;
            }
            set
            {
                SetProperty(ref voltage, value);
            }
        }

        private float timeSpan;
        [NotMapped, Browsable(false)]
        public float TimeSpan
        {
            get
            {
                return timeSpan;
            }
            set
            {
                SetProperty(ref timeSpan, value);
            }
        }

        public InsulationTester() : this(-1)
        {

        }
        public InsulationTester(int id)
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

            this.Resistance = TengDa._Convert.StrToFloat(ReceiveString.Substring(6, 5), 0);
            this.Voltage = 100;
            this.TimeSpan = 5;

            this.RealtimeStatus = string.Format("获得数据完成，电阻：{0}，电压：{1}，测试间隔：{2}", Resistance, Voltage, TimeSpan);



            Current.ShowResistanceData.Add(this.Resistance);
            Current.ShowResistanceData.RemoveAt(0);

            Current.ShowVoltageData.Add(this.Voltage);
            Current.ShowVoltageData.RemoveAt(0);


            Current.ShowTimeSpanData.Add(this.TimeSpan);
            Current.ShowTimeSpanData.RemoveAt(0);


            Current.AnimatedPlot();

            Context.InsulationContext.DataLogs.Add(new InsulationDataLog()
            {
                User = AppCurrent.User,
                TesterId = Current.Tester.Id,
                IsUploaded = false,
                Resistance = this.Resistance,
                TimeSpan = this.TimeSpan,
                Voltage = this.Voltage,
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
