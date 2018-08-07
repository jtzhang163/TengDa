using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Configuration;
using System.Data.Entity;
using TengDa;
using TengDa.Wpf;

namespace Zopoise.Scada.App
{
    /// <summary>
    /// 电流电压数据
    /// </summary>
    [DisplayName("电流电压数据")]
    public class CurrentVoltageData : RecordData
    {

        /// <summary>
        /// 测试仪Id
        /// </summary>
        public long TesterId { get; set; }

        public long UserId { get; set; }

        public CurrentVoltageData() : base()
        {
        }

        /// <summary>
        /// 电压
        /// </summary>
        [NotMapped]
        public float Voltage
        {
            get
            {
                return Convert.ToSingle(VoltageString);
            }
            set
            {
                VoltageString = value.ToString("#0.0000");
            }
        }

        public string VoltageString { get; set; }

        /// <summary>
        /// 电流
        /// </summary>
        [NotMapped]
        public float[] Currents
        {
            get
            {
                var currents = new float[Tester.CurrentCount];
                var CurrentsStrings = CurrentsString.Split(',');
                for (int i = 0; i < Tester.CurrentCount && i < CurrentsStrings.Length; i++)
                {
                    currents[i] = Convert.ToSingle(CurrentsStrings[i]);
                }
                return currents;
            }
            set
            {
                CurrentsString = string.Join(",", Array.ConvertAll<float, string>(value, delegate (float f) { return f.ToString("#0.0000"); }));
            }
        }

        public string CurrentsString { get; set; } = "0,0,0,0,0,0";

        /// <summary>
        /// 电流类型
        /// </summary>
        public CurrentType CurrentType { get; set; }
    }

    public class CurrentVoltageDataContext : DbContext
    {
        private static string connectionString = ConfigurationManager.ConnectionStrings["DefaultDatabase"].ToString();
        public CurrentVoltageDataContext() : base(connectionString)
        {
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CurrentVoltageData>().Property(cvd => cvd.RecordTime).HasColumnType("datetime");
        }
        public DbSet<CurrentVoltageData> CurrentVoltageDatas { get; set; }
    }
}
