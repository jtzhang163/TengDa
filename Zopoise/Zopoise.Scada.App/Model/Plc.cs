using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TengDa.Wpf;

namespace Zopoise.Scada.App
{
  /// <summary>
  /// PLC
  /// </summary>
  [DisplayName("可编程逻辑控制器（PLC）")]
  public class Plc : EthernetTerminal
  {
    public Plc():this(-1)
    {

    }
    public Plc(long id)
    {
      this.Id = Id;
    }
  }

  public class PlcContext : DbContext
  {

    private static string connectionString = ConfigurationManager.ConnectionStrings["DefaultDatabase"].ToString();
    public PlcContext() : base(connectionString)
    {
    }

    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
      modelBuilder.Entity<Plc>().Property(p => p.Name).HasMaxLength(50).IsRequired();
      modelBuilder.Entity<Plc>().Property(p => p.Company).HasMaxLength(50);
      modelBuilder.Entity<Plc>().Property(p => p.Location).HasMaxLength(50);
      modelBuilder.Entity<Plc>().Property(p => p.Number).HasMaxLength(50);
      modelBuilder.Entity<Plc>().Property(p => p.Model).HasMaxLength(50);
    }

    public DbSet<Plc> Plcs { get; set; }
  }

  public class PlcInitializer : DropCreateDatabaseIfModelChanges<PlcContext>
  {
    protected override void Seed(PlcContext context)
    {
      var plc = new Plc
      {
        Name = "PLC",
        Company = "Mitsubishi",
        IsEnable = true,
        Model = "FX5u",
        IP = "127.0.0.1",
        Port = 1000
      };
      context.Plcs.Add(plc);
    }
  }
}
