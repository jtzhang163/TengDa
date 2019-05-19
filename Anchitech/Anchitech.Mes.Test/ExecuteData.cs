using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anchitech.Mes.Test
{
    public class ExecuteData
    {
        public string RESOURCE { get; set; }
        public string ACTION { get; set; }
        public string CONTAINER_ID { get; set; }
        public string IS_PROCESS_LOT { get; set; }
        public SfcData[] SFC_LIST { get; set; }
        public SfcData[] NC_SFC_LIST { get; set; }
        public string SFC { get; set; }
        public MachStatus[] RESOURCE_LIST { get; set; }
    }

    public class SfcData
    {
        public string SFC { get; set; }
    }

    public class MachStatus
    {
        public string RESOURCE { get; set; }
        public string STATUS { get; set; }
    }
}
