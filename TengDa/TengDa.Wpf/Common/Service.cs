using System.ComponentModel;

namespace TengDa.Wpf
{
    /// <summary>
    /// 基类
    /// </summary>
    public abstract class Service : BindableObject
    {
        [ReadOnly(true), DisplayName("ID"), Category("基本信息")]
        public int Id { get; set; }
    }
}
