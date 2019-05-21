using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Anchitech.Baking.Controls
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
            this.lbClampCode.Text = station.Clamp.Code;

            //var sampleStatusFlag = Current.Transfer.Station.SampleStatus == SampleStatus.待测试 ? "" : Current.Transfer.Station.SampleStatus == SampleStatus.测试OK ? "✔ " : "✘ ";

            bool canChangeVisible = DateTime.Now.Second % 3 == 1;

            if (station.IsAlive && canChangeVisible && station.Id == Current.Task.FromStationId && (Current.Task.Status == TaskStatus.就绪 || Current.Task.Status == TaskStatus.可取 || Current.Task.Status == TaskStatus.正取))
            {
                this.LabelClampCode.Visible = false;
            }
            else if (station.IsAlive && canChangeVisible && station.Id == Current.Task.ToStationId)
            {
                this.LabelClampCode.Visible = true;
                this.LabelClampCode.BackColor = Current.Task.FromClampStatus == ClampStatus.空夹具 ? Color.Cyan : Color.LimeGreen;
            }
            else
            {

                if (!station.IsAlive)
                {
                    this.LabelClampCode.Visible = true;
                    this.LabelClampCode.BackColor = Color.LightGray;
                }
                else
                {
                    this.LabelClampCode.Visible = station.ClampStatus != ClampStatus.无夹具;
                    switch (station.ClampStatus)
                    {
                        case ClampStatus.满夹具: this.LabelClampCode.BackColor = Color.LimeGreen; break;
                        case ClampStatus.空夹具: this.LabelClampCode.BackColor = Color.Cyan; break;
                        case ClampStatus.异常: this.LabelClampCode.BackColor = Color.Red; break;
                        default: this.LabelClampCode.BackColor = SystemColors.Control; break;
                    }
                }
            }
        }
    }
}
