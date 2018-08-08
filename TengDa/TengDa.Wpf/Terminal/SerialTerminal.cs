using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TengDa.Wpf
{
    /// <summary>
    /// RS232串口通信终端
    /// </summary>
    public abstract class SerialTerminal : Terminal, ICommunicate
    {

        /// <summary>
        /// 端口号
        /// </summary>
        [DisplayName("端口号")]
        [Category("通信设置")]
        [MaxLength(10)]
        public string PortName { get; set; } = "COM1";

        /// <summary>
        /// 波特率
        /// </summary>
        [DisplayName("波特率")]
        [Category("通信设置")]
        public int BaudRate { get; set; } = 9600;
        /// <summary>
        /// 奇偶校验位
        /// </summary>
        [DisplayName("奇偶校验位")]
        [Category("通信设置")]
        public Parity Parity { get; set; } = Parity.None;
        /// <summary>
        /// 数据位
        /// </summary>
        [DisplayName("数据位")]
        [Category("通信设置")]
        public int DataBits { get; set; } = 8;
        /// <summary>
        /// 停止位
        /// </summary>
        [DisplayName("停止位")]
        [Category("通信设置")]
        public StopBits StopBits { get; set; } = StopBits.One;


        private SerialPort serialPort = null;// => new SerialPort(PortName, BaudRate, Parity, DataBits, StopBits);

        [Browsable(false), NotMapped]
        public SerialPort SerialPort
        {
            get
            {
                if (serialPort == null)
                {
                    serialPort = new SerialPort(PortName, BaudRate, Parity, DataBits, StopBits);
                }
                return serialPort;
            }
            set
            {
                serialPort = value;
            }
        }

        public string ReceiveString = string.Empty;

        /// <summary>
        /// 获得新数据
        /// </summary>
        public bool IsGetNewData = false;

        /// <summary>
        /// 是否被动接受串口返回的数据
        /// </summary>
        [DisplayName("是否被动接受串口返回的数据"),ReadOnly(true)]
        public bool IsPassiveReceiveSerialPort { get; set; }


        #region 开启/断开连接、打开/关闭串口
        public bool Connect(out string msg)
        {
            try
            {
                if (IsPassiveReceiveSerialPort)
                {
                    SerialPort.DataReceived += new SerialDataReceivedEventHandler(SerialPort_DataReceived);
                }

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
                SerialPort = null;
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
                    SerialPort.ReceivedBytesThreshold = 1;
                    SerialPort.RtsEnable = true;
                    SerialPort.Open();
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return false;
            }

            if (SerialPort.IsOpen)
            {
                msg = string.Empty;
                return true;
            }
            msg = "打开" + SerialPort.PortName + "失败";
            return false;
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
                ASCIIEncoding encoding = new ASCIIEncoding();
                ReceiveString = encoding.GetString(InputBuf).Trim('\0');
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
