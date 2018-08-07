using System;
using System.ComponentModel;

namespace TengDa.WF.Products
{
    /// <summary>
    /// 产品
    /// </summary>
    public class Product : Service
    {
        protected string code = string.Empty;
        protected string location = string.Empty;
        protected DateTime scanTime = Common.DefaultTime;

        [Description("条码")]
        [DisplayName("条码")]
        [Category("基本信息")]
        public string Code
        {
            get
            {
                return code;
            }
            set
            {
                if (code != value)
                {
                    UpdateDbField("Code", value);
                }
                code = value;
            }
        }

        [Description("所在位置")]
        [DisplayName("所在位置")]
        [Category("基本信息")]
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


        [Description("扫码时间")]
        [DisplayName("扫码时间")]
        [Category("基本信息")]
        public DateTime ScanTime
        {
            get
            {
                return scanTime;
            }
            set
            {
                if (scanTime != value)
                {
                    UpdateDbField("ScanTime", value);
                }
                scanTime = value;
            }
        }
    }
}
