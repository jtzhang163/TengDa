using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Tafel.MES
{
    public class IP : BaseClass
    {
        [XmlElement("IP_Address")]
        public string IPAddress { get; set; }
    }

    [XmlRoot("response")]
    public class ResponseProcess
    {
        [XmlElement("table")]
        public TableReturnProcess table { get; set; }
    }

    public class TableReturnProcess
    {
        [XmlElement(ElementName = "rows")]
        public ProcessInfo rows { get; set; }
    }

    public class ProcessInfo
    {
        [XmlElement("Process_Code")]
        public string ProcessCode { get; set; }

        [XmlElement("Process_Name")]
        public string ProcessName { get; set; }
    }
}
