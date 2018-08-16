using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TengDa.Wpf;

namespace Tafel.Hipot.App
{

    public class InsulationDataLog : Record
    {
        public float Resistance { get; set; }

        public float Voltage { get; set; }

        public float Temperature { get; set; }

        public float TimeSpan { get; set; }

        public int TesterId { get; set; }

        public InsulationTester Tester { get; set; }

        public bool IsUploaded { get; set; }
    }
}
