﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace TengDa.WF.Terminals
{
    /// <summary>
    /// 终端
    /// </summary>
    public abstract class Terminal : Service
    {

        #region 属性
        protected string name = string.Empty;
        protected string company = string.Empty;
        protected string model = string.Empty;
        protected string number = string.Empty;
        protected string location = string.Empty;
        protected bool isEnable = true;

        /// <summary>
        /// 名称
        /// </summary>
        [Description("名称")]
        [DisplayName("名称")]
        [Category("基本设置")]
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                if (name != value)
                {
                    UpdateDbField("Name", value);
                }
                name = value;
            }
        }

        /// <summary>
        /// 生产厂商
        /// </summary>
        [Description("生产厂商")]
        [DisplayName("生产厂商")]
        [Category("基本设置")]
        public string Company
        {
            get
            {
                return company;
            }
            set
            {
                if (company != value)
                {
                    UpdateDbField("Company", value);
                }
                company = value;
            }
        }

        /// <summary>
        /// 型号
        /// </summary>
        [Description("型号")]
        [DisplayName("型号")]
        [Category("基本设置")]
        public string Model
        {
            get
            {
                return model;
            }
            set
            {
                if (model != value)
                {
                    UpdateDbField("Model", value);
                }
                model = value;
            }
        }

        /// <summary>
        /// 资源号/编号
        /// </summary>
        [Description("资源号/编号")]
        [DisplayName("资源号/编号")]
        [Category("基本设置")]
        public string Number
        {
            get
            {
                return number;
            }
            set
            {
                if (number != value)
                {
                    UpdateDbField("Number", value);
                }
                number = value;
            }
        }

        /// <summary>
        /// 所在位置
        /// </summary>
        [Description("所在位置")]
        [DisplayName("所在位置")]
        [Category("基本设置")]
        public string Location
        {
            get
            {
                return location;
            }
            set
            {
                if (location != value)
                {
                    UpdateDbField("Location", value);
                }
                location = value;
            }
        }

        private string alarmStr = string.Empty;
        /// <summary>
        /// 报警字符串
        /// </summary>
        [Description("报警信息")]
        [DisplayName("报警信息")]
        [ReadOnly(true)]
        public string AlarmStr
        {
            get
            {
                if (!IsAlive)
                {
                    return string.Empty;
                }
                return alarmStr;
            }
            set
            {
                alarmStr = value;
            }
        }

        [Browsable(false)]
        public string PreAlarmStr { get; set; }


        /// <summary>
        /// 是否在线
        /// </summary>
        [ReadOnly(true), Description("是否在线")]
        [DisplayName("是否在线")]
        public bool IsAlive { get; set; } = false;

        public bool PreIsAlive = false;

        /// <summary>
        /// 是否启用
        /// </summary>
        [Description("是否启用")]
        [DisplayName("是否启用")]
        public bool IsEnable
        {
            get
            {
                return isEnable;
            }
            set
            {
                if (isEnable != value)
                {
                    UpdateDbField("IsEnable", value);
                }
                isEnable = value;
            }
        }


        /// <summary>
        /// X轴坐标
        /// </summary>
        [Browsable(false)]
        public int X
        {
            get
            {
                if (string.IsNullOrEmpty(this.Location) || this.Location.Split(',').Length < 1)
                {
                    return 0;
                }
                return TengDa._Convert.StrToInt(this.Location.Split(',')[0], -1);
            }
        }

        /// <summary>
        /// Y轴坐标
        /// </summary>
        [Browsable(false)]
        public int Y
        {
            get
            {
                if (string.IsNullOrEmpty(this.Location) || this.Location.Split(',').Length < 2)
                {
                    return 0;
                }
                return TengDa._Convert.StrToInt(this.Location.Split(',')[1], -1);
            }
        }

        #endregion

        /// <summary>
        /// 计算距离
        /// </summary>
        /// <param name="nextStation">下一站点</param>
        /// <returns></returns>
        public double Distance(Terminal nextTerminal)
        {
            return Math.Sqrt(Math.Pow(nextTerminal.X - this.X, 2) + Math.Pow(nextTerminal.Y - this.Y, 2));
        }

    }
}
