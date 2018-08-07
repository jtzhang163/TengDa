using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TengDa.WF.Controls
{
    /// <summary>
    /// 双缓冲panel,画曲线用到
    /// </summary>
    public class DoubleBufferPanel : Panel
    {
        public DoubleBufferPanel()
        {
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | //不擦除背景 ,减少闪烁
                 ControlStyles.OptimizedDoubleBuffer | //双缓冲
                 ControlStyles.UserPaint, //使用自定义的重绘事件,减少闪烁
                 true);
        }
    }
}
