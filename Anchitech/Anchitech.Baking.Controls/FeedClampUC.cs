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
    public partial class FeedClampUC : UserControl
    {
        public FeedClampUC()
        {
            InitializeComponent();
        }

        public void Init(Station station)
        {
            this.lbClampCode.Text = station.Clamp.Code;
        }

        public void Update(Station station)
        {

            lbClampCode.Text = station.Clamp.Code;

            lbClampCode.BackColor = station.HasSampleFlag ? Color.Blue : Color.White;
            lbClampCode.ForeColor = station.HasSampleFlag ? Color.White : Color.Blue;

            bool canChangeVisible = DateTime.Now.Second % 3 == 1;

            if (Current.Feeder.IsAlive && canChangeVisible && station.Id == Current.Task.FromStationId && (Current.Task.Status == TaskStatus.就绪 || Current.Task.Status == TaskStatus.可取 || Current.Task.Status == TaskStatus.正取))
            {
                tlpFeederStationClamp.Visible = false;
            }
            else if (Current.Feeder.IsAlive && canChangeVisible && station.Id == Current.Task.ToStationId)
            {
                tlpFeederStationClamp.Visible = true;
                tlpFeederStationClamp.BackColor = Current.Task.FromClampStatus == ClampStatus.空夹具 ? Color.Cyan : Color.LimeGreen;
            }
            else
            {
                tlpFeederStationClamp.Visible = station.ClampStatus != ClampStatus.无夹具;

                if (!station.IsAlive)
                {
                    tlpFeederStationClamp.BackColor = SystemColors.Control;
                }
                else
                {
                    switch (station.ClampStatus)
                    {
                        case ClampStatus.满夹具: tlpFeederStationClamp.BackColor = Color.LimeGreen; break;
                        case ClampStatus.空夹具: tlpFeederStationClamp.BackColor = Color.Cyan; break;
                        case ClampStatus.异常: tlpFeederStationClamp.BackColor = Color.Red; break;
                        default: tlpFeederStationClamp.BackColor = SystemColors.Control; break;
                    }
                }
            }
        }
    }
}
