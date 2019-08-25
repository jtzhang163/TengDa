using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAMEL.RGV.Touchscreen.Util
{
    public static class _Convert
    {
        /// <summary>
        /// 字符串反转
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Revert(string str)
        {

            char[] cs = str.ToCharArray();
            Array.Reverse(cs);
            return new string(cs);
        }

        public static string GetBitStr(short val, int totalWidth)
        {
            return Convert.ToString(val, 2).PadLeft(totalWidth, '0');
        }
    }
}
