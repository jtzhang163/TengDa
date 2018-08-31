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

        /// <summary>
        /// 采集器准备好准备好采集数据
        /// </summary>
        public bool CollectorIsReadyCollect = false;

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
            this.Voltage = Current.Option.ConstVoltage;
            this.TimeSpan = Current.Option.ConstTimeSpan;

            if (this.Resistance == 0)
            {
                this.Resistance = 999;
            }

            InsulationData.Resistance = this.Resistance;
            InsulationData.Voltage = this.Voltage;
            InsulationData.TimeSpan = this.TimeSpan;

            this.RealtimeStatus = string.Format("获得电阻：{0}", Resistance);

            this.CollectorIsReadyCollect = true;


            Current.ShowResistanceData.Add(this.Resistance);
            Current.ShowResistanceData.RemoveAt(0);

            Current.AnimatedPlot();

            IsGetNewData = false;

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
