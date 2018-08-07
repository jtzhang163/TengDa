using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TengDa.Wpf;

namespace Zopoise.Scada.App
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
      if (AppCurrent.Plc.IsEnable)
      {
        if (!AppCurrent.Plc.IsPingSuccess)
        {
          AppCurrent.AppViewModel.ShowTips("无法连接到PLC：" + AppCurrent.Plc.IP);
          return false;
        }

        string msg = string.Empty;
        if (!AppCurrent.Plc.TcpConnect(out msg))
        {
          AppCurrent.AppViewModel.ShowTips(msg);
          return false;
        }
      }

      AppCurrent.AppViewModel.ShowTips("连接PLC成功，IP：" + AppCurrent.Plc.IP);

      if (AppCurrent.Communicator.IsEnable)
      {

        var localPortNames = SerialPort.GetPortNames();

        if (Array.IndexOf(localPortNames, AppCurrent.Communicator.PortName) < 0)
        {
          AppCurrent.AppViewModel.ShowTips("当前PC不存在串口：" + AppCurrent.Communicator.PortName);
          return false;
        }

        string msg = string.Empty;
        if (!AppCurrent.Communicator.Connect(out msg))
        {
          AppCurrent.AppViewModel.ShowTips(msg);
          return false;
        }
        AppCurrent.AppViewModel.ShowTips("连接串口成功：" + AppCurrent.Communicator.PortName);
      }
      return true;
    }

    /// <summary>
    /// 通信结束
    /// </summary>
    public static bool CommunicateStop()
    {

      if (AppCurrent.Plc.IsEnable)
      {
        string msg = string.Empty;
        if (!AppCurrent.Plc.TcpDisConnect(out msg))
        {
          AppCurrent.AppViewModel.ShowTips(msg);
          return false;
        }
        AppCurrent.Plc.AlarmStr = string.Empty;
      }

      AppCurrent.AppViewModel.ShowTips("关闭和PLC的连接成功，IP：" + AppCurrent.Plc.IP);

      if (AppCurrent.Communicator.IsEnable)
      {

        string msg = string.Empty;
        if (!AppCurrent.Communicator.DisConnect(out msg))
        {
          AppCurrent.AppViewModel.ShowTips(msg);
          return false;
        }
        AppCurrent.Communicator.AlarmStr = string.Empty;
        AppCurrent.AppViewModel.ShowTips("关闭串口连接成功：" + AppCurrent.Communicator.PortName);
      }
      return true;
    }
  }
}
