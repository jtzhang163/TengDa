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
    public partial class PageMain2UC : UserControl
    {
        private const int OvenCount = 29;
        private const int OvenFloorCount = 5;
        private OvenUC[] ovenUCs = new OvenUC[OvenCount];
        public PageMain2UC()
        {
            InitializeComponent();
        }

        public void Init()
        {

            if (System.Windows.SystemParameters.PrimaryScreenHeight > 800)
            {
                this.tlpOvenCol1.Margin = new Padding(3, 50, 3, 3);
                this.tlpOvenCol2.Margin = new Padding(3, 3, 3, 50);
            }

            #region 烤箱相关控件数组

            for (int i = 0; i < OvenCount; i++)
            {
                ovenUCs[i] = (OvenUC)(this.Controls.Find(string.Format("ovenUC{0}", (i + 1).ToString("D3")), true)[0]);
            }

            #endregion

            this.feederUC1.Init(Current.Feeder);

            for (int i = 0; i < OvenCount; i++)
            {
                this.ovenUCs[i].Init(Current.ovens[i]);
            }

            this.robotUC1.Init(Current.RGV);
        }

        public void RefreshUI()
        {
            this.feederUC1.Update(Current.Feeder);
            this.taskInfo1.UpdateInfo();

            #region 烤箱

            for (int i = 0; i < OvenCount; i++)
            {
                this.ovenUCs[i].UpdateUI();
            }

            #endregion

            #region RGV

            this.robotUC1.Update(Current.RGV);
            this.panelRGV.Padding = new Padding(Current.RGV.Position + 3, 3, 0, 3);

            #endregion
        }

        public void OvenInvalidate(int i, int j)
        {
            this.ovenUCs[i].Invalidate(j);
        }

    }
}
