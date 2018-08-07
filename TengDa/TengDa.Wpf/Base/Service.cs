using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TengDa.Wpf
{
    /// <summary>
    /// 基类
    /// </summary>
    public abstract class Service : BindableObject
    {
        [ReadOnly(true), DisplayName("ID"), Category("基本信息")]
        public long Id { get; set; }
    }
}
