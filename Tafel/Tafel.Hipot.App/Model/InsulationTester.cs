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

            var tmpStr = ReceiveString.Split(',');
            if (tmpStr.Length < 4)
            {
                return;
            }

            this.Resistance = TengDa._Convert.StrToFloat(tmpStr[0], 0);
            this.Voltage = TengDa._Convert.StrToFloat(tmpStr[1], 0);
            this.TimeSpan = TengDa._Convert.StrToFloat(tmpStr[2], 0);
            this.Temperature = TengDa._Convert.StrToFloat(tmpStr[3], 0);

            this.RealtimeStatus = string.Format("获得数据完成，电阻：{0}，电压：{1}，测试间隔：{2}，温度：{3}", Resistance, Voltage, TimeSpan, Temperature);



            AppCurrent.ShowResistanceData.Add(this.Resistance);
            AppCurrent.ShowResistanceData.RemoveAt(0);

            AppCurrent.ShowVoltageData.Add(this.Voltage);
            AppCurrent.ShowVoltageData.RemoveAt(0);


            AppCurrent.ShowTimeSpanData.Add(this.TimeSpan);
            AppCurrent.ShowTimeSpanData.RemoveAt(0);


            AppCurrent.ShowTemperatureData.Add(this.Temperature);
            AppCurrent.ShowTemperatureData.RemoveAt(0);

            AppCurrent.AnimatedPlot();

            AppContext.InsulationContext.InsulationDataLogs.Add(new InsulationDataLog()
            {
                UserId = Current.User.Id,
                TesterId = AppCurrent.InsulationTester.Id,
                IsUploaded = false,
                Resistance = this.Resistance,
                Temperature = this.Temperature,
                TimeSpan = this.TimeSpan,
                Voltage = this.Voltage,
                RecordTime = DateTime.Now
            });
            AppContext.InsulationContext.SaveChangesAsync();
            IsGetNewData = false;

            Current.RealtimeYieldViewModel.FeedingOK++;

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
            modelBuilder.Entity<InsulationTester>().ToTable("t_insulation_tester");
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
