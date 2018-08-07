using System;
using System.Text;

namespace TengDa
{
    /// <summary>
    /// Panasonic PLC 字符串处理
    /// </summary>
    public class PanasonicPLC
    {

        public static string ConvertHexStr(string str, bool revert)
        {
            return ConvertHexStr(str, revert, false);
        }
        /// <summary>
        /// 截取有用的字符串并将小端转化为大端(反转并前后倒置)
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ConvertHexStr(string str, bool revert, bool revertStr)
        {
            if (!string.IsNullOrEmpty(str) && str.Length > 11 && (str.Length - 8) % 4 == 0)
            {
                ///截取前后校验字符
                str = str.Substring(6, str.Length - 8);
                ///反转
                StringBuilder sb = new StringBuilder();
                if (revert)
                {
                    if (revertStr)
                    {
                        for (int i = 0; i < str.Length / 8; i++)
                        {
                            sb.Append(RevertEndian(str.Substring(4 + i * 8, 4)) + RevertEndian(str.Substring(i * 8, 4)));
                        }
                    }
                    else
                    {
                        for (int i = str.Length / 8 - 1; i >= 0; i--)
                        {
                            sb.Append(RevertEndian(str.Substring(4 + i * 8, 4)) + RevertEndian(str.Substring(i * 8, 4)));
                        }
                    }
                }
                else
                {
                    if (revertStr)
                    {
                        for (int i = str.Length / 4 - 1; i >= 0; i--)
                        {
                            sb.Append(RevertEndian(str.Substring(i * 4, 4)));
                        }
                    }
                    else
                    {
                        for (int i = 0; i < str.Length / 4; i++)
                        {
                            sb.Append(RevertEndian(str.Substring(i * 4, 4)));
                        }
                    }

                }

                return sb.ToString();
            }
            return string.Empty;
        }


        public static string Convert2BinStringForAlarm(string str)
        {
            str = ConvertHexStr(str, revert: false);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < str.Length / 4; i++)
            {
                string str2 = str.Substring(i * 4, 4);
                string str3 = HexString2BinString(str2);
                string str4 = Revert(str3);
                sb.Append(str4);
            }
            return sb.ToString();
        }


        /// <summary>
        /// 反转 eg:1234-->3412
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string RevertEndian(string value)
        {
            if (value.Length == 4)
            {
                return value.Substring(2) + value.Substring(0, 2);
            }
            return string.Empty;
        }

        //https://zhidao.baidu.com/question/1512152788280392060.html
        /// <summary>
        /// 16进制字符串转2进制字符串
        /// </summary>
        /// <param name="hexString"></param>
        /// <returns></returns>
        public static string HexString2BinString(string hexString)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in hexString)
            {
                int v = Convert.ToInt32(c.ToString(), 16);
                int v2 = int.Parse(Convert.ToString(v, 2));
                sb.Append(string.Format("{0:d4}", v2));
            }
            return sb.ToString();
        }

        public static string Revert(string str)
        {

            char[] cs = str.ToCharArray();
            Array.Reverse(cs);

            return new string(cs);
        }
    }
}
