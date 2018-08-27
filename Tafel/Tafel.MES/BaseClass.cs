using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Tafel.MES
{
    [XmlInclude(typeof(MESUser))]
    [XmlInclude(typeof(Sfc))]
    [XmlInclude(typeof(MachineState))]
    [XmlInclude(typeof(HipotInfo))]
    [XmlInclude(typeof(TrayInfo))]
    [XmlInclude(typeof(IP))]
    [XmlInclude(typeof(IpAndProcess))]
    public class BaseClass
    {

    }
}
