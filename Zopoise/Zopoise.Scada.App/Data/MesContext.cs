using System.Data.Entity;
using TengDa.Wpf;

namespace Zopoise.Scada.App
{

    public class MesContext : DbContext
    {
        public MesContext() : base(AppCurrent.ConnectionString)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MES>().ToTable("t_mes");
        }

        public DbSet<MES> MESs { get; set; }
    }

    public class MesInitializer : DropCreateDatabaseIfModelChanges<MesContext>
    {
        protected override void Seed(MesContext context)
        {
            var mes = new MES
            {
                Name = "MES",
                Host = "192.168.1.1",
                IsEnabled = true
            };
            context.MESs.Add(mes);
        }
    }
}
