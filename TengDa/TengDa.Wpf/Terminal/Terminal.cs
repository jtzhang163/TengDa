using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TengDa.Wpf
{
    public abstract class Terminal : Service
    {
        /// <summary>
        /// 名称
        /// </summary>
        [DisplayName("名称"), Category("基本信息")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 生产厂商
        /// </summary>
        [DisplayName("生产厂商"), Category("基本信息")]
        public string Company { get; set; } = string.Empty;

        /// <summary>
        /// 型号
        /// </summary>
        [DisplayName("型号"), Category("基本信息")]
        public string Model { get; set; } = string.Empty;

        /// <summary>
        /// 资源号/编号
        /// </summary>
        [DisplayName("资源号/编号"), Category("基本信息")]
        public string Number { get; set; } = string.Empty;

        /// <summary>
        /// 所在位置
        /// </summary>
        [DisplayName("所在位置"), Category("基本信息")]
        public string Location { get; set; } = string.Empty;

        /// <summary>
        /// 报警字符串
        /// </summary>
        public string AlarmStr = string.Empty;


        /// <summary>
        /// 是否在线
        /// </summary>
        public bool IsAlive = false;

        /// <summary>
        /// 是否启用
        /// </summary>
        [DisplayName("是否启用"), Category("常见设置"), ReadOnly(true)]
        public bool IsEnable { get; set; } = true;


        /// <summary>
        /// X轴坐标
        /// </summary>
        [DisplayName("X轴坐标"), Category("位置坐标")]
        public float X { get; set; }

        /// <summary>
        /// Y轴坐标
        /// </summary>
        [DisplayName("Y轴坐标"), Category("位置坐标")]
        public float Y { get; set; }

        /// <summary>
        /// Z轴坐标
        /// </summary>
        [DisplayName("Z轴坐标"), Category("位置坐标")]
        public float Z { get; set; }

        /// <summary>
        /// 计算距离
        /// </summary>
        /// <param name="nextStation">下一站点</param>
        /// <returns></returns>
        public double Distance(Terminal nextTerminal)
        {
            return Math.Sqrt(Math.Pow(nextTerminal.X - this.X, 2) + Math.Pow(nextTerminal.Y - this.Y, 2) + Math.Pow(nextTerminal.Z - this.Z, 2));
        }
    }
}
