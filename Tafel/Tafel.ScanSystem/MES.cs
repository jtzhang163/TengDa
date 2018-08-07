using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using Tafel.MES;
using TengDa;
using TengDa.WF;

namespace Tafel.ScanSystem
{
    public class MES : TengDa.WF.MES.MES
    {
        #region 本机IP
        private static string ipaddr = "127.0.0.1";

        public static IPAddress IPAddr
        {
            get
            {

                List<IPAddress> ips = TengDa.Net.GetIPList().Where(i => Regex.IsMatch(i.ToString(), "10.1.*")).ToList();
                if (ips.Count > 0)
                {
                    ipaddr = ips[0].ToString();
                }
                return IPAddress.Parse(ipaddr);    
            }
        }
        #endregion

        #region 构造方法

        public MES() : this(-1) { }

        public MES(int id)
        {
            if (id < 0)
            {
                this.Id = -1;
                return;
            }

            string msg = string.Empty;

            DataTable dt = Database.Query("SELECT * FROM [dbo].[" + TableName + "] WHERE Id = " + id, out msg);

            if (!string.IsNullOrEmpty(msg))
            {
                Error.Alert(msg);
                return;
            }

            if (dt == null || dt.Rows.Count == 0) { return; }

            Init(dt.Rows[0]);

            //释放资源
            dt.Dispose();
        }


        #endregion

        #region 初始化方法
        protected void Init(DataRow rowInfo)
        {
            if (rowInfo == null)
            {
                this.Id = -1;
                return;
            }

            InitFields(rowInfo);
        }

        protected void InitFields(DataRow rowInfo)
        {
            this.Id = TengDa._Convert.StrToInt(rowInfo["Id"].ToString(), -1);
            this.name = rowInfo["Name"].ToString();
            this.host = rowInfo["Host"].ToString();
            this.IsEnable = Convert.ToBoolean(rowInfo["IsEnable"]);
        }
        #endregion

        #region 系统层列表
        private static List<MES> mesList = new List<MES>();
        private static List<MES> MesList
        {
            get
            {
                string msg = string.Empty;

                DataTable dt = Database.Query("SELECT * FROM [dbo].[" + TableName + "]", out msg);
                if (!string.IsNullOrEmpty(msg))
                {
                    Error.Alert(msg);
                    return null;
                }

                if (dt == null || dt.Rows.Count == 0)
                {
                    mesList = null;
                }
                else
                {
                    mesList.Clear();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        MES mes = new MES();
                        mes.InitFields(dt.Rows[i]);
                        mesList.Add(mes);
                    }
                }
                return mesList;
            }
        }

        #endregion

        #region 获取

        public static MES GetMES(string name, out string msg)
        {
            try
            {
                List<MES> list = (from mes in MesList where mes.Name == name select mes).ToList();
                if (list.Count() > 0)
                {
                    msg = string.Empty;
                    return list[0];
                }
                msg = string.Format("数据库不存在名称为 {0} 的PLC！", name);
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }

            return null;
        }

        #endregion

        #region MES方法


        public static bool CheckSfc(string code, out string msg)
        {

            if (!Current.mes.IsOffline)
            {
                Sfc sfc = new Sfc
                {
                    BarcodeNo = code,
                    IPAddress = MES.IPAddr.ToString(),
                    MachineNo = Current.feeder.Number,
                    MaterialOrderNo = Current.option.CurrentMaterialOrderNo,
                    OrderNo = Current.option.CurrentOrderNo,
                    ProcessCode = Current.option.CurrentProcessCode,
                    StationCode = Current.option.CurrentStationCode,
                    UserNumber = TengDa.WF.Current.user.Number
                };
               
                if (!Tafel.MES.MES.CheckSfc(sfc, out msg))
                {
                    return false;
                }
            }
            msg = string.Empty;
            return true;
        }

        public static bool UploadBattery(string code ,string trayCode)
        {
            if (!Current.mes.IsOffline)
            {
                TrayInfo trayInfo = new TrayInfo
                {
                    TrayCode = trayCode,
                    BarcodeNo = code,
                    ProcessCode = Current.option.CurrentProcessCode,
                    UserNumber = TengDa.WF.Current.user.Number,
                    InputTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                };
                string msg = string.Empty;
                if (!Tafel.MES.MES.UploadBattery(trayInfo, out msg))
                {
                    Error.Alert(msg);
                    return false;
                }
            }
            return true;
        }

        public static void UploadMachineInfo(string state)
        {
            if (!Current.mes.IsOffline)
            {
                MachineState ms = new MachineState
                {
                    state = (State)Enum.Parse(typeof(State), state),
                    MachineNo = Current.feeder.Number,
                    UserNumber = TengDa.WF.Current.user.Number
                };
                string msg = string.Empty;
                if (!Tafel.MES.MES.UploadMachineState(ms, out msg))
                {
                    Error.Alert(msg);
                }
            }
        }

        public static void GetInfo(out string lbProcessText, out string lbStationText, out string msg)
        {
            string ip = MES.IPAddr.ToString();
            ProcessInfo pi = Tafel.MES.MES.GetProcessInfo(new IP { IPAddress = ip }, out msg);
            if (string.IsNullOrEmpty(msg))//成功获取到
            {
                lbProcessText = string.Format("{0}[{1}]", pi.ProcessName, pi.ProcessCode);
                Current.option.CurrentProcess = string.Format("{0},{1}", pi.ProcessName, pi.ProcessCode);
            }
            else
            {
                Error.Alert(msg);
                string[] s = Current.option.CurrentProcess.Split(',');
                lbProcessText = string.Format("{0}[{1}]", s[0], s[1]);
            }


            StationInfo si = Tafel.MES.MES.GetStationInfo(new IpAndProcess { IPAddress = ip, ProcessCode = pi.ProcessCode }, out msg);
            if (string.IsNullOrEmpty(msg))//成功获取到
            {
                lbStationText = string.Format("{0}[{1}]", si.StationName, si.StationCode);
                Current.option.CurrentStation = string.Format("{0},{1}", si.StationName, si.StationCode);
            }
            else
            {
                string[] s = Current.option.CurrentStation.Split(',');
                lbStationText = string.Format("{0}[{1}]", s[0], s[1]);
            }
        }

        public static bool GetUserName(string userNumber, string password, out string userName, out string msg)
        {
            Tafel.MES.MESUser user = new Tafel.MES.MESUser
            {
                UserNumber = userNumber,
                UserPwd = password,
                MachineNo = Current.feeder.Number,
                ProcessCode = Current.option.CurrentProcessCode,
                StationCode = Current.option.CurrentStationCode
            };
            return Tafel.MES.MES.CheckUser(user, out userName, out msg);
        }
        #endregion
    }
}
