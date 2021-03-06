﻿using System;
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
            if (Current.Tester.IsEnabled)
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
                OperationHelper.ShowTips("连接电阻测试仪成功：" + Current.Tester.PortName);
            }

            if (Current.Collector.IsEnabled)
            {
                var localPortNames = SerialPort.GetPortNames();
                if (Array.IndexOf(localPortNames, Current.Collector.PortName) < 0)
                {
                    OperationHelper.ShowTips("当前PC不存在串口：" + Current.Collector.PortName, true);
                    return false;
                }

                Current.Collector.StopBits = StopBits.None;

                string msg = string.Empty;
                if (!Current.Collector.Connect(out msg))
                {
                    OperationHelper.ShowTips(msg, true);
                    Current.Collector.RealtimeStatus = "连接出现异常";
                    return false;
                }
                Current.Collector.RealtimeStatus = "连接成功";
                OperationHelper.ShowTips("连接温度采集器成功：" + Current.Collector.PortName);
            }

            if (Current.Cooler.IsEnabled)
            {
                if (!Current.Cooler.PLC.IsPingSuccess)
                {
                    OperationHelper.ShowTips(string.Format("无法连接到{0}，IP：{1}", Current.Cooler.Name, Current.Cooler.PLC.IP), true);
                    return false;
                }
                string msg = string.Empty;
                if (!Current.Cooler.PLC.Connect(out msg))
                {
                    OperationHelper.ShowTips(msg, true);
                    Current.Cooler.RealtimeStatus = "连接出现异常";
                    return false;
                }
                Current.Cooler.RealtimeStatus = "连接成功";
                OperationHelper.ShowTips("连接冷却机成功：" + Current.Cooler.PLC.IP);
            }

            if (Current.Scaner.IsEnabled)
            {
                if (!Current.Scaner.IsPingSuccess)
                {
                    OperationHelper.ShowTips(string.Format("无法连接到{0}，IP：{1}", Current.Scaner.Name, Current.Scaner.IP), true);
                    return false;
                }
                string msg = string.Empty;
                if (!Current.Scaner.Connect(out msg))
                {
                    OperationHelper.ShowTips(msg, true);
                    Current.Scaner.RealtimeStatus = "连接出现异常";
                    return false;
                }
                Current.Scaner.RealtimeStatus = "连接成功";
                OperationHelper.ShowTips("连接扫码枪成功：" + Current.Scaner.IP);
            }

            if (Current.Mes.IsEnabled)
            {
                if (!Current.Mes.IsPingSuccess)
                {
                    OperationHelper.ShowTips(string.Format("无法连接到{0}，IP：{1}",Current.Mes.Name, Current.Mes.Host), true);
                    return false;
                }
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
            if (Current.Tester.IsEnabled)
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

            if (Current.Collector.IsEnabled)
            {
                string msg = string.Empty;
                if (!Current.Collector.DisConnect(out msg))
                {
                    OperationHelper.ShowTips(msg);
                    Current.Collector.RealtimeStatus = "断开连接出现异常";
                    return false;
                }
                Current.Collector.AlarmStr = string.Empty;
                Current.Collector.RealtimeStatus = "断开连接";
                OperationHelper.ShowTips("关闭串口连接成功：" + Current.Collector.PortName);
            }

            if (Current.Cooler.IsEnabled)
            {
                string msg = string.Empty;
                if (!Current.Cooler.PLC.DisConnect(out msg))
                {
                    OperationHelper.ShowTips(msg);
                    Current.Cooler.RealtimeStatus = "断开连接出现异常";
                    return false;
                }
                Current.Cooler.IsAlive = false;
                Current.Cooler.RealtimeStatus = "断开连接";
                OperationHelper.ShowTips("关闭PLC连接成功：" + Current.Cooler.PLC.IP);
            }

            if (Current.Scaner.IsEnabled)
            {
                string msg = string.Empty;
                if (!Current.Scaner.DisConnect(out msg))
                {
                    OperationHelper.ShowTips(msg);
                    Current.Scaner.RealtimeStatus = "断开连接出现异常";
                    return false;
                }

                Current.Scaner.RealtimeStatus = "断开连接";
                OperationHelper.ShowTips("关闭扫码枪连接成功：" + Current.Scaner.IP);
            }

            if (Current.Mes.IsEnabled)
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
