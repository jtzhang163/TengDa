using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TengDa.Wpf
{
    /// <summary>
    /// 上下文
    /// </summary>
    public static class Context
    {
        public static OperationContext OperationContext = new OperationContext();
        public static UserContext UserContext = new UserContext();
        public static YieldContext YieldContext = new YieldContext();
    }
}
