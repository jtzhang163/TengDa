using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Tafel.MES
{
    public class MachineState : BaseClass
    {

        [XmlElement("MachineNo_Code")]
        public string MachineNo { get; set; }

        [XmlElement("State")]
        public State state { get; set; } = State.T;

        [XmlElement("Emp_Code")]
        public string UserNumber { get; set; }
    }
    /// <summary>
    /// 设备状态  S,开始 T,停止 D,待料
    /// </summary>
    public enum State
    {
        S,
        T,
        D
    }
}
