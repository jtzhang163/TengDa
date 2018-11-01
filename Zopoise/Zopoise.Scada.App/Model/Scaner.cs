using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Configuration;
using System.Data.Entity;
using System.Text.RegularExpressions;
using System.Threading;
using TengDa;
using TengDa.Wpf;

namespace Zopoise.Scada.App
{
    /// <summary>
    /// 扫码枪
    /// </summary>
    [DisplayName("扫码枪")]
    public class Scaner : EthernetTerminal
    {

        private string code = "000000000000000000000000";
        [NotMapped,Browsable(false)]
        public string Code
        {
            get => code;
            set => SetProperty(ref code, value);
        }

        public Scaner() : this(-1)
        {

        }
        public Scaner(int id)
        {
            this.Id = Id;
        }


        public bool GetInfo()
        {
            var output = string.Empty;
            var msg = string.Empty;

            if (!GetInfo(Current.Option.TriggerScanCommand, out output, out msg))
            {
                Current.Cooler.ScanResult = ScanResult.Error;
                this.RealtimeStatus = msg;
                return false;
            }

            if (string.IsNullOrEmpty(output))
            {
                Current.Cooler.ScanResult = ScanResult.Error;
                this.RealtimeStatus = "指定时间未接收到串口数据！";
                return false;
            }

            var code = Regex.Match(output, Current.Option.BatteryCodeRegularExpression).Value;
            if (!string.IsNullOrEmpty(code))
            {
                Current.Cooler.ScanResult = ScanResult.OK;
                this.RealtimeStatus = code;

                if (!Current.Mes.IsOffline && Current.Mes.IsEnabled)
                {
                    if (!MES.CheckSfc(code, out msg))
                    {
                        OperationHelper.ShowTips(string.Format("MES检验失败，Code：{0}", code));
                        Current.Mes.RealtimeStatus = string.Format("MES检验失败，ID：{0}", code);
                        LogHelper.WriteError(string.Format("MES检验失败，Code：{0}", code));
                    }
                }

                Code = code;
                Current.Cooler.IsReadyScan = false;

                var t = new Thread(() => {
                    Thread.Sleep(2000);
                    this.RealtimeStatus = "等待扫码";
                });
                t.Start();

                var battery = new Battery
                {
                    Code = code,
                    ScanTime = DateTime.Now
                };
                Context.InsulationContext.Batteries.Add(battery);
                Context.InsulationContext.SaveChanges();
                InsulationData.Battery = battery;

                return true;

            }

            code = Regex.Match(output, Current.Option.ScanFailedReturnStr).Value;
            if (!string.IsNullOrEmpty(code))
            {
                Current.Cooler.ScanResult = ScanResult.NG;
                this.RealtimeStatus = "扫码失败！";
                Current.Cooler.IsReadyScan = false;
                return false;
            }

            msg = "扫码枪返回字符串无法识别！";
            Current.Cooler.ScanResult = ScanResult.Unknown;
            this.RealtimeStatus = msg + output;
            return false;
        }
    }

}
