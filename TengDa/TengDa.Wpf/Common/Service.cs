using System.ComponentModel;

namespace TengDa.Wpf
{
    /// <summary>
    /// 基类
    /// </summary>
    public abstract class Service
    {
        [ReadOnly(true), DisplayName("ID"), Category("基本信息")]
        public long Id { get; set; }
    }
}
