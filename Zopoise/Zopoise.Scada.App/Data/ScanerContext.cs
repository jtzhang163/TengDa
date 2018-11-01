using System.Data.Entity;
using TengDa.Wpf;

namespace Zopoise.Scada.App
{
    public class ScanerContext : DbContext
    {
        public ScanerContext() : base(AppCurrent.ConnectionString)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Scaner>().ToTable("t_scaner");
        }

        public DbSet<Scaner> Scaners { get; set; }
    }

    public class ScanerInitializer : DropCreateDatabaseIfModelChanges<ScanerContext>
    {
        protected override void Seed(ScanerContext context)
        {
            var scaner = new Scaner
            {
                Name = "扫码枪",
                Company = "Datalogic",
                Model = "Matrix 120",
                IP = "192.168.1.100",
                Port = 3000
            };
            context.Scaners.Add(scaner);
            context.SaveChanges();
        }
    }
}
