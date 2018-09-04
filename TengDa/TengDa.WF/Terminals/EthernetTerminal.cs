using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Net.NetworkInformation;
using System.ComponentModel;
using HslCommunication.Profinet;
using HslCommunication;
using System.Diagnostics;

namespace TengDa.WF.Terminals
{
    /// <summary>
    /// 以太网通信终端
    /// </summary>
    public abstract class EthernetTerminal : Terminal, ICommunicate
    {
        #region 属性

        protected string ip = "0.0.0.0";
        protected int port = 0;

        [Category("基本设置")]
        public string IP
        {
            get
            {
                return ip;
            }
            set
            {
                if (Regex.IsMatch(value, RegexString.IPv4))
                {
                    if (ip != value)
                    {
                        this.UpdateDbField("IP", value);
                    }
                    ip = value;
                }
                else
                {
                    throw new Exception("IP格式错误");
                }
            }
        }

        private IPAddress IpAddress
        {
            get
            {
                return IPAddress.Parse(IP);
            }
        }

        private TcpClient tcpClient = null;

        private TcpClient TcpClient
        {
            get
            {
                if (tcpClient == null)
                {
                    tcpClient = new TcpClient();
                }
                return tcpClient;
            }
            set
            {
                tcpClient = value;
            }
        }

        [Description("端口")]
        [DisplayName("端口")]
        [Category("基本设置")]
        public int Port
        {
            get
            {
                return port;
            }
            set
            {
                if (value > 0 && value <= 65535)
                {
                    if (port != value)
                    {
                        this.UpdateDbField("Port", value);
                    }
                    port = value;
                }
                else
                {
                    throw new Exception("Port输入范围[1-65535]错误: port: " + value);
                }
            }
        }

