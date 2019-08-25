using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAMEL.RGV.Touchscreen
{
    public class Option : BindableObject
    {
        private string rgv1_ip;
        public string RGV1_IP
        {
            get
            {
                if (string.IsNullOrEmpty(rgv1_ip))
                {
                    rgv1_ip = ConfigurationManager.AppSettings["rgv1_ip"].ToString();
                }
                return rgv1_ip;
            }
        }


        private string rgv2_ip;
        public string RGV2_IP
        {
            get
            {
                if (string.IsNullOrEmpty(rgv2_ip))
                {
                    rgv2_ip = ConfigurationManager.AppSettings["rgv2_ip"].ToString();
                }
                return rgv2_ip;
            }
        }

        private bool? isPad;
        /// <summary>
        /// 程序是否运行在平板
        /// </summary>
        public bool IsPad
        {
            get
            {
                if (isPad == null)
                {
                    isPad = bool.Parse(ConfigurationManager.AppSettings["IsPad"]);
                }
                return isPad.Value;
            }
        }

        private string username;
        public string Username
        {
            get
            {
                return username;
            }
            set
            {
                this.SetProperty(ref username, value);
            }
        }


        private string currentTime;
        public string CurrentTime
        {
            get
            {
                return currentTime;
            }
            set
            {
                this.SetProperty(ref currentTime, value);
            }
        }

    }
}
