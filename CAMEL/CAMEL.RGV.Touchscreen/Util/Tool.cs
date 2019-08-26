using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAMEL.RGV.Touchscreen.Util
{
    public static class Tool
    {
        /// <summary>
        /// 获取类中的属性值
        /// </summary>
        public static string GetValue(object obj, string fieldName)
        {
            try
            {
                Type t = obj.GetType();
                object o = t.GetProperty(fieldName).GetValue(obj, null);
                string Value = Convert.ToString(o);
                if (string.IsNullOrEmpty(Value)) return null;
                return Value;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 设置类中的属性值
        /// </summary>
        public static bool SetValue(object obj, string fieldName, object value)
        {
            try
            {
                Type t = obj.GetType();
                t.GetProperty(fieldName).SetValue(obj, value, null);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
