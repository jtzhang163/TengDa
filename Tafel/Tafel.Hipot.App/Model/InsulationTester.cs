using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Configuration;
using System.Data.Entity;
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
        public InsulationTester(long id)
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
            this.Temperature = 0;

            this.RealtimeStatus = string.Format("获得数据完成，电阻：{0}，电压：{1}，测试间隔：{2}，温度：{3}", Resistance, Voltage, TimeSpan, Temperature);



            Current.ShowResistanceData.Add(this.Resistance);
            Current.ShowResistanceData.RemoveAt(0);

            Current.ShowVoltageData.Add(this.Voltage);
            Current.ShowVoltageData.RemoveAt(0);


            Current.ShowTimeSpanData.Add(this.TimeSpan);
            Current.ShowTimeSpanData.RemoveAt(0);


            Current.ShowTemperatureData.Add(this.Temperature);
            Current.ShowTemperatureData.RemoveAt(0);

            Current.AnimatedPlot();

            Context.InsulationContext.InsulationDataLogs.Add(new InsulationDataLog()
            {
                UserId = TengDa.Wpf.Current.User.Id,
                TesterId = Current.Tester.Id,
                IsUploaded = false,
                Resistance = this.Resistance,
                Temperature = this.Temperature,
                TimeSpan = this.TimeSpan,
                Voltage = this.Voltage,
                DateTime = DateTime.Now
            });
            Context.InsulationContext.SaveChanges();
            IsGetNewData = false;

            TengDa.Wpf.Current.YieldNow.FeedingOK++;

            var t = new Thread(() =>
            {
                Thread.Sleep(1000);
                this.RealtimeStatus = "等待数据";
            });
            t.IsBackground = true;
            t.Start();

        }
    }

    public class InsulationContext : DbContext
    {

        private static string connectionString = ConfigurationManager.ConnectionStrings["DefaultDatabase"].ToString();
        public InsulationContext() : base(connectionString)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<InsulationTester>().ToTable("t_tester");
            modelBuilder.Entity<InsulationDataLog>().ToTable("t_insulation_data_log");
        }

        public DbSet<InsulationTester> InsulationTesters { get; set; }
        public DbSet<InsulationDataLog> InsulationDataLogs { get; set; }
    }

    public class InsulationTesterInitializer : DropCreateDatabaseIfModelChanges<InsulationContext>
    {
        protected override void Seed(InsulationContext context)
        {
            var tester = new InsulationTester
            {
                Name = "绝缘电阻测试仪",
                Company = "Tengda",
                IsPassiveReceiveSerialPort = true
            };
            context.InsulationTesters.Add(tester);
            context.SaveChanges();
        }
    }
}
