using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Tafel.MES
{
    public class TrayInfo : BaseClass
    {

        [XmlElement("BarcodeNo")]
        public string BarcodeNo { get; set; }

        [XmlElement("Process_Code")]
        public string ProcessCode { get; set; }

        [XmlElement("Tray_Code")]
        public string TrayCode { get; set; }

        [XmlElement("Input_Time")]
        public string InputTime { get; set; }

        [XmlElement("Emp_Code")]
        public string UserNumber { get; set; }
    }

}
