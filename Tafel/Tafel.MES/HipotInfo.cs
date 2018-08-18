using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Tafel.MES
{
    public class HipotInfo : BaseClass
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

        [XmlElement("ConfigProcessCode_01")]
        public string BarcodeNo_N { get; set; }

        [XmlElement("ConfigProcessCode_02")]
        public string Resistance_N { get; set; }

        [XmlElement("ConfigProcessCode_03")]
        public string Voltage_N { get; set; }

        [XmlElement("ConfigProcessCode_04")]
        public string Temperature_N { get; set; }

        [XmlElement("ConfigProcessCode_05")]
        public string TestTimeSpan_N { get; set; }

        [XmlElement("ConfigProcessCode_06")]
        public string TestResult_N { get; set; }

        [XmlElement("ConfigProcessCode_07")]
        public string UserNumber_N { get; set; }

        [XmlElement("ConfigProcessCode_08")]
        public string InsertTime_N { get; set; }
    }

}
