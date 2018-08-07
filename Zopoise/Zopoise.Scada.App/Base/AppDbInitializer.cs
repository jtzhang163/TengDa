using System.Data.Entity;
using TengDa.Wpf;

namespace Zopoise.Scada.App
{
    public class AppDbInitializer : DbInitializer
    {
        public override void Initialize()
        {
            Database.SetInitializer(new TesterInitializer());
            Database.SetInitializer(new CommunicatorInitializer());
            Database.SetInitializer(new PlcInitializer());
            base.Initialize();
        }
    }
}
