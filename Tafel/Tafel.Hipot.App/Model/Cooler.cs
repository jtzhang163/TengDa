using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TengDa.Wpf;

namespace Tafel.Hipot.App
{
    /// <summary>
    /// 冷却机
    /// </summary>
    [DisplayName("冷却机")]
    public class Cooler : Terminal
    {
        public int PlcId { get; set; }

        public virtual PLC PLC { get; set; }
    }
}
