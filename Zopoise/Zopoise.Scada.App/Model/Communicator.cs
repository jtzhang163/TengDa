using System.ComponentModel;
using System.Configuration;
using System.Data.Entity;
using TengDa.Wpf;

namespace Zopoise.Scada.App
{
    /// <summary>
    /// 数据采集器
    /// 通过该设备获得全部测试仪的数据
    /// </summary>
    [DisplayName("数据采集器")]
    public class Communicator : SerialTerminal
    {
        public Communicator() : this(-1)
        {

        }
        public Communicator(long id)
        {
            this.Id = Id;
        }
    }

    public class CommunicatorContext : DbContext
    {

        private static string connectionString = ConfigurationManager.ConnectionStrings["DefaultDatabase"].ToString();
        public CommunicatorContext() : base(connectionString)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Communicator>().ToTable("t_communicator");
        }

        public DbSet<Communicator> Communicators { get; set; }
    }

    public class CommunicatorInitializer : DropCreateDatabaseIfModelChanges<CommunicatorContext>
    {
        protected override void Seed(CommunicatorContext context)
        {
            var communicator = new Communicator
            {
                Name = "数据采集器",
                Company = "Tengda",
            };
            context.Communicators.Add(communicator);
        }
    }
}
