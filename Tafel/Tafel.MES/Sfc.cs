using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Tafel.MES
{
    public class Sfc : BaseClass
    {
        [XmlElement("BarcodeNo")]
        public string BarcodeNo { get; set; }

        [XmlElement("Process_Code")]
        public string ProcessCode { get; set; }

        [XmlElement("Station_Code")]
        public string StationCode { get; set; }

        [XmlElement("Order_No")]
        public string OrderNo { get; set; }

        [XmlElement("IP_Address")]
        public string IPAddress { get; set; }

        [XmlElement("Material_OrderNo")]
        public string MaterialOrderNo { get; set; }

        [XmlElement("MachineNo")]
        public string MachineNo { get; set; }

        [XmlElement("Emp_Code")]
        public string UserNumber { get; set; }
    }
}
