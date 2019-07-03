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
    public partial class CacherUC : UserControl
    {
        public CacherUC()
        {
            InitializeComponent();
        }

        public void Init(Cacher cacher)
        {
            this.lbName.Text = cacher.Name;
            this.simpleClampUC1.Init(cacher.Stations[0]);
            this.simpleClampUC2.Init(cacher.Stations[1]);
            this.simpleClampUC3.Init(cacher.Stations[2]);
        }

        public void Update(Cacher cacher)
        {
            cacher.IsAlive = cacher.IsEnable;
            //cacher.IsAlive = cacher.IsEnable && cacher.Plc.IsAlive;
            cacher.Stations.ForEach(s => s.IsAlive = s.IsEnable && cacher.IsAlive);
            this.BackColor = cacher.IsAlive ? Color.White : Color.LightGray;
            this.simpleClampUC1.Update(cacher.Stations[0]);
            this.simpleClampUC2.Update(cacher.Stations[1]);
            this.simpleClampUC3.Update(cacher.Stations[2]);
        }

    }
}
