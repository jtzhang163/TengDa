using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TengDa.WF.Terminals
{
    interface ICommunicate
    {
        bool GetInfo(string input, out string output, out string msg);
    }
}
