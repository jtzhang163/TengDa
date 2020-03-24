using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TengDa;
using TengDa.WF;

namespace Anchitech.Baking
{
    public class Cleaner
    {
        public static void ClearOldDbData()
        {
            try
            {
                new Thread(() =>
                {
                    string msg = "";

                    if (AlarmLog.GetCount(out msg) > 21000)
                    {
                        AlarmLog.DeleteLongAgo(out msg);
                    }

                    if (Battery.GetCount(out msg) > 110000)
                    {
                        Battery.DeleteLongAgo(out msg);
                    }

                    if (Clamp.GetCount(out msg) > 11000)
                    {
                        Clamp.DeleteLongAgo(out msg);
                    }

                    if (FloorLog.GetCount(out msg) > 11000)
                    {
                        FloorLog.DeleteLongAgo(out msg);
                    }

                    if (TaskLog.GetCount(out msg) > 11000)
                    {
                        TaskLog.DeleteLongAgo(out msg);
                    }

                    if (TVD.GetCount(out msg) > 210000)
                    {
                        TVD.DeleteLongAgo(out msg);
                    }

                }).Start();
            }
            catch (Exception ex)
            {
                LogHelper.WriteError("删除数据库中较早的数据失败，msg：" + ex.ToString());
            }
        }
    }
}
