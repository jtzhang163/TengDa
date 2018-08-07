using System.Data.Entity;
using TengDa.Wpf;

namespace Zopoise.Scada.App
{
    public class AppDbInitializer : DbInitializer
    {
        public override void Initialize()
        {
            Database.SetInitializer(new InsulationTesterInitializer());
            base.Initialize();
        }
    }
}
