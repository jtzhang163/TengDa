using System;
using System.Security.Cryptography;
using System.Text;

namespace TengDa.Encrypt
{
    public class MD5
    {
        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="inputValue"></param>
        /// <returns></returns>
        public static string MD5Encrypt(string inputValue)
        {
            var result = "";
            using (var md5 = System.Security.Cryptography.MD5.Create())
            {
                var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(inputValue));
                var strResult = BitConverter.ToString(hash).ToLower();
                result = strResult.Replace("-", "");
            }
            return result;
        }
    }
}
