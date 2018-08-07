using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TengDa.Wpf;

namespace Tafel.Hipot.App
{
    public class InsulationData
    {
        public float Resistance { get; set; }
    }

    public class InsulationDataLog : RecordData
    {
        public float Resistance { get; set; }

        public long TesterId { get; set; }

        public long UserId { get; set; }
    }
}
