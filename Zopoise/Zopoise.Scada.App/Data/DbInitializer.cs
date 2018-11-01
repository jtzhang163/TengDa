using System.Data.Entity;
using TengDa.Wpf;

namespace Zopoise.Scada.App
{
    public class DbInitializer : TengDa.Wpf.DbInitializer
    {
        public override void Initialize()
        {
            Database.SetInitializer(new MesInitializer());
            Database.SetInitializer(new CollectorInitializer());
            Database.SetInitializer(new TesterInitializer());
            Database.SetInitializer(new ScanerInitializer());
            Database.SetInitializer(new CoolerInitializer());
            base.Initialize();
        }
    }
}
