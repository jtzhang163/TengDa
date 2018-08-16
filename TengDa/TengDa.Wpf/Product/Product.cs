using System.ComponentModel;

namespace TengDa.Wpf
{
    /// <summary>
    /// 产品类型基类
    /// </summary>
    public abstract class Product : Service
    {
        [DisplayName("条码")]
        [Category("基本设置")]
        public string Code { get; set; }

        [DisplayName("所在位置")]
        [Category("基本设置")]
        public string Location { get; set; }
    }
}
