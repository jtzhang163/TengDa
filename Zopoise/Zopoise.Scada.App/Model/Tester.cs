using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Configuration;
using System.Data.Entity;
using TengDa;
using TengDa.Wpf;

namespace Zopoise.Scada.App
{
    /// <summary>
    /// 电流电压测试仪
    /// </summary>
    [DisplayName("电流电压测试仪")]
    public class Tester : Terminal
    {
        public const int TesterCount = 200;
        public const int CurrentCount = 6;

        [DisplayName("通信字符串"), Description("发送到串口的字符串")]
        public string CommunicateString { get; set; }

        private float voltage;
        /// <summary>
        /// 电压
        /// </summary>
        [NotMapped]
        [DisplayName("当前电压"), ReadOnly(true)]
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

        /// <summary>
        /// 电流
        /// </summary>
        [Browsable(false), NotMapped]
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

        private string currentsString = "0,0,0,0,0,0";

        [DisplayName("当前电流"), ReadOnly(true), NotMapped]
        public string CurrentsString
        {
            get => currentsString;
            set => SetProperty(ref currentsString, value);
        }

        private CurrentType currentType = CurrentType.AC;
        /// <summary>
        /// 电流类型
        /// </summary>
        [DisplayName("直流/交流"), ReadOnly(true), NotMapped]
        public CurrentType CurrentType
        {
            get => currentType;
            set => SetProperty(ref currentType, value);
        }

    }

    public class TesterContext : DbContext
    {
        private static string connectionString = ConfigurationManager.ConnectionStrings["DefaultDatabase"].ToString();
        public TesterContext() : base(connectionString)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Tester>().ToTable("t_tester");
        }
        public DbSet<Tester> Testers { get; set; }
    }

    public class TesterInitializer : DropCreateDatabaseIfModelChanges<TesterContext>
    {
        protected override void Seed(TesterContext context)
        {
            List<Tester> Testers = new List<Tester>();
            for (var i = 0; i < Tester.TesterCount; i++)
            {
                Testers.Add(new Tester
                {
                    Name = string.Format("工装板{0}", (i + 1).ToString("D3")),
                    Company = "Tengda",
                    CommunicateString = string.Format("{0}-001-{0}-**#", (i + 1).ToString("D3"))
                });
            }
            Testers.ForEach(t => context.Testers.Add(t));
        }
    }
}
