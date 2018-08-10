using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
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
                List<IPAddress> ips = TengDa.Net.GetIPList().Where(i => Regex.IsMatch(i.ToString(), AppCurrent.Option.IPAddressRegex)).ToList();
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
        public MES(long id)
        {
            this.Id = id;
        }

        public static void Upload()
        {
            var datas = AppContext.InsulationContext.InsulationDataLogs.Where(i => !i.IsUploaded).Take(100).ToList();
            datas.ForEach(d =>
            {
                d.IsUploaded = true;

                //上传MES

                Current.RealtimeYieldViewModel.BlankingOK++;

                AppCurrent.Mes.RealtimeStatus = string.Format("上传MES完成，电阻：{0}，电压：{1}，测试间隔：{2}，温度：{3}", d.Resistance, d.Voltage, d.TimeSpan, d.Temperature);
                AppContext.InsulationContext.SaveChangesAsync();
                Thread.Sleep(200);
            });

            var t = new Thread(() => {
                Thread.Sleep(1000);
                AppCurrent.Mes.RealtimeStatus = "等待上传";
            });
            t.IsBackground = true;
            t.Start();
        }

        #region MES方法


        public static bool CheckSfc(string code, out string msg)
        {

            if (!AppCurrent.Mes.IsEnable)
            {
                msg = "MES未启用";
                return false;
            }
            if (!AppCurrent.Mes.IsAlive)
            {
                msg = "MES未在线";
                return false;
            }
            Sfc sfc = new Sfc
            {
                BarcodeNo = code,
                IPAddress = IPAddr.ToString(),
                MachineNo = AppCurrent.InsulationTester.Number,
                MaterialOrderNo = AppCurrent.Option.CurrentMaterialOrderNo,
                OrderNo = AppCurrent.Option.CurrentOrderNo,
                ProcessCode = AppCurrent.Option.CurrentProcessCode,
                StationCode = AppCurrent.Option.CurrentStationCode,
                UserNumber = Current.User.Number
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
            if (!AppCurrent.Mes.IsEnable)
            {
                return false;
            }
            if (!AppCurrent.Mes.IsAlive)
            {
                return false;
            }
            TrayInfo trayInfo = new TrayInfo
            {
                TrayCode = trayCode,
                BarcodeNo = code,
                ProcessCode = AppCurrent.Option.CurrentProcessCode,
                UserNumber = Current.User.Number,
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
            if (!AppCurrent.Mes.IsEnable)
            {
                return;
            }
            if (!AppCurrent.Mes.IsAlive)
            {
                return;
            }
            MachineState ms = new MachineState
            {
                state = (State)Enum.Parse(typeof(State), state),
                MachineNo = AppCurrent.InsulationTester.Number,
                UserNumber = Current.User.Number
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
                AppCurrent.Option.CurrentProcess = string.Format("{0},{1}", pi.ProcessName, pi.ProcessCode);
            }
            else
            {
                Error.Alert(msg);
                string[] s = AppCurrent.Option.CurrentProcess.Split(',');
                lbProcessText = string.Format("{0}[{1}]", s[0], s[1]);
            }


            StationInfo si = Tafel.MES.MES.GetStationInfo(new IpAndProcess { IPAddress = ip, ProcessCode = pi.ProcessCode }, out msg);
            if (string.IsNullOrEmpty(msg))//成功获取到
            {
                lbStationText = string.Format("{0}[{1}]", si.StationName, si.StationCode);
                AppCurrent.Option.CurrentStation = string.Format("{0},{1}", si.StationName, si.StationCode);
            }
            else
            {
                string[] s = AppCurrent.Option.CurrentStation.Split(',');
                lbStationText = string.Format("{0}[{1}]", s[0], s[1]);
            }
        }

        public static bool GetUserName(string userNumber, string password, out string userName, out string msg)
        {
            Tafel.MES.MESUser user = new Tafel.MES.MESUser
            {
                UserNumber = userNumber,
                UserPwd = password,
                MachineNo = AppCurrent.InsulationTester.Number,
                ProcessCode = AppCurrent.Option.CurrentProcessCode,
                StationCode = AppCurrent.Option.CurrentStationCode
            };
            return Tafel.MES.MES.CheckUser(user, out userName, out msg);
        }
        #endregion
    }

    public class MesContext : DbContext
    {

        private static string connectionString = ConfigurationManager.ConnectionStrings["DefaultDatabase"].ToString();
        public MesContext() : base(connectionString)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MES>().ToTable("t_mes");
        }

        public DbSet<MES> MESs { get; set; }
    }

    public class MesInitializer : DropCreateDatabaseIfModelChanges<MesContext>
    {
        protected override void Seed(MesContext context)
        {
            var mes = new MES
            {
                Name = "MES",
                Host = "192.168.1.1"
            };
            context.MESs.Add(mes);
            context.SaveChanges();
        }
    }
}
