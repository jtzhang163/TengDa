using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TengDa
{
    public class OmronPLC
    {
        public static string GetBitStr(ushort val, int totalWidth)
        {
            return Convert.ToString(val, 2).PadLeft(totalWidth, '0');
        }
    }
}
