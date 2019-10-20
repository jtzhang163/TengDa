using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CAMEL.Baking.Control
{
    public partial class ShowFloorFinishUC : UserControl
    {
        private string ShowInfo = "";
        public ShowFloorFinishUC()
        {
            InitializeComponent();
        }

        public void Init()
        {
            this.lbBakingFinishFloors.Text = "";
        }

        public void UpdateUI()
        {
            var floorNames = new StringBuilder();
            Current.ovens.ForEach(o =>
            {
                o.Floors.Where(f => f.RunRemainMinutes == 0 || f.RunMinutes == 0).ToList().ForEach(f =>
                {
                    if (f.Stations.Count(s => s.FloorStatus == FloorStatus.待出) > 0)
                    {
                        floorNames.Append(f.Name + ",");
                    }
                });
            });


            var showInfo = floorNames.ToString().Trim(',');

            var length = this.lbBakingFinishFloors.Text.Length;
            if (showInfo != this.ShowInfo)
            {
                this.ShowInfo = showInfo;
                this.lbBakingFinishFloors.Text = this.ShowInfo + " ";
            }
            else if (length > 1)
            {
                var text = this.lbBakingFinishFloors.Text;
                this.lbBakingFinishFloors.Text = text.Substring(1, length - 1) + text.Substring(0, 1);
            }
            else
            {
                this.lbBakingFinishFloors.Text = "";
            }
        }
    }
}
