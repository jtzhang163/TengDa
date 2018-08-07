using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zopoise.Scada.App
{
  public static class AppContext
  {
    public static CommunicatorContext CommunicatorContext = new CommunicatorContext();
    public static PlcContext PlcContext = new PlcContext();
    public static TesterContext TesterContext = new TesterContext();
  }
}
