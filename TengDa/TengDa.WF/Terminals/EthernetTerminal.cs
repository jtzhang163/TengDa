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
using System.Collections.Generic;

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

        private Socket socket = null;

        private Socket Socket
        {
            get
            {
                if (socket == null)
                {
                    if(this.Company == PlcCompany.OMRON.ToString() && this.Model == "NX1P2")
                    {
                        socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                    }
                    else
                    {
                        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    }
                }
                return socket;
            }
            set
            {
                socket = value;
            }
        }

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

        private SiemensTcpNet siemens_net = null;

        private HslCommunice533.Profinet.Omron.OmronFinsNet omron_net = null;

        IPEndPoint point = null;

        #endregion

        #region 开启/断开连接
        public bool TcpConnect(out string msg)
        {
            msg = string.Empty;
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
                else if (this.Company == PlcCompany.Siemens.ToString())
                {
                    siemens_net = new SiemensTcpNet(SiemensPLCS.S1200);

                    siemens_net.PLCIpAddress = System.Net.IPAddress.Parse(this.IP);    // PLC的IP地址
                    siemens_net.PortRead = Port;                                           // 端口
                    siemens_net.PortWrite = 102;                                          // 写入端口，最好和读取分开
                    siemens_net.ConnectTimeout = 500;                                      // 连接超时时间
                    siemens_net.ConnectServer(); 
                    IsAlive = true;
                }
                else if (this.Company == PlcCompany.OMRON.ToString() && this.Model == "SYSMAC CP1H")
                {

                    omron_net = new HslCommunice533.Profinet.Omron.OmronFinsNet(this.IP, this.Port);
                    omron_net.SA1 = TengDa.Net.GetIpLastValue(Net.GetLocalIpByRegex("192.168.*"));  // PC网络号，PC的IP地址的最后一个数
                    omron_net.DA1 = TengDa.Net.GetIpLastValue(this.IP);  // PLC网络号，PLC的IP地址的最后一个数0
                    omron_net.DA2 = 0x00; // PLC单元号，通常为0

                    omron_net.ReceiveTimeOut = 1000;
                    omron_net.ConnectTimeOut = 1000;
                    HslCommunice533.OperateResult result = omron_net.ConnectServer();
                    if (!result.IsSuccess)
                    {
                        IsAlive = false;
                        return false;
                    }
                    IsAlive = true;
                }
                else if (this.Company == PlcCompany.OMRON.ToString() && this.Model == "NX1P2")
                {
                    point = new IPEndPoint(IPAddress.Parse(this.IP), this.Port);
                    IsAlive = true;
                }
                else if (!Socket.Connected)
                {
                    Socket.Connect(IpAddress, Port);
                    IsAlive = true;
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                IsAlive = false;
                return false;
            }
            return true;
        }

        public bool TcpDisConnect(out string msg)
        {
            try
            {
                if (this.Company == PlcCompany.Mitsubishi.ToString())
                {
                    if (melsec_net != null)
                    {
                        melsec_net.ConnectClose();// 关闭长连接，并切换为短连接，在系统退出时可以调用
                    }
                    IsAlive = false;
                }
                else if (this.Company == PlcCompany.Siemens.ToString())
                {
                    if (siemens_net != null)
                    {
                        siemens_net.ConnectClose();
                        siemens_net = null;
                    }
                    IsAlive = false;
                }
                else if (this.Company == PlcCompany.OMRON.ToString() && this.Model == "SYSMAC CP1H")
                {
                    if (omron_net != null)
                    {
                        omron_net.ConnectClose();
                        omron_net = null;
                    }
                    IsAlive = false;
                }
                else if (this.Company == PlcCompany.OMRON.ToString() && this.Model == "NX1P2")
                {
                    if (Socket != null)
                    {
                        Socket.Close();
                        Socket = null;
                    }
                    if (point != null)
                    {
                        point = null;
                    }                   
                    IsAlive = false;
                }
                else if (Socket != null)
                {
                    Socket.Close();
                    Socket = null;
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

        private bool connectSuccess = false;

        private string getStr = string.Empty;

        public bool GetInfo(string input, out string output, out string msg)
        {
            return GetInfo(true, 0, input, out output, out msg);
        }

        public bool GetInfo(string input, int readtimeout, out string output, out string msg)
        {
            return GetInfo(true, readtimeout, input, out output, out msg);
        }

        public bool GetInfo(bool checkPingSuccess, string input, out string output, out string msg)
        {
            return GetInfo(checkPingSuccess, 0, input, out output, out msg);
        }

        public bool GetInfo(bool checkPingSuccess, int readtimeout, string input, out string output, out string msg)
        {
            try
            {
                output = string.Empty;
                msg = string.Empty;
                if (checkPingSuccess)
                {
                    if (!IsPingSuccess)
                    {
                        IsAlive = false;
                        msg = string.Format("无法连接到【{0}】，IP：{1}", this.Name, this.IP);
                        return false;
                    }
                }

                if (!Socket.Connected)
                {
                    Socket = null;
                    Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    TcpConnect(out msg);
                }

                Byte[] sendBytes = Encoding.UTF8.GetBytes(input + "\r");
                Socket.Send(sendBytes);

                Thread.Sleep(10);

                getStr = string.Empty;
                Byte[] Data = new Byte[1024];

                if (readtimeout > 0)
                {
                    Stopwatch sw = new Stopwatch();
                    connectSuccess = false;
                    // Try to open the connection, if anything goes wrong, make sure we set connectSuccess = false
                    Thread t = new Thread(delegate ()
                    {
                        try
                        {
                            sw.Start();
                            Socket.Receive(Data);
                            getStr = Encoding.ASCII.GetString(Data).Trim('\0');
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
                        output = getStr.Replace("\r", "").Replace("\n", "").Trim('\0').Trim();
                        IsAlive = true;
                        return true;
                    }
                    else
                    {
                        msg = this.Name + "超时...";
                        return false;
                    }
                }
                else
                {
                    Socket.Receive(Data);
                    output = Encoding.ASCII.GetString(Data).Trim('\0');
                }

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

        public bool GetInfo(byte input, int readtimeout, out string output, out string msg)
        {
            return GetInfo(true, input, readtimeout, out output, out msg);
        }

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

                if (!Socket.Connected)
                {
                    socket = null;
                    socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    TcpConnect(out msg);
                }

                Byte[] sendBytes = new byte[] { input };

                Socket.Send(sendBytes);

                Thread.Sleep(10);

                getStr = string.Empty;
                Byte[] Data = new Byte[1024];

                Stopwatch sw = new Stopwatch();
                connectSuccess = false;
                // Try to open the connection, if anything goes wrong, make sure we set connectSuccess = false
                Thread t = new Thread(delegate ()
                {
                    try
                    {
                        sw.Start();
                        Socket.Receive(Data);
                        getStr = Encoding.ASCII.GetString(Data).Trim('\0');
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

        public bool GetInfo(string address, ushort length, out ushort[] output, out string msg)
        {
            output = new ushort[] { };
            msg = string.Empty;
            if (this.Company == PlcCompany.OMRON.ToString() && this.Model == "SYSMAC CP1H")
            {
                HslCommunice533.OperateResult<ushort[]> result = omron_net.ReadUInt16(address, length);
                if (result.IsSuccess)
                {
                    output = result.Content;
                    return true;
                }
                msg = result.Message;
            }
            else if (this.Company == PlcCompany.OMRON.ToString() && this.Model == "NX1P2")
            {
                try
                {
                    // string send = "80000200-11-0000-FE-0000-0101-82-03E8-0000-02";
                    string send = string.Format("80000200-{0:X2}-0000-{1:X2}-0000-0101-82-{2:X4}-0000-{3:X2}", TengDa.Net.GetIpLastValue(this.IP),
                        TengDa.Net.GetIpLastValue(Net.GetLocalIpByRegex("192.168.*")), Convert.ToUInt32(address.Replace("D", "")), length);
                    byte[] data = _Convert.HexStrTobyte(send.Replace("-", ""));
                    Socket.SendTo(data, point);
                    IPEndPoint sender = new IPEndPoint(IPAddress.Any,0);
                    EndPoint remote = (EndPoint)sender;
                    byte[] receiveData = new byte[1024];
                    int len = Socket.ReceiveFrom(receiveData, ref remote);
                    string recv = BitConverter.ToString(receiveData, 0, len);
                    string[] recvs = recv.Split('-');
                    var results = new List<ushort>();
                    if (recvs[12] == "00")
                    {
                        for (int n = 0; n < length; n++)
                        {
                            results.Add(Convert.ToUInt16(recvs[14 + 2 * n] + recvs[15 + 2 * n], 16));
                        }
                        output = results.ToArray();
                        return true;
                    }
                    msg = "读取数据出现错误";
                }
                catch (Exception ex)
                {
                    msg = ex.ToString();
                }
            }
            return false;
        }

        public bool SetInfo(string address, ushort val, out string msg)
        {
            msg = string.Empty;
            if (this.Company == PlcCompany.OMRON.ToString() && this.Model == "SYSMAC CP1H")
            {
                HslCommunice533.OperateResult result = omron_net.Write(address, val);
                if (result.IsSuccess)
                {
                    return true;
                }
                msg = result.Message;
            }
            else if (this.Company == PlcCompany.OMRON.ToString() && this.Model == "NX1P2")
            {
                try
                {
                    // string send = "80000200-11-0000-FE-0000-0101-82-03E8-0000-02";
                    string send = string.Format("80000200-{0:X2}-0000-{1:X2}-0000-0102-82-{2:X4}-000001-{3:X4}", TengDa.Net.GetIpLastValue(this.IP),
                        TengDa.Net.GetIpLastValue(Net.GetLocalIpByRegex("192.168.*")), Convert.ToUInt32(address.Replace("D", "")), val);
                    byte[] data = _Convert.HexStrTobyte(send.Replace("-", ""));
                    Socket.SendTo(data, point);
                    IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
                    EndPoint remote = (EndPoint)sender;
                    byte[] receiveData = new byte[1024];
                    int len = Socket.ReceiveFrom(receiveData, ref remote);
                    string recv = BitConverter.ToString(receiveData, 0, len);
                    string[] recvs = recv.Split('-');
                    var results = new List<ushort>();
                    if (recvs[12] == "00")
                    {
                        return true;
                    }
                    msg = "写入出现错误";
                }
                catch (Exception ex)
                {
                    msg = ex.Message;
                }
            }
            return false;
        }

        public bool SetInfo(byte input, out string msg)
        {
            return SetInfo(false, input, out msg);
        }

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
                if (!Socket.Connected)
                {
                    socket = null;
                    socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    TcpConnect(out msg);
                }

                Socket.Send(new byte[] { input });
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

        public bool SetInfo(string input, out string msg)
        {
            return SetInfo(false, input, out msg);
        }

        public bool SetInfo(bool checkPingSuccess, string input, out string msg)
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
                if (!Socket.Connected)
                {
                    socket = null;
                    socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    TcpConnect(out msg);
                }
                Byte[] sendBytes = Encoding.UTF8.GetBytes(input + "\r");
                Socket.Send(sendBytes);
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

        public bool GetInfo(bool checkPingSuccess, PlcCompany plcCompany, bool isRead, string address, bool value, out bool output, out string msg)
        {

            output = false;
            msg = string.Empty;

            if (checkPingSuccess)
            {
                if (!IsPingSuccess)
                {
                    IsAlive = false;
                    msg = string.Format("无法连接到【{0}】，IP：{1}", this.Name, this.IP);
                    return false;
                }
            }

            try
            {

                if (plcCompany == PlcCompany.Mitsubishi)
                {

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
                else if (this.Company == PlcCompany.Siemens.ToString())
                {
                    if (isRead)//读
                    {
                        OperateResult<bool> result = siemens_net.ReadBoolFromPLC(address);
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
                        OperateResult result = siemens_net.WriteIntoPLC(address, value);
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

            if (checkPingSuccess)
            {
                if (!IsPingSuccess)
                {
                    IsAlive = false;
                    msg = string.Format("无法连接到【{0}】，IP：{1}", this.Name, this.IP);
                    return false;
                }
            }

            try
            {
                if (plcCompany == PlcCompany.Mitsubishi)
                {

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
                else if (plcCompany == PlcCompany.Siemens)
                {

                    if (isRead)//读
                    {
                        OperateResult<byte> result = siemens_net.ReadByteFromPLC(address);
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
                        OperateResult result = siemens_net.WriteIntoPLC(address, value);
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

        public bool GetInfo(bool checkPingSuccess, PlcCompany plcCompany, bool isRead, string address, byte value, out int output, out string msg)
        {

            output = -1;
            msg = string.Empty;

            if (checkPingSuccess)
            {
                if (!IsPingSuccess)
                {
                    IsAlive = false;
                    msg = string.Format("无法连接到【{0}】，IP：{1}", this.Name, this.IP);
                    return false;
                }
            }

            try
            {
                if (plcCompany == PlcCompany.Siemens)
                {

                    if (isRead)//读
                    {
                        OperateResult<byte> result = siemens_net.ReadByteFromPLC(address);
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
                        OperateResult result = siemens_net.WriteIntoPLC(address, value);
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

        //public bool GetInfo<T>(PlcCompany plcCompany, bool isRead, string address, T value, out T output, out string msg) where T : IDataTransfer, new()
        //{
        //    return GetInfo<T>(true, plcCompany, isRead, address, value, out output, out msg);
        //}

        //public bool GetInfo<T>(bool checkPingSuccess, PlcCompany plcCompany, bool isRead, string address, T value, out T output, out string msg) where T : IDataTransfer, new()
        //{

        //    output = default(T);
        //    msg = string.Empty;

        //    try
        //    {
        //        if (plcCompany == PlcCompany.Mitsubishi)
        //        {
        //            if (checkPingSuccess)
        //            {
        //                if (!IsPingSuccess)
        //                {
        //                    IsAlive = false;
        //                    msg = string.Format("无法连接到【{0}】，IP：{1}", this.Name, this.IP);
        //                    return false;
        //                }
        //            }

        //            if (isRead)//读
        //            {
        //                OperateResult<T> result = melsec_net.ReadFromPLC<T>(address);
        //                if (result.IsSuccess)
        //                {
        //                    output = result.Content;
        //                    IsAlive = true;
        //                    return true;
        //                }
        //                else
        //                {
        //                    msg = string.Format("从{0} 中读取数据出现错误，代码：{1}", address, result.ErrorCode);
        //                    IsAlive = false;
        //                    return false;
        //                }
        //            }
        //            else//写
        //            {
        //                OperateResult result = melsec_net.WriteIntoPLC<T>(address, value);
        //                if (result.IsSuccess)
        //                {
        //                    IsAlive = true;
        //                    return true;
        //                }
        //                else
        //                {
        //                    msg = string.Format("{0} 中写入 {1} 出现错误，代码：{2}", address, value, result.ErrorCode);
        //                    IsAlive = false;
        //                    return false;
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        msg = string.Format("和{0}通信出现异常！原因：{1}", Name, ex.Message);
        //    }

        //    IsAlive = false;
        //    return false;
        //}

        #endregion
    }
}

