using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TengDa.Wpf
{
    public abstract class Product : Service
    {
        [Description("条码")]
        [DisplayName("条码")]
        [Category("基本设置")]
        public string Code { get; set; }

        [Description("所在位置")]
        [DisplayName("所在位置")]
        [Category("基本设置")]
        public string Location { get; set; }
    }
}
