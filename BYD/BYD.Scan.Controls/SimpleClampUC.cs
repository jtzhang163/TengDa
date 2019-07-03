using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BYD.Scan.Controls
{
    public partial class SimpleClampUC : UserControl
    {
        public SimpleClampUC()
        {
            InitializeComponent();
        }

        public void Init(Station station)
        {
            this.lbClampCode.Text = station.Clamp.Code;
        }

        public void Update(Station station)
        {

            this.BackColor = station.IsEnable ? Color.Transparent : Color.LightGray;

            this.lbClampCode.Text = station.Clamp.Code;

            //var sampleStatusFlag = Current.Transfer.Station.SampleStatus == SampleStatus.待测试 ? "" : Current.Transfer.Station.SampleStatus == SampleStatus.测试OK ? "✔ " : "✘ ";

            bool canChangeVisible = DateTime.Now.Second % 3 == 1;

            if (station.IsAlive && canChangeVisible && station.Id == Current.Task.FromStationId/* && (Current.Task.Status == TaskStatus.就绪 || Current.Task.Status == TaskStatus.可取 || Current.Task.Status == TaskStatus.正取)*/)
            {
                this.lbClampCode.Visible = false;
            }
            else if (station.IsAlive && canChangeVisible && station.Id == Current.Task.ToStationId)
            {
                this.lbClampCode.Visible = true;
                this.lbClampCode.BackColor = Current.Task.FromClampStatus == ClampStatus.空夹具 ? Color.Cyan : Color.LimeGreen;
            }
            else
            {

                if (!station.IsAlive)
                {
                    this.BackColor = Color.LightGray;
                }
                else
                {
                    this.lbClampCode.Visible = station.ClampStatus != ClampStatus.无夹具;
                    switch (station.ClampStatus)
                    {
                        case ClampStatus.满夹具: this.lbClampCode.BackColor = Color.LimeGreen; break;
                        case ClampStatus.空夹具: this.lbClampCode.BackColor = Color.Cyan; break;
                        case ClampStatus.异常: this.lbClampCode.BackColor = Color.Red; break;
                        default: this.lbClampCode.BackColor = SystemColors.Control; break;
                    }
                }
            }
        }
    }
}
