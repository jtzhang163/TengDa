﻿using System;
using System.ComponentModel;
using TengDa;

namespace Zopoise.Scada.App
{
    public class UserIDLogViewModel
    {
        [DisplayName("用户名称")]
        public string UserName { get; set; }

        /// <summary>
        /// 电池条码
        /// </summary>
        [DisplayName("电池条码")]
        public string BatteryCode { get; set; }

        /// <summary>
        /// 电阻
        /// </summary>
        [DisplayName("电阻")]
        public float Resistance { get; set; }

        /// <summary>
        /// 电压
        /// </summary>
        [DisplayName("电压")]
        public float Voltage { get; set; }

        /// <summary>
        /// 温度
        /// </summary>
        [DisplayName("温度")]
        public float Temperature { get; set; }

        /// <summary>
        /// 测试时长
        /// </summary>
        [DisplayName("测试时长")]
        public float TimeSpan { get; set; }

        /// <summary>
        /// 已上传
        /// </summary>
        [DisplayName("已上传")]
        public bool IsUploaded { get; set; }

        /// <summary>
        /// 记录时间
        /// </summary>
        [DisplayName("记录时间")]
        public DateTime DateTime { get; set; }
    }
}
