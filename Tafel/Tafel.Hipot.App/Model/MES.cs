using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using Tafel.MES;
using TengDa.Wpf;

namespace Tafel.Hipot.App
{
    public class MES : TengDa.Wpf.Mes
    {

        #region 本机IP
        private static string ipaddr = "127.0.0.1";

        public static IPAddress IPAddr
        {
            get
            {
                List<IPAddress> ips = TengDa.Net.GetIPList().Where(i => Regex.IsMatch(i.ToString(), Current.Option.IPAddressRegex)).ToList();
                if (ips.Count > 0)
                {
                    ipaddr = ips[0].ToString();
                }
                return IPAddress.Parse(ipaddr);
            }
        }
        #endregion

        public MES() : this(-1)
        {

        }
        public MES(int id)
        {
            this.Id = id;
        }

        public static void Upload()
        {
            var datas = Context.InsulationContext.DataLogs.Where(i => !i.IsUploaded).Take(100).ToList();
            datas.ForEach(d =>
            {
                d.IsUploaded = true;

                //上传MES

                AppCurrent.YieldNow.BlankingOK++;

                Current.Mes.RealtimeStatus = string.Format("上传MES完成，电阻：{0}，电压：{1}，测试间隔：{2}，温度：{3}", d.Resistance, d.Voltage, d.TimeSpan, d.Temperature);
                Context.InsulationContext.SaveChanges();
                Thread.Sleep(200);
            });

            var t = new Thread(() => {
                Thread.Sleep(1000);
                Current.Mes.RealtimeStatus = "等待上传";
            });
            t.IsBackground = true;
            t.Start();
        }

        #region MES方法


        public static bool Login(string userNumber, string password, out string msg)
        {

            string userName = string.Empty;

            if (!MES.GetUserName(userNumber, password, out userName, out msg))
            {
                return false;
            }

            if (TengDa.Wpf.Context.UserContext.Users.Where(u => u.Name == userName).Count() == 0)
            {
                //首次登录需先注册
                if (!User.Register(userName, userNumber, password, true, out msg))
                {
                    return false;
                }
            }

            return User.Login(userName, password, out msg);
        }



        public static bool CheckSfc(string code, out string msg)
        {

            if (!Current.Mes.IsEnable)
            {
                msg = "MES未启用";
                return false;
            }
            if (!Current.Mes.IsAlive)
            {
                msg = "MES未在线";
                return false;
            }
            Sfc sfc = new Sfc
            {
                BarcodeNo = code,
                IPAddress = IPAddr.ToString(),
                MachineNo = Current.Tester.Number,
                MaterialOrderNo = Current.Option.CurrentMaterialOrderNo,
                OrderNo = Current.Option.CurrentOrderNo,
                ProcessCode = Current.Option.CurrentProcessCode,
                StationCode = Current.Option.CurrentStationCode,
                UserNumber = AppCurrent.User.Number
            };

            if (!Tafel.MES.MES.CheckSfc(sfc, out msg))
            {
                return false;
            }

            msg = string.Empty;
            return true;
        }

        public static bool UploadBattery(string code, string trayCode)
        {
            if (!Current.Mes.IsEnable)
            {
                return false;
            }
            if (!Current.Mes.IsAlive)
            {
                return false;
            }
            TrayInfo trayInfo = new TrayInfo
            {
                TrayCode = trayCode,
                BarcodeNo = code,
                ProcessCode = Current.Option.CurrentProcessCode,
                UserNumber = AppCurrent.User.Number,
                InputTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
            };
            string msg = string.Empty;
            if (!Tafel.MES.MES.UploadBattery(trayInfo, out msg))
            {
                Error.Alert(msg);
                return false;
            }
            return true;
        }

        public static void UploadMachineInfo(string state)
        {
            if (!Current.Mes.IsEnable)
            {
                return;
            }
            if (!Current.Mes.IsAlive)
            {
                return;
            }
            MachineState ms = new MachineState
            {
                state = (State)Enum.Parse(typeof(State), state),
                MachineNo = Current.Tester.Number,
                UserNumber = AppCurrent.User.Number
            };
            string msg = string.Empty;
            if (!Tafel.MES.MES.UploadMachineState(ms, out msg))
            {
                Error.Alert(msg);
            }

        }

        public static void GetInfo(out string lbProcessText, out string lbStationText, out string msg)
        {
            string ip = MES.IPAddr.ToString();
            ProcessInfo pi = Tafel.MES.MES.GetProcessInfo(new IP { IPAddress = ip }, out msg);
            if (string.IsNullOrEmpty(msg))//成功获取到
            {
                lbProcessText = string.Format("{0}[{1}]", pi.ProcessName, pi.ProcessCode);
                Current.Option.CurrentProcess = string.Format("{0},{1}", pi.ProcessName, pi.ProcessCode);
            }
            else
            {
                Error.Alert(msg);
                string[] s = Current.Option.CurrentProcess.Split(',');
                lbProcessText = string.Format("{0}[{1}]", s[0], s[1]);
            }


            StationInfo si = Tafel.MES.MES.GetStationInfo(new IpAndProcess { IPAddress = ip, ProcessCode = pi.ProcessCode }, out msg);
            if (string.IsNullOrEmpty(msg))//成功获取到
            {
                lbStationText = string.Format("{0}[{1}]", si.StationName, si.StationCode);
                Current.Option.CurrentStation = string.Format("{0},{1}", si.StationName, si.StationCode);
            }
            else
            {
                string[] s = Current.Option.CurrentStation.Split(',');
                lbStationText = string.Format("{0}[{1}]", s[0], s[1]);
            }
        }

        public static bool GetUserName(string userNumber, string password, out string userName, out string msg)
        {
            Tafel.MES.MESUser user = new Tafel.MES.MESUser
            {
                UserNumber = userNumber,
                UserPwd = password,
                MachineNo = Current.Tester.Number,
                ProcessCode = Current.Option.CurrentProcessCode,
                StationCode = Current.Option.CurrentStationCode
            };
            return Tafel.MES.MES.CheckUser(user, out userName, out msg);
        }
        #endregion
    }
}
