using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Ports;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace TengDa.WF.Terminals
{
    /// <summary>
    /// RS232串口通信终端
    /// </summary>
    public abstract class SerialTerminal : Terminal, ICommunicate
    {
        #region 属性
        protected string portName = string.Empty;
        protected int baudRate = 9600;
        /// <summary>
        /// 端口号
        /// </summary>
        [Description("端口号")]
        [DisplayName("端口号")]
        [Category("基本设置")]
        public string PortName
        {
            get
            {
                return portName;
            }
            set
            {
                if (portName != value)
                {
                    UpdateDbField("PortName", value);
                }
                portName = value;
            }
        }

        /// <summary>
        /// 波特率
        /// </summary>
        [Description("波特率")]
        [DisplayName("波特率")]
        [Category("基本设置")]
        public int BaudRate
        {
            get
            {
                return baudRate;
            }
            set
            {
                if (baudRate != value)
                {
                    UpdateDbField("BaudRate", value);
                }
                baudRate = value;
            }
        }

        public SerialPort SerialPort = new SerialPort();
        public string ReceiveString = string.Empty;

        private bool isGetPassiveReceiveSerialPort = false;
        private bool isPassiveReceiveSerialPort = true;

        /// <summary>
        /// 获得新数据
        /// </summary>
        [Description("获得新数据")]
        [DisplayName("获得新数据")]
        [ReadOnly(true)]
        public bool IsGetNewData { get; set; } = false;

        /// <summary>
        /// 是否被动接受串口返回的数据
        /// </summary>
        private bool IsPassiveReceiveSerialPort
        {
            get
            {
                if (!isGetPassiveReceiveSerialPort)
                {
                    isPassiveReceiveSerialPort = TengDa._Convert.StrToBool(Option.GetOption("IsPassiveReceiveSerialPort"), true);
                    isGetPassiveReceiveSerialPort = true;
                }
                return isPassiveReceiveSerialPort;
            }
        }
        #endregion

        #region 开启/断开连接、打开/关闭串口
        public bool Connect(out string msg)
        {
            try
            {
                SerialPort.DataReceived += new SerialDataReceivedEventHandler(SerialPort_DataReceived);
                if (this.OpenPort(out msg))
                {
                    IsAlive = true;
                    msg = string.Empty;
                    return true;
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                IsAlive = false;
            }
            return false;
        }

        public bool DisConnect(out string msg)
        {
            try
            {
                this.ClosePort(out msg);
                SerialPort.DataReceived -= new SerialDataReceivedEventHandler(SerialPort_DataReceived);
                SerialPort.Dispose();
                IsAlive = false;
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                IsAlive = false;
                return false;
            }
            msg = string.Empty;
            return true;
        }

        public bool OpenPort(out string msg)
        {
            try
            {
                if (!SerialPort.IsOpen)
                {
                    SerialPort.Open();
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return false;
            }
            msg = string.Empty;
            return true;
        }

        public bool ClosePort(out string msg)
        {
            try
            {
                if (SerialPort.IsOpen)
                {
                    SerialPort.Close();
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return false;
            }
            msg = string.Empty;
            return true;
        }
        #endregion

        #region 通信
        public void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            Byte[] InputBuf = new Byte[128];
            try
            {
                if (IsPassiveReceiveSerialPort)
                {
                    //接收数据
                    ReceiveString = "";
                    IsGetNewData = false;

                    do
                    {
                        int count = SerialPort.BytesToRead;
                        if (count <= 0)
                            break;
                        byte[] readBuffer = new byte[count];
                        SerialPort.Read(readBuffer, 0, count);
                        ReceiveString += System.Text.Encoding.Default.GetString(readBuffer).Trim('\0').Trim('\r').Trim('\n');
                        Thread.Sleep(10);
                    } while (SerialPort.BytesToRead > 0);

                    IsAlive = true;
                    IsGetNewData = true;
                }
            }
            catch (Exception ex)
            {
                IsAlive = false;
                Error.Alert(ex);
            }
        }

        public bool GetInfo(string input, out string output, out string msg)
        {

            output = string.Empty;
            try
            {
                string ReceiveString = string.Empty;
                SerialPort.Write(input);

                Thread.Sleep(500);

                Byte[] InputBuf = new Byte[128];
                SerialPort.Read(InputBuf, 0, SerialPort.BytesToRead);
                ReceiveString = Encoding.ASCII.GetString(InputBuf).Trim('\0');
                IsAlive = true;

                if (!string.IsNullOrEmpty(ReceiveString))
                {
                    output = ReceiveString;
                    msg = string.Empty;
                    return true;
                }
                IsAlive = true;

                msg = "指定时间串口未返回数据！";
                return false;
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            IsAlive = false;
            return false;
        }
        #endregion
    }
}
