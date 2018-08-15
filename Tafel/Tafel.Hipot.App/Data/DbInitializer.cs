using System.Data.Entity;
using TengDa.Wpf;

namespace Tafel.Hipot.App
{
    public class DbInitializer : TengDa.Wpf.DbInitializer
    {
        public override void Initialize()
        {
            Database.SetInitializer(new InsulationTesterInitializer());
            base.Initialize();
        }
    }
}
