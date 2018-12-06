using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TengDa;
using TengDa.Wpf;

namespace Zopoise.Scada.App
{
    /// <summary>
    /// 控制器（PLC）
    /// </summary>
    [DisplayName("控制器（PLC）")]
    public class Controller : Terminal
    {
        [DisplayName("PLC ID")]
        public int PlcId { get; set; }

        [Browsable(false)]
        public virtual PLC PLC { get; set; }

        /// <summary>
        /// 准备好扫码，防止重复扫码
        /// </summary>
        public bool IsReadyScan = false;

        /// <summary>
        /// 可扫码
        /// </summary>
        public bool CanScan
        {
            set
            {
                if(!canScan && value)
                {
                    IsReadyScan = true;
                }
                canScan = value;
            }
        }
        
        private bool canScan = false;

        public ScanResult ScanResult = ScanResult.Unknown;

        public bool GetInfo()
        {
            if (!this.PLC.IsPingSuccess)
            {
                this.IsAlive = false;
                LogHelper.WriteError("无法连接到 " + this.PLC.IP);
                return false;
            }

            string msg = string.Empty;
            string output = string.Empty;
            string input = string.Empty;
            try
            {
                #region 获取是否可扫码信号

                output = string.Empty;
                if (!this.PLC.GetInfo(false, Current.Option.GetIsReadyScanCommand, out output, out msg))
                {
                    Error.Alert(msg);
                    this.IsAlive = false;
                    return false;
                }

                if (output.Substring(3, 1) != "$")
                {
                    LogHelper.WriteError(string.Format("与PLC通信格式错误，input：{0}，output：{1}", Current.Option.GetIsReadyScanCommand, output));
                    return false;
                }

                //%01$RC120
                this.CanScan = output.Trim().Substring(6, 1) == "1";

                #endregion


                #region 控制开门

                if (this.ScanResult == ScanResult.OK || this.ScanResult == ScanResult.NG)
                {
                    var command = this.ScanResult == ScanResult.OK ? Current.Option.SetScanOKCommand : Current.Option.SetScanNGCommand;
                    output = string.Empty;
                    if (!this.PLC.GetInfo(false, command, out output, out msg))
                    {
                        Error.Alert(msg);
                        this.IsAlive = false;
                        return false;
                    }

                    if (output.Substring(3, 1) != "$")
                    {
                        LogHelper.WriteError(string.Format("与PLC通信格式错误，input：{0}，output：{1}", command, output));
                        return false;
                    }
                    LogHelper.WriteInfo(string.Format("成功发送指令到{0}:{1}", this.Name, command));
                    this.ScanResult = ScanResult.Unknown;
                }
                #endregion

                this.RealtimeStatus = IsReadyScan ? "可扫码" : "等待可扫码信号";
            }
            catch (Exception ex)
            {
                Error.Alert(ex);
                this.IsAlive = false;
                return false;
            }

            this.IsAlive = true;
            return true;
        }
    }
}
