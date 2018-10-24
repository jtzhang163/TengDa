using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TengDa.WF
{
    /// <summary>
    /// 检测
    /// </summary>
    public static class Check
    {
        /// <summary>
        /// 检测是否登录，未登录弹窗提示
        /// </summary>
        /// <returns></returns>
        public static bool Login()
        {
            var isLogin = Current.IsLogin;
            if (!isLogin)
            {
                Tip.Alert("尚未登录!");
            }
            return isLogin;
        }
    }
}