        /// <summary>
        /// 是否能Ping通，可判断远程主机是否存在
        /// </summary>
        [ReadOnly(true)]
        [Description("是否能Ping通，可判断远程主机是否存在")]
        [DisplayName("是否能Ping通")]
        public bool IsPingSuccess
        {
            get
            {
                try
                {
                    Ping ping = new Ping();
                    PingReply pingReply = ping.Send(IP, 500);
                    if (pingReply.Status == IPStatus.Success)
                    {
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.WriteError(ex);
                }
                IsAlive = false;
                return false;
            }
        }

        //public bool IsCommunicatting = false;

        private MelsecNet melsec_net = new MelsecNet();
        #endregion

        #region 开启/断开连接
        public bool TcpConnect(out string msg)
        {
            try
            {
                if (this.Company == PlcCompany.Mitsubishi.ToString())
                {
                    //初始化
                    melsec_net.PLCIpAddress = System.Net.IPAddress.Parse(this.IP);    // PLC的IP地址
                    melsec_net.PortRead = Port;                                           // 端口
                    melsec_net.PortWrite = 6001;                                          // 写入端口，最好和读取分开
                    melsec_net.NetworkNumber = 0;                                         // 网络号
                    melsec_net.NetworkStationNumber = 0;                                  // 网络站号
                    melsec_net.ConnectTimeout = 500;                                      // 连接超时时间

                    // 如果需要长连接，就取消下面这行代码的注释，对于数据读写的代码，没有影响
                    melsec_net.ConnectServer(); // 切换长连接，这行代码可以放在其他任何地方
                    IsAlive = true;
                }
                else if (!TcpClient.Connected)
                {
                    TcpClient.Connect(IpAddress, Port);
                    IsAlive = true;
                }
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

        public bool TcpDisConnect(out string msg)
        {
            try
            {
                if (this.Company == PlcCompany.Mitsubishi.ToString())
                {
                    melsec_net.ConnectClose();// 关闭长连接，并切换为短连接，在系统退出时可以调用
                    IsAlive = false;
                }
                else
                {
                    TcpClient.Close();
                    TcpClient = null;
                    IsAlive = false;
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                IsAlive = true;
                return false;
            }
            msg = string.Empty;
            return true;
        }
        #endregion

        #region 通信
        public bool GetInfo(string input, out string output, out string msg)
        {
            return GetInfo(true, input, out output, out msg);
        }
        /// <summary>
        /// 避免每次调用IsPingSuccess出现异常
        /// </summary>
        /// <param name="checkPingSuccess"></param>
        /// <param name="input"></param>
        /// <param name="output"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool GetInfo(bool checkPingSuccess, string input, out string output, out string msg)
        {
            try
            {
                if (checkPingSuccess)
                {
                    if (!IsPingSuccess)
                    {
                        IsAlive = false;
                        output = string.Empty;
                        msg = string.Format("无法连接到【{0}】，IP：{1}", this.Name, this.IP);
                        return false;
                    }
                }

                if (!TcpClient.Connected)
                {
                    tcpClient = null;
                    tcpClient = new TcpClient();
                    TcpConnect(out msg);
                }
                NetworkStream ns = TcpClient.GetStream();
                Byte[] sendBytes = Encoding.UTF8.GetBytes(input + "\r");
                ns.Write(sendBytes, 0, sendBytes.Length);

                Thread.Sleep(100);

                Byte[] Data = new Byte[1024];
                NetworkStream nsData = TcpClient.GetStream();
                Int32 bytes = nsData.Read(Data, 0, Data.Length);


                output = Encoding.ASCII.GetString(Data, 0, bytes);

                msg = string.Empty;
                IsAlive = true;
                return true;

            }
            catch (Exception ex)
            {
                msg = string.Format("和{0}通信出现异常！原因：{1}", Name, ex.Message);
            }

            IsAlive = false;
            output = string.Empty;
            return false;
        }

        public bool GetInfo(Byte input, int readtimeout, out string output, out string msg)
        {
            return GetInfo(true, input, readtimeout, out output, out msg);
        }

        private bool connectSuccess = false;
        private string getStr = string.Empty;
        /// <summary>
        /// 避免每次调用IsPingSuccess出现异常
        /// </summary>
        /// <param name="checkPingSuccess"></param>
        /// <param name="input"></param>
        /// <param name="output"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool GetInfo(bool checkPingSuccess, byte input, int readtimeout, out string output, out string msg)
        {

            //IsCommunicatting = true;
            output = string.Empty;
            msg = string.Empty;
            try
            {
                if (checkPingSuccess)
                {
                    if (!IsPingSuccess)
                    {
                        IsAlive = false;
                        output = string.Empty;
                        msg = string.Format("无法连接到【{0}】，IP：{1}", this.Name, this.IP);
                        return false;
                    }
                }

                if (!TcpClient.Connected)
                {
                    tcpClient = null;
                    tcpClient = new TcpClient();
                    TcpConnect(out msg);
                }
                NetworkStream ns = TcpClient.GetStream();
                Byte[] sendBytes = new byte[] { input };
                ns.Write(sendBytes, 0, sendBytes.Length);

                Thread.Sleep(100);

                getStr = string.Empty;
                Byte[] Data = new Byte[1024];
                NetworkStream nsData = TcpClient.GetStream();

                Stopwatch sw = new Stopwatch();
                connectSuccess = false;
                // Try to open the connection, if anything goes wrong, make sure we set connectSuccess = false
                Thread t = new Thread(delegate ()
                {
                    try
                    {
                        sw.Start();
                        Int32 bytes = nsData.Read(Data, 0, Data.Length);
                        getStr = Encoding.ASCII.GetString(Data, 0, bytes);
                        connectSuccess = true;
                    }
                    catch { }
                });

                // Make sure it's marked as a background thread so it'll get cleaned up automatically
                t.IsBackground = true;
                t.Start();

                // Keep trying to join the thread until we either succeed or the timeout value has been exceeded
                while (readtimeout > sw.ElapsedMilliseconds)
                    if (t.Join(1))
                        break;
               // IsCommunicatting = false;
                // If we didn't connect successfully, throw an exception
                if (connectSuccess)
                {
                    //throw new Exception("Timed out while trying to connect.");
                    output = getStr.Replace("\r", "").Replace("\n", "").Trim();
                    IsAlive = true;
                    return true;
                }
                else
                {
                    msg = this.Name + "超时...";
                    return false;
                }
            }
            catch (Exception ex)
            {
                msg = string.Format("和{0}通信出现异常！原因：{1}", Name, ex.Message);
                IsAlive = false;
                return false;
            }
        }

        /// <summary>
        /// 避免每次调用IsPingSuccess出现异常
        /// </summary>
        /// <param name="checkPingSuccess"></param>
        /// <param name="input"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool SetInfo(byte input, out string msg)
        {
            return SetInfo(true, input, out msg);
        }

        /// <summary>
        /// 避免每次调用IsPingSuccess出现异常
        /// </summary>
        /// <param name="checkPingSuccess"></param>
        /// <param name="input"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool SetInfo(bool checkPingSuccess, byte input, out string msg)
        {
            try
            {
                if (checkPingSuccess)
                {
                    if (!IsPingSuccess)
                    {
                        IsAlive = false;
                        msg = string.Format("无法连接到【{0}】，IP：{1}", this.Name, this.IP);
                        return false;
                    }
                }
                if (!TcpClient.Connected)
                {
                    tcpClient = null;
                    tcpClient = new TcpClient();
                    TcpConnect(out msg);
                }
                NetworkStream ns = TcpClient.GetStream();
                Byte[] sendBytes = new byte[] { input };
                ns.Write(sendBytes, 0, sendBytes.Length);
                msg = string.Empty;
                IsAlive = true;
                return true;
            }
            catch (Exception ex)
            {
                msg = string.Format("和{0}通信出现异常！原因：{1}", Name, ex.Message);
            }

            IsAlive = false;
            return false;
        }

        public bool GetInfo<T>(PlcCompany plcCompany, bool isRead, string address, T value, out T output, out string msg) where T : IDataTransfer, new()
        {
            return GetInfo<T>(true, plcCompany, isRead, address, value, out output, out msg);
        }

        public bool GetInfo<T>(bool checkPingSuccess, PlcCompany plcCompany, bool isRead, string address, T value, out T output, out string msg) where T : IDataTransfer, new()
        {

            output = default(T);
            msg = string.Empty;

            try
            {
                if (plcCompany == PlcCompany.Mitsubishi)
                {
                    if (checkPingSuccess)
                    {
                        if (!IsPingSuccess)
                        {
                            IsAlive = false;
                            msg = string.Format("无法连接到【{0}】，IP：{1}", this.Name, this.IP);
                            return false;
                        }
                    }

                    if (isRead)//读
                    {
                        OperateResult<T> result = melsec_net.ReadFromPLC<T>(address);
                        if (result.IsSuccess)
                        {
                            output = result.Content;
                            IsAlive = true;
                            return true;
                        }
                        else
                        {
                            msg = string.Format("从{0} 中读取数据出现错误，代码：{1}", address, result.ErrorCode);
                            IsAlive = false;
                            return false;
                        }
                    }
                    else//写
                    {
                        OperateResult result = melsec_net.WriteIntoPLC<T>(address, value);
                        if (result.IsSuccess)
                        {
                            IsAlive = true;
                            return true;
                        }
                        else
                        {
                            msg = string.Format("{0} 中写入 {1} 出现错误，代码：{2}", address, value, result.ErrorCode);
                            IsAlive = false;
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                msg = string.Format("和{0}通信出现异常！原因：{1}", Name, ex.Message);
            }

            IsAlive = false;
            return false;
        }

        public bool GetInfo(bool checkPingSuccess, PlcCompany plcCompany, bool isRead, string address, bool value, out bool output, out string msg)
        {

            output = false;
            msg = string.Empty;

            try
            {
                if (plcCompany == PlcCompany.Mitsubishi)
                {
                    if (checkPingSuccess)
                    {
                        if (!IsPingSuccess)
                        {
                            IsAlive = false;
                            msg = string.Format("无法连接到【{0}】，IP：{1}", this.Name, this.IP);
                            return false;
                        }
                    }

                    if (isRead)//读
                    {
                        OperateResult<bool> result = melsec_net.ReadBoolFromPLC(address);
                        if (result.IsSuccess)
                        {
                            output = result.Content;
                            IsAlive = true;
                            return true;
                        }
                        else
                        {
                            msg = string.Format("从{0} 中读取数据出现错误，代码：{1}", address, result.ErrorCode);
                            IsAlive = false;
                            return false;
                        }
                    }
                    else//写
                    {
                        OperateResult result = melsec_net.WriteIntoPLC(address, value);
                        if (result.IsSuccess)
                        {
                            IsAlive = true;
                            return true;
                        }
                        else
                        {
                            msg = string.Format("{0} 中写入 {1} 出现错误，代码：{2}", address, value, result.ErrorCode);
                            IsAlive = false;
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                msg = string.Format("和{0}通信出现异常！原因：{1}", Name, ex.Message);
            }

            IsAlive = false;
            return false;
        }

        public bool GetInfo(bool checkPingSuccess, PlcCompany plcCompany, bool isRead, string address, int value, out int output, out string msg)
        {

            output = -1;
            msg = string.Empty;

            try
            {
                if (plcCompany == PlcCompany.Mitsubishi)
                {
                    if (checkPingSuccess)
                    {
                        if (!IsPingSuccess)
                        {
                            IsAlive = false;
                            msg = string.Format("无法连接到【{0}】，IP：{1}", this.Name, this.IP);
                            return false;
                        }
                    }

                    if (isRead)//读
                    {
                        OperateResult<int> result = melsec_net.ReadIntFromPLC(address);
                        if (result.IsSuccess)
                        {
                            output = result.Content;
                            IsAlive = true;
                            return true;
                        }
                        else
                        {
                            msg = string.Format("从{0}中读取数据出现错误，代码：{1}", address, result.ErrorCode);
                            IsAlive = false;
                            return false;
                        }
                    }
                    else//写
                    {
                        OperateResult result = melsec_net.WriteIntoPLC(address, value);
                        if (result.IsSuccess)
                        {
                            IsAlive = true;
                            return true;
                        }
                        else
                        {
                            msg = string.Format("{0} 中写入 {1} 出现错误，代码：{2}", address, value, result.ErrorCode);
                            IsAlive = false;
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                msg = string.Format("和{0}通信出现异常！原因：{1}", Name, ex.Message);
            }

            IsAlive = false;
            return false;
        }
        #endregion
    }
}

