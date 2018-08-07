using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Tafel.MES
{
    public class MESUser : BaseClass
    {
        [XmlElement("Emp_Code")]
        public string UserNumber { get; set; }

        [XmlElement("Emp_PWD")]
        public string UserPwd { get; set; }

        [XmlElement("Process_Code")]
        public string ProcessCode { get; set; }

        [XmlElement("Station_Code")]
        public string StationCode { get; set; }

        [XmlElement("MachineNo")]
        public string MachineNo { get; set; }
    }
}
