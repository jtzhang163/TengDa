using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tafel.Hipot.App
{
    public class MES : TengDa.Wpf.Mes
    {
        public MES() : this(-1)
        {

        }
        public MES(long id)
        {
            this.Id = id;
        }
    }

    public class MesContext : DbContext
    {

        private static string connectionString = ConfigurationManager.ConnectionStrings["DefaultDatabase"].ToString();
        public MesContext() : base(connectionString)
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
            };
            context.MESs.Add(mes);
        }
    }
}
