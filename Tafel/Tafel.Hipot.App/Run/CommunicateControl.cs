using System;
using System.IO.Ports;
using TengDa.Wpf;

namespace Tafel.Hipot.App
{
    /// <summary>
    /// 通信控制
    /// </summary>
    public class CommunicateControl
    {
        /// <summary>
        /// 通信开始
        /// </summary>
        public static bool CommunicateStart()
        {
            if (Current.Tester.IsEnable)
            {
                var localPortNames = SerialPort.GetPortNames();
                if (Array.IndexOf(localPortNames, Current.Tester.PortName) < 0)
                {
                    OperationHelper.ShowTips("当前PC不存在串口：" + Current.Tester.PortName, true);
                    return false;
                }

                string msg = string.Empty;
                if (!Current.Tester.Connect(out msg))
                {
                    OperationHelper.ShowTips(msg, true);
                    Current.Tester.RealtimeStatus = "连接出现异常";
                    return false;
                }
                Current.Tester.RealtimeStatus = "连接成功";
                OperationHelper.ShowTips("连接串口成功：" + Current.Tester.PortName);
            }

            if (Current.Mes.IsEnable)
            {
                string msg = string.Empty;
                if(!Current.Mes.Connect(out msg))
                {
                    OperationHelper.ShowTips(msg, true);
                    Current.Mes.RealtimeStatus = "连接出现异常";
                    return false;
                }
                Current.Mes.RealtimeStatus = "连接成功";
                OperationHelper.ShowTips("连接MES成功：" + Current.Mes.Host);
            }
            return true;
        }

        /// <summary>
        /// 通信结束
        /// </summary>
        public static bool CommunicateStop()
        {
            if (Current.Tester.IsEnable)
            {
                string msg = string.Empty;
                if (!Current.Tester.DisConnect(out msg))
                {
                    OperationHelper.ShowTips(msg);
                    Current.Tester.RealtimeStatus = "断开连接出现异常";
                    return false;
                }
                Current.Tester.AlarmStr = string.Empty;
                Current.Tester.RealtimeStatus = "断开连接";
                OperationHelper.ShowTips("关闭串口连接成功：" + Current.Tester.PortName);
            }

            if (Current.Mes.IsEnable)
            {
                string msg = string.Empty;
                if (!Current.Mes.DisConnect(out msg))
                {
                    OperationHelper.ShowTips(msg);
                    Current.Mes.RealtimeStatus = "断开连接出现异常";
                    return false;
                }
                
                Current.Mes.RealtimeStatus = "断开连接";
                OperationHelper.ShowTips("关闭MES连接成功：" + Current.Mes.Host);
            }
            return true;
        }
    }
}
