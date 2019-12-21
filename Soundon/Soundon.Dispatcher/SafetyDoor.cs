using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soundon.Dispatcher
{
    /// <summary>
    /// 安全门
    /// </summary>
    public class SafetyDoor
    {
        /// <summary>
        /// 电箱实际急停
        /// </summary>
        public bool D1105 { get; set; }
        /// <summary>
        /// 3号上料门打开
        /// </summary>
        public bool D1106 { get; set; }
        /// <summary>
        /// 3号下料门打开
        /// </summary>
        public bool D1107 { get; set; }
        /// <summary>
        /// 4号上料门打开
        /// </summary>
        public bool D1108 { get; set; }
        /// <summary>
        /// 4号下料门打开
        /// </summary>
        public bool D1109 { get; set; }
        /// <summary>
        /// 4号小门打开
        /// </summary>
        public bool D1110 { get; set; }
        /// <summary>
        /// 3号上料急停
        /// </summary>
        public bool D1111 { get; set; }
        /// <summary>
        /// 3号下料急停
        /// </summary>
        public bool D1112 { get; set; }
        /// <summary>
        /// 4号上料急停
        /// </summary>
        public bool D1113 { get; set; }
        /// <summary>
        /// 4号下料急停
        /// </summary>
        public bool D1114 { get; set; }
    }
}
