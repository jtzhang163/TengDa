using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using TengDa;

namespace CAMEL.Baking.Control
{
    public partial class ShowTandVForm : Form
    {
        private Oven oven;
        private Label[,] lbTemp = new Label[5,3];
        public ShowTandVForm(Oven oven)
        {
            InitializeComponent();

            this.oven = oven;

            this.Text = this.oven.Name + " 实时温度";
            for (int i = 0; i < this.oven.Floors.Count; i++)
            {
                for (int j = 0; j < Option.TemperaturePointCount; j++)
                {
                    lbTemp[i, j] = (Label)(this.Controls.Find(string.Format("lbTemp{0:D2}{1:D2}", i + 1, j + 1), true)[0]);
                }
            }

            Timer1_Tick(null, null);
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < this.oven.Floors.Count; i++)
            {
                for (int j = 0; j < Option.TemperaturePointCount; j++)
                {
                    lbTemp[i, j].Text = this.oven.Floors[i].Temperatures[j].ToString("#00.0") + "℃";
                }
            }
        }
    }
}
