using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Tafel.MES
{
    public class IpAndProcess : BaseClass
    {
        [XmlElement("IP_Address")]
        public string IPAddress { get; set; }

        [XmlElement("Process_Code")]
        public string ProcessCode { get; set; }
    }

    [XmlRoot("response")]
    public class ResponseStation
    {
        [XmlElement("table")]
        public TableReturnStation table { get; set; }
    }

    public class TableReturnStation
    {
        [XmlElement(ElementName = "rows")]
        public StationInfo rows { get; set; }
    }

    public class StationInfo
    {
        [XmlElement("Station_Code")]
        public string StationCode { get; set; }

        [XmlElement("Station_Name")]
        public string StationName { get; set; }
    }
}
