using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Tafel.MES
{
    [XmlRoot("response")]
    public class Response
    {
        [XmlElement("table")]
        public TableReturn table { get; set; }
    }

    public class TableReturn
    {
        [XmlElement(ElementName = "rows")]
        public CheckReturn rows { get; set; }
    }

    public class CheckReturn
    {
        [XmlElement("returncode")]
        public string ReturnCode { get; set; }

        [XmlElement("errormsg")]
        public string ErrorMsg { get; set; }
    }
}
