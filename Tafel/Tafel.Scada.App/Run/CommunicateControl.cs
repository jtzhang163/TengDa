using System;
using System.IO.Ports;

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
            if (AppCurrent.InsulationTester.IsEnable)
            {

                var localPortNames = SerialPort.GetPortNames();

                if (Array.IndexOf(localPortNames, AppCurrent.InsulationTester.PortName) < 0)
                {
                    AppCurrent.AppViewModel.ShowTips("当前PC不存在串口：" + AppCurrent.InsulationTester.PortName);
                    return false;
                }

                string msg = string.Empty;
                if (!AppCurrent.InsulationTester.Connect(out msg))
                {
                    AppCurrent.AppViewModel.ShowTips(msg);
                    return false;
                }
                AppCurrent.AppViewModel.ShowTips("连接串口成功：" + AppCurrent.InsulationTester.PortName);
            }
            return true;
        }

        /// <summary>
        /// 通信结束
        /// </summary>
        public static bool CommunicateStop()
        {
            if (AppCurrent.InsulationTester.IsEnable)
            {
                string msg = string.Empty;
                if (!AppCurrent.InsulationTester.DisConnect(out msg))
                {
                    AppCurrent.AppViewModel.ShowTips(msg);
                    return false;
                }
                AppCurrent.InsulationTester.AlarmStr = string.Empty;
                AppCurrent.AppViewModel.ShowTips("关闭串口连接成功：" + AppCurrent.InsulationTester.PortName);
            }
            return true;
        }
    }
}
