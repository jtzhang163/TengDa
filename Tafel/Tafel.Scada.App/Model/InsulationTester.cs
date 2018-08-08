using System.ComponentModel;
using System.Configuration;
using System.Data.Entity;
using TengDa.Wpf;

namespace Tafel.Hipot.App
{
    /// <summary>
    /// 绝缘电阻测试仪
    /// </summary>
    [DisplayName("绝缘电阻测试仪")]
    public class InsulationTester : SerialTerminal
    {

        public float Resistance { get; set; }

        public float Voltage { get; set; }

        public float Temperature { get; set; }

        public float TimeSpan { get; set; }

        public InsulationTester() : this(-1)
        {

        }
        public InsulationTester(long id)
        {
            this.Id = Id;
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
            };
            context.InsulationTesters.Add(tester);
        }
    }
}
