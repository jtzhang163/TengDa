using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TengDa;

namespace Anchitech.Baking.Controls
{
    public partial class FeedClampUC : UserControl
    {
        private Station station;
        public FeedClampUC()
        {
            InitializeComponent();
        }

        public void Init(Station station)
        {
            this.lbClampCode.Text = station.Clamp.Code;
            this.station = station;
        }

        public void Update(Station station)
        {

            lbClampCode.Text = station.Clamp.Code;

            lbClampCode.BackColor = station.HasSampleFlag ? Color.Blue : Color.White;
            lbClampCode.ForeColor = station.HasSampleFlag ? Color.White : Color.Blue;

            bool canChangeVisible = DateTime.Now.Second % 3 == 1;

            if (Current.Feeder.IsAlive && canChangeVisible && station.Id == Current.Task.FromStationId/* && (Current.Task.Status == TaskStatus.就绪 || Current.Task.Status == TaskStatus.可取 || Current.Task.Status == TaskStatus.正取)*/)
            {
                this.Visible = false;
            }
            else if (Current.Feeder.IsAlive && canChangeVisible && station.Id == Current.Task.ToStationId)
            {
                this.Visible = true;
                tlpFeederStationClamp.BackColor = Current.Task.FromClampStatus == ClampStatus.空夹具 ? Color.Cyan : Color.LimeGreen;
            }
            else
            {
                this.Visible = station.ClampStatus != ClampStatus.无夹具;

                if (!station.IsAlive)
                {
                    this.BackColor = SystemColors.Control;
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

        private void TlpFeederStationClamp_CellPaint(object sender, TableLayoutCellPaintEventArgs e)
        {

            Graphics g = e.Graphics;
            Rectangle r = e.CellBounds;
            Brush brush = Brushes.Cyan;
            if (!(this.station.IsAlive || Current.Feeder.Plc.PreIsAlive))//|| Current.Feeder.Plc.PreIsAlive防闪烁
            {
                brush = Brushes.WhiteSmoke;
            }
            else if (this.station.ClampStatus == ClampStatus.满夹具)
            {
                brush = Brushes.LimeGreen;
            }
            else if (this.station.ClampStatus == ClampStatus.空夹具)
            {
                brush = Brushes.Cyan;
                if (this.station.Id == Current.Feeder.CurrentPutStationId)
                {
                    for (int x = 0; x < 3; x++)
                    {
                        for (int y = 0; y < 20; y++)
                        {
                            if (e.Row == y && e.Column == x)
                            {

                                if (y + x * 20 < Current.Feeder.CurrentBatteryCount)
                                {
                                    brush = Brushes.LimeGreen;
                                }

                                //g.FillRectangle(brush, r);
                            }
                        }
                    }
                }
            }
            g.FillRectangle(brush, r);
            //tableLayoutPanel1.GetType().GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).SetValue(tableLayoutPanel1, true, null); 

        }

        public void InvalidateShowBatteryCount()
        {
            this.tlpFeederStationClamp.Invalidate();
        }
    }
}
