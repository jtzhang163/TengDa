using CAMEL.RGV.Touchscreen.Util;
using HslCommunication;
using HslCommunication.Profinet.Omron;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CAMEL.RGV.Touchscreen
{
    public class RGV : BindableObject
    {

        #region 基本参数
        private string ip;
        public string IP
        {
            get
            {
                return ip;
            }
            set
            {
                this.SetProperty(ref ip, value);
            }
        }

        private int port;
        public int Port
        {
            get
            {
                return port;
            }
            set
            {
                this.SetProperty(ref port, value);
            }
        }

        private bool isConnected;
        public bool IsConnected
        {
            get
            {
                return isConnected;
            }
            set
            {
                this.SetProperty(ref isConnected, value);
            }
        }

        #endregion

        #region 设备参数

        public int _步号;
        public int 步号
        {
            set { this.SetProperty(ref _步号, value); }
            get { return _步号; }
        }

        public int _行走位;
        public int 行走位
        {
            set { this.SetProperty(ref _行走位, value); }
            get { return _行走位; }
        }

        public int _取货位号;
        public int 取货位号
        {
            set { this.SetProperty(ref _取货位号, value); }
            get { return _取货位号; }
        }

        public int _放货位号;
        public int 放货位号
        {
            set { this.SetProperty(ref _放货位号, value); }
            get { return _放货位号; }
        }

        public int _货叉进;
        public int 货叉进
        {
            set { this.SetProperty(ref _货叉进, value); }
            get { return _货叉进; }
        }

        private int _运行时间;
        public int 运行时间
        {
            get
            {
                return _运行时间;
            }
            set
            {
                this.SetProperty(ref _运行时间, value);
            }
        }

        private int _行走定位时间;
        public int 行走定位时间
        {
            get
            {
                return _行走定位时间;
            }
            set
            {
                this.SetProperty(ref _行走定位时间, value);
            }
        }

        private int _升降定位时间;
        public int 升降定位时间
        {
            get
            {
                return _升降定位时间;
            }
            set
            {
                this.SetProperty(ref _升降定位时间, value);
            }
        }

        private int _货叉定位时间;
        public int 货叉定位时间
        {
            get
            {
                return _货叉定位时间;
            }
            set
            {
                this.SetProperty(ref _货叉定位时间, value);
            }
        }

        private int _行走当前速度;
        public int 行走当前速度
        {
            get
            {
                return _行走当前速度;
            }
            set
            {
                this.SetProperty(ref _行走当前速度, value);
            }
        }

        public int _保存参数HIM;
        public int 保存参数HIM
        {
            set { this.SetProperty(ref _保存参数HIM, value); }
            get { return _保存参数HIM; }
        }
        public int _货叉电机原点设定;
        public int 货叉电机原点设定
        {
            set { this.SetProperty(ref _货叉电机原点设定, value); }
            get { return _货叉电机原点设定; }
        }
        public int _行走JOG正转;
        public int 行走JOG正转
        {
            set { this.SetProperty(ref _行走JOG正转, value); }
            get { return _行走JOG正转; }
        }
        public int _行走JOG反转;
        public int 行走JOG反转
        {
            set { this.SetProperty(ref _行走JOG反转, value); }
            get { return _行走JOG反转; }
        }
        public int _升降JOG上升;
        public int 升降JOG上升
        {
            set { this.SetProperty(ref _升降JOG上升, value); }
            get { return _升降JOG上升; }
        }
        public int _升降JOG降下;
        public int 升降JOG降下
        {
            set { this.SetProperty(ref _升降JOG降下, value); }
            get { return _升降JOG降下; }
        }
        public int _货叉JOG正转;
        public int 货叉JOG正转
        {
            set { this.SetProperty(ref _货叉JOG正转, value); }
            get { return _货叉JOG正转; }
        }
        public bool _货叉原点;
        public bool 货叉原点
        {
            set { this.SetProperty(ref _货叉原点, value); }
            get { return _货叉原点; }
        }
        public int _货叉JOG反转;
        public int 货叉JOG反转
        {
            set { this.SetProperty(ref _货叉JOG反转, value); }
            get { return _货叉JOG反转; }
        }
        public bool _行走测试;
        public bool 行走测试
        {
            set { this.SetProperty(ref _行走测试, value); }
            get { return _行走测试; }
        }
        public bool _升降1测试;
        public bool 升降1测试
        {
            set { this.SetProperty(ref _升降1测试, value); }
            get { return _升降1测试; }
        }
        public bool _升降2测试;
        public bool 升降2测试
        {
            set { this.SetProperty(ref _升降2测试, value); }
            get { return _升降2测试; }
        }
        public bool _货叉测试;
        public bool 货叉测试
        {
            set { this.SetProperty(ref _货叉测试, value); }
            get { return _货叉测试; }
        }
        public bool _参数写入;
        public bool 参数写入
        {
            set { this.SetProperty(ref _参数写入, value); }
            get { return _参数写入; }
        }

        public bool _蜂鸣停止;
        public bool 蜂鸣停止
        {
            set { this.SetProperty(ref _蜂鸣停止, value); }
            get { return _蜂鸣停止; }
        }


        public int _升降当前速度;
        public int 升降当前速度
        {
            set { this.SetProperty(ref _升降当前速度, value); }
            get { return _升降当前速度; }
        }
        public int _货叉当前速度;
        public int 货叉当前速度
        {
            set { this.SetProperty(ref _货叉当前速度, value); }
            get { return _货叉当前速度; }
        }

        public int _行走加速时间;
        public int 行走加速时间
        {
            set { this.SetProperty(ref _行走加速时间, value); }
            get { return _行走加速时间; }
        }


        public int _行走减速时间;
        public int 行走减速时间
        {
            set { this.SetProperty(ref _行走减速时间, value); }
            get { return _行走减速时间; }
        }
        public int _行走目标速度;
        public int 行走目标速度
        {
            set { this.SetProperty(ref _行走目标速度, value); }
            get { return _行走目标速度; }
        }
        public int _升降加速时间;
        public int 升降加速时间
        {
            set { this.SetProperty(ref _升降加速时间, value); }
            get { return _升降加速时间; }
        }
        public int _升降减速时间;
        public int 升降减速时间
        {
            set { this.SetProperty(ref _升降减速时间, value); }
            get { return _升降减速时间; }
        }
        public int _升降目标速度;
        public int 升降目标速度
        {
            set { this.SetProperty(ref _升降目标速度, value); }
            get { return _升降目标速度; }
        }
        public int _货叉加速时间;
        public int 货叉加速时间
        {
            set { this.SetProperty(ref _货叉加速时间, value); }
            get { return _货叉加速时间; }
        }
        public int _货叉减速时间;
        public int 货叉减速时间
        {
            set { this.SetProperty(ref _货叉减速时间, value); }
            get { return _货叉减速时间; }
        }
        public int _货叉目标速度;
        public int 货叉目标速度
        {
            set { this.SetProperty(ref _货叉目标速度, value); }
            get { return _货叉目标速度; }
        }
        public int _货位号;
        public int 货位号
        {
            set { this.SetProperty(ref _货位号, value); }
            get { return _货位号; }
        }

        public int _行走电机;
        public int 行走电机
        {
            set { this.SetProperty(ref _行走电机, value); }
            get { return _行走电机; }
        }
        public int _升降电机;
        public int 升降电机
        {
            set { this.SetProperty(ref _升降电机, value); }
            get { return _升降电机; }
        }
        public int _货叉电机;
        public int 货叉电机
        {
            set { this.SetProperty(ref _货叉电机, value); }
            get { return _货叉电机; }
        }
        public int _行走位置参数;
        public int 行走位置参数
        {
            set { this.SetProperty(ref _行走位置参数, value); }
            get { return _行走位置参数; }
        }
        public int _升降1位置参数;
        public int 升降1位置参数
        {
            set { this.SetProperty(ref _升降1位置参数, value); }
            get { return _升降1位置参数; }
        }
        public int _升降2位置参数;
        public int 升降2位置参数
        {
            set { this.SetProperty(ref _升降2位置参数, value); }
            get { return _升降2位置参数; }
        }

        public int _货叉位置参数;
        public int 货叉位置参数
        {
            set { this.SetProperty(ref _货叉位置参数, value); }
            get { return _货叉位置参数; }
        }

        public int _行走当前位置;
        public int 行走当前位置
        {
            set { this.SetProperty(ref _行走当前位置, value); }
            get { return _行走当前位置; }
        }

        public int _升降当前位置;
        public int 升降当前位置
        {
            set { this.SetProperty(ref _升降当前位置, value); }
            get { return _升降当前位置; }
        }

        public int _货叉当前位置;
        public int 货叉当前位置
        {
            set { this.SetProperty(ref _货叉当前位置, value); }
            get { return _货叉当前位置; }
        }

        public bool _平板有效;
        public bool 平板有效
        {
            set { this.SetProperty(ref _平板有效, value); }
            get { return _平板有效; }
        }

        public bool _手动状态;
        public bool 手动状态
        {
            set { this.SetProperty(ref _手动状态, value); }
            get { return _手动状态; }
        }

        public bool _调度无效;
        public bool 调度无效
        {
            set { this.SetProperty(ref _调度无效, value); }
            get { return _调度无效; }
        }

        #endregion

        private string alarmStr;
        public string AlarmStr
        {
            set { this.SetProperty(ref alarmStr, value); }
            get { return alarmStr; }
        }

        private bool isAlarming;
        public bool IsAlarming
        {
            set { this.SetProperty(ref isAlarming, value); }
            get { return isAlarming; }
        }

        public string Alarm2BinString { get; private set; }

        int overtimeCount = 0;
        public string[] Alarms = new string[] {
            "急停按下",
            "PLC异常状态异常",
            "EtherNet/IP异常状态异常",
            "EthenCAT状态异常",
            "PLC总线状态异常",
            "行走电机报警",
            "升降电机报警",
            "货叉电机报警",
            "调度心跳报警",
            "门号错误",
            "行走.货叉同时动作",
            "行走电机前限报警",
            "升降电机上限报警",
            "货叉电机前限报警",
            "升降下降超过保护限位",
            "货叉不在原点",
            "行走安全位.取门号.放门号不能同时给",
            "行走位置方向错误",
            "升降位置方向错误",
            "行走位置错误",
            "升降位置错误",
            "货叉位置错误",
            "请复位完成信号",
            "行走电机后限报警",
            "行走电机下限报警",
            "货叉电机后限报警",
            "料框前限报警",
            "料框后限报警",
            "货叉前炉内有料框报警",
            "货叉后炉内有料框报警",
            "货叉前炉门未打开报警",
            "货叉后炉门未打开报警",
            "货叉有料报警",
            "货叉无料报警",
            "相序错误"
        };

        private OmronFinsNet omron_net = null;
        public bool Connect(out string msg)
        {
            msg = "";
            if (!Net.IsPingSuccess(this.IP))
            {
                msg = "无法连接到 " + this.IP;
                return false;
            }

            if (omron_net == null)
            {
                omron_net = new OmronFinsNet(this.IP, this.Port);
                var ips = this.IP.Split('.');
                omron_net.SA1 = Net.GetIpLastValue(Net.GetLocalIpByRegex(string.Format("{0}.{1}.{2}.*", ips[0], ips[1], ips[2])));  // PC网络号，PC的IP地址的最后一个数
                omron_net.DA1 = Net.GetIpLastValue(this.IP);  // PLC网络号，PLC的IP地址的最后一个数0
                omron_net.DA2 = 0x00; // PLC单元号，通常为0

                omron_net.ReceiveTimeOut = 1000;
                omron_net.ConnectTimeOut = 1000;
                OperateResult result = omron_net.ConnectServer();
                System.Diagnostics.Debug.WriteLine("连接欧姆龙 ："+result.IsSuccess);
                if (!result.IsSuccess)
                {
                    msg = result.Message;
                    this.IsConnected = false;
                    return false;
                }
            }
            this.IsConnected = true;
            return true;
        }

        public bool DisConnect(out string msg)
        {
            msg = "";
            if (omron_net != null)
            {
                omron_net.ConnectClose();
                omron_net = null;
            }
            this.IsConnected = false;
            return true;
        }

        public bool Read(string addr, out short value, out string msg)
        {
            msg = "";
            value = 0;
            try
            {
                OperateResult<short> result = omron_net.ReadInt16(addr);
                if (result.IsSuccess)
                {
                    value = result.Content;
                    return true;
                }
                msg = result.Message;
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            return false;
        }

        public bool Write(string addr, short value, out string msg)
        {
            msg = "";
            try
            {
                OperateResult result = omron_net.Write(addr, value);
                if (result.IsSuccess)
                {
                    return true;
                }
                msg = result.Message;
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            return false;
        }

        public void GetInfo()
        {         
            if (!this.IsConnected)
            {
                return;
            }

            OperateResult<short[]> result = omron_net.ReadInt16("D2000", (ushort)100);
                     
            if (result.IsSuccess)
            {
                overtimeCount = 0;
                var datas = result.Content;               
                this.运行时间 = datas[4];
                this.行走定位时间 = datas[5];
                this.升降定位时间 = datas[6];
                this.货叉定位时间 = datas[7];

                this.行走当前速度 = datas[8];
                this.升降当前速度 = datas[9];
                this.货叉当前速度 = datas[10];

                this.行走加速时间 = datas[20];
                this.行走减速时间 = datas[21];
                this.行走目标速度 = datas[22];
                this.升降加速时间 = datas[23];
                this.升降减速时间 = datas[24];
                this.升降目标速度 = datas[25];
                this.货叉加速时间 = datas[26];
                this.货叉减速时间 = datas[27];
                this.货叉目标速度 = datas[28];
                this.货位号 = datas[29];

              
                this.升降当前位置 = datas[32];
                this.货叉当前位置 = datas[34];
                
                this.升降电机 = datas[38];
                this.货叉电机 = datas[40];

                this.升降1位置参数 = datas[44];
                this.升降2位置参数 = datas[46];
                this.货叉位置参数 = datas[48];

                this.步号 = datas[61];

                this.货叉原点 = datas[68] == 1;

                this.蜂鸣停止 = datas[60] == 1;
                this.行走测试 = datas[70] == 1;
                this.升降1测试 = datas[71] == 1;
                this.升降2测试 = datas[72] == 1;
                this.货叉测试 = datas[73] == 1;
                this.参数写入 = datas[62] == 1;

                this.平板有效 = datas[76] == 1;
            }
            else
            {
                ReeultError(result.Message);
                //if (result.Message.Contains("1000") && overtimeCount < 1)
                //{
                //    overtimeCount++;
                //}
                //else
                //{
                //    this.IsConnected = false;
                //    MessageBox.Show("连接PLC出错：" + result.Message, "异常提示", MessageBoxButton.OK, MessageBoxImage.Information);
                //}
            }

            OperateResult<ushort[]> resultUInt16 = omron_net.ReadUInt16("D2000", 50);
            if (resultUInt16.IsSuccess)
            {
                overtimeCount = 0;
                var datasUInt16 = resultUInt16.Content;

                this.行走当前位置 = datasUInt16[30];
                this.行走电机 = datasUInt16[36];
                this.行走位置参数 = datasUInt16[42];
            }
            else
            {
                ReeultError(result.Message);
                //if (result.Message.Contains("1000") && overtimeCount < 1)
                //{
                //    overtimeCount++;
                //}
                //else
                //{
                //    this.IsConnected = false;
                //    MessageBox.Show("连接PLC出错：" + result.Message, "异常提示", MessageBoxButton.OK, MessageBoxImage.Information);
                //}
            }
            if (omron_net == null)
            {
                return;
            }

            result = omron_net.ReadInt16("D1000", (ushort)100);
            if (result.IsSuccess)
            {
                overtimeCount = 0;
                var datas = result.Content;
                this.手动状态 = datas[6] == 0;

                this.行走位 = datas[7];
                this.取货位号 = datas[9];
                this.放货位号 = datas[8];
                this.货叉进 = datas[10];

                this.调度无效 = datas[13] == 2;
                this.IsAlarming = datas[11] == 1 || datas[60] == 4;
                if (this.IsAlarming)
                {
                    StringBuilder sb = new StringBuilder();
                    for (int n = 30; n < 33; n++)
                    {
                        sb.Append(_Convert.Revert(_Convert.GetBitStr(datas[n], 16)));
                    }

                    this.Alarm2BinString = sb.ToString();

                    var alarmStr = "";

                    for (int x = 0; x < this.Alarms.Length; x++)
                    {
                        if (this.Alarm2BinString[x] == '1')
                        {
                            alarmStr += this.Alarms[x] + ",";
                        }
                    }
                    this.AlarmStr = alarmStr;
                }
                else
                {
                    this.AlarmStr = "";
                }
            }
            else
            {
                ReeultError(result.Message);
                //this.IsConnected = false;
                //MessageBox.Show("连接PLC出错：" + result.Message, "异常提示", MessageBoxButton.OK, MessageBoxImage.Information);
            }

        }

        public void ReeultError(string msg)
        {
            if (msg.Contains("1000") && overtimeCount < 1)
            {
                overtimeCount++;
            }
            else
            {
                this.IsConnected = false;
                MessageBox.Show("连接PLC出错：" + msg, "异常提示", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
