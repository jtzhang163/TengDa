using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace TengDa
{
    public class Net
    {
        /// <summary>
        /// 获取本地电脑IP列表
        /// </summary>
        /// <returns></returns>
        public static List<IPAddress> GetIPList()
        {
            string hostname = Dns.GetHostName();
            IPAddress[] ipadrarray = Dns.GetHostAddresses(hostname);
            List<IPAddress> ipadrslist = new List<IPAddress>();
            foreach (IPAddress ipa in ipadrarray)
            {
                if (ipa.AddressFamily == AddressFamily.InterNetwork)
                    ipadrslist.Add(ipa);
            }
            return ipadrslist;
        }
    }
}
