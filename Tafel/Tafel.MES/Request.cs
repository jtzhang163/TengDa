using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Tafel.MES
{
    [XmlRoot("request")]
    public class Request
    {
        [XmlElement("table")]
        public Table table { get; set; }
    }
}
