using System.Data.Entity;
using TengDa.Wpf;

namespace Tafel.Hipot.App
{

    public class BatteryContext : DbContext
    {
        public BatteryContext() : base(AppCurrent.ConnectionString)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Battery>().ToTable("t_battery");
        }

        public DbSet<Battery> Batteries { get; set; }
    }

}
