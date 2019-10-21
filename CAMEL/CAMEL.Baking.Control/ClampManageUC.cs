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
using TengDa.WF;

namespace CAMEL.Baking.Control
{
    public partial class ClampManageUC : UserControl
    {
        public ClampManageUC()
        {
            InitializeComponent();

        }

        private Station OvenStation;

        public void Init()
        {
            Current.ovens.ForEach(o =>
            {
                this.cbOvens.Items.Add(o.Name);
            });
            this.cbOvens.SelectedIndex = 0;

            Current.ovens[0].Floors.ForEach(f => f.Stations.ForEach(s =>
            {
                this.cbOvenStations.Items.Add(s.Name);
            }));
            this.cbOvenStations.SelectedIndex = 0;
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            var oldCode = this.lbClampCode.Text.Trim();
            var newCode = this.tbClampCodeNew.Text.Trim();
            if (oldCode == newCode)
            {
                return;
            }
            if (string.IsNullOrEmpty(newCode))
            {
                this.OvenStation.ClampId = -1;
                this.lbClampCode.Text = "";
                MessageBox.Show("成功清除该工位夹具！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var clamps = Clamp.GetList(string.Format("SELECT TOP 1 * FROM [CAMEL.Clamp] WHERE Code = '{0}' ORDER BY [Id] DESC", newCode), out string msg);
            if (clamps.Count == 0)
            {
                MessageBox.Show(string.Format("系统中不存在夹具：{0}", newCode), "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            this.OvenStation.ClampId = clamps[0].Id;
            this.lbClampCode.Text = newCode;
            MessageBox.Show(string.Format("成功设置工位夹具为：{0}", newCode), "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void CbOvens_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.cbOvenStations.Items.Clear();
            Current.ovens[(sender as ComboBox).SelectedIndex].Floors.ForEach(f => f.Stations.ForEach(s =>
            {
                this.cbOvenStations.Items.Add(s.Name);
            }));
            this.cbOvenStations.SelectedIndex = 0;
        }

        private void CbOvenStations_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.OvenStation = Station.StationList.Single(s => s.Name == (sender as ComboBox).SelectedItem.ToString());
            this.lbClampCode.Text = this.OvenStation.Clamp.Code;
            this.tbClampCodeNew.Text = this.OvenStation.Clamp.Code;
        }
    }
}
