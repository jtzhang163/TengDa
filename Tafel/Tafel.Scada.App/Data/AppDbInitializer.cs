using System.Data.Entity;
using TengDa.Wpf;

namespace Tafel.Hipot.App
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
