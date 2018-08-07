using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
