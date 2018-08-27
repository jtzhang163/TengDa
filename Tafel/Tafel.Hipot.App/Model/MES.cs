using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using Tafel.MES;
using TengDa;
using TengDa.Wpf;

namespace Tafel.Hipot.App
{
    public class MES : TengDa.Wpf.Mes
    {

        #region 本机IP
        private static string ipaddr = "127.0.0.1";

        /// <summary>
        /// 本机IP
        /// </summary>
        public static IPAddress LocalIPAddr
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

            if (Current.Mes.IsOffline)
            {
                return;
            }

            var datas = Context.InsulationContext.DataLogs.Where(d => d.Resistance > 0 && d.Temperature > 0)
                .Include("Battery").OrderByDescending(d => d.DateTime)
                .Where(i => !i.IsUploaded).Take(5).ToList();
            datas.ForEach(d =>
            {

                Current.Mes.RealtimeStatus = string.Format("MES检验通过，ID：{0}", d.Id);
                Thread.Sleep(100);
                if (MES.UploadBattery(d.Battery.Code, d.Resistance, d.Voltage, d.Temperature, d.TimeSpan))
                {
                    d.IsUploaded = true;
                    //上传MES
                    AppCurrent.YieldNow.BlankingOK++;
                    Current.Mes.RealtimeStatus = string.Format("上传MES完成，ID：{0}", d.Id);
                    Context.InsulationContext.SaveChanges();
                }
                else
                {
                    Current.Mes.RealtimeStatus = string.Format("上传MES失败，ID：{0}", d.Id);
                }

                Thread.Sleep(100);

            });

            var t = new Thread(() =>
            {
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

            if (!Current.Mes.IsEnabled)
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
                IPAddress = LocalIPAddr.ToString(),
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

        public static bool UploadBattery(string code, float resistance, float voltage, float temperature, float timespan)
        {
            if (!Current.Mes.IsEnabled)
            {
                return false;
            }
            if (!Current.Mes.IsAlive)
            {
                return false;
            }

            HipotInfo hipotInfo = new HipotInfo
            {
                BarcodeNo = code,
                ProcessCode = Current.Option.CurrentProcessCode,
                StationCode = Current.Option.CurrentStationCode,
                OrderNo = Current.Option.CurrentOrderNo,
                IPAddress = LocalIPAddr.ToString(),
                MaterialOrderNo = Current.Option.CurrentMaterialOrderNo,
                MachineNo = Current.Tester.Number,
                UserNumber = AppCurrent.User.Number,
                BarcodeNo_N = code,
                Resistance_N = resistance.ToString(),
                Voltage_N = voltage.ToString(),
                Temperature_N = temperature.ToString(),
                TestTimeSpan_N = timespan.ToString(),
                TestResult_N = (resistance > Current.Option.ThresholdResistance && temperature < Current.Option.ThresholdTemperature) ? "OK" : "NG",
                UserNumber_N = AppCurrent.User.Number,
                InsertTime_N = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
            };
            string msg = string.Empty;
            if (!Tafel.MES.MES.UploadBattery(hipotInfo, out msg))
            {
                OperationHelper.ShowTips(msg);
                return false;
            }
            return true;
        }

        public static void UploadMachineInfo(string state)
        {
            if (!Current.Mes.IsEnabled)
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

        public static void GetInfo()
        {
            if (!Current.Mes.IsPingSuccess)
            {
                return;
            }

            string msg = string.Empty;
            string ip = MES.LocalIPAddr.ToString();
            ProcessInfo pi = Tafel.MES.MES.GetProcessInfo(new IP { IPAddress = ip }, out msg);
            if (string.IsNullOrEmpty(msg))//成功获取到
            {
                if(pi == null)
                {
                    Error.Alert("无法获取到工序工位信息");
                    return;
                }           
                Current.Option.CurrentProcess = string.Format("{0},{1}", pi.ProcessName, pi.ProcessCode);
            }
            else
            {
                Error.Alert(msg);
            }


            StationInfo si = Tafel.MES.MES.GetStationInfo(new IpAndProcess { IPAddress = ip, ProcessCode = pi.ProcessCode }, out msg);
            if (string.IsNullOrEmpty(msg))//成功获取到
            {
                Current.Option.CurrentStation = string.Format("{0},{1}", si.StationName, si.StationCode);
            }
            else
            {
                Error.Alert(msg);
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
