using System;
using System.IO.Ports;
using TengDa.Wpf;

namespace Zopoise.Scada.Bak
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
            if (Current.Plc.IsEnabled)
            {
                if (!Current.Plc.IsPingSuccess)
                {
                    OperationHelper.ShowTips("无法连接到PLC：" + Current.Plc.IP);
                    return false;
                }

                string msg = string.Empty;
                if (!Current.Plc.Connect(out msg))
                {
                    OperationHelper.ShowTips(msg);
                    return false;
                }
            }

            OperationHelper.ShowTips("连接PLC成功，IP：" + Current.Plc.IP);

            if (Current.Communicator.IsEnabled)
            {

                var localPortNames = SerialPort.GetPortNames();

                if (Array.IndexOf(localPortNames, Current.Communicator.PortName) < 0)
                {
                    OperationHelper.ShowTips("当前PC不存在串口：" + Current.Communicator.PortName);
                    return false;
                }

                string msg = string.Empty;
                if (!Current.Communicator.Connect(out msg))
                {
                    OperationHelper.ShowTips(msg);
                    return false;
                }
                OperationHelper.ShowTips("连接串口成功：" + Current.Communicator.PortName);
            }
            return true;
        }

        /// <summary>
        /// 通信结束
        /// </summary>
        public static bool CommunicateStop()
        {

            if (Current.Plc.IsEnabled)
            {
                string msg = string.Empty;
                if (!Current.Plc.DisConnect(out msg))
                {
                    OperationHelper.ShowTips(msg);
                    return false;
                }
                Current.Plc.AlarmStr = string.Empty;
            }

            OperationHelper.ShowTips("关闭和PLC的连接成功，IP：" + Current.Plc.IP);

            if (Current.Communicator.IsEnabled)
            {

                string msg = string.Empty;
                if (!Current.Communicator.DisConnect(out msg))
                {
                    OperationHelper.ShowTips(msg);
                    return false;
                }
                Current.Communicator.AlarmStr = string.Empty;
                OperationHelper.ShowTips("关闭串口连接成功：" + Current.Communicator.PortName);
            }
            return true;
        }
    }
}
