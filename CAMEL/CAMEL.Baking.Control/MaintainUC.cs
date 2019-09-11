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
    public partial class MaintainUC : UserControl
    {
        public MaintainUC()
        {
            InitializeComponent();

        }
        Label[] labels = new Label[3];
        public void Init()
        {
            //初始化控件
            for (int i = 0; i < labels.Length; i++)
            {
                labels[i] = (Label)Controls.Find("Clear" + (i + 1), true)[0];
                labels[i].Text = Current.maintains[i].Name;
                labels[i].Visible = false;
                Maintain maintains = Current.maintains.FirstOrDefault(a => a.Name.Contains(labels[i].Text));
                //labels[i].Tag = maintains.Id;
            }
        }
        public void UpdateUI()
        {

            for (int i = 0; i < Current.maintains.Count; i++)
            {
                var lable = labels[i];
                var maintain = Current.maintains[i];
                JudgeTime(maintain);
                labels[i].Visible = !maintain.IsCleared;
            }
        }

        public void JudgeTime(Maintain maintain)
        {
            if (DateTime.Now > maintain.ClocKtime.AddDays(maintain.TipNumber) && maintain.IsCleared)
            {
                Current.maintains.First(a => a.Id == maintain.Id).IsCleared = false;
            }
        }


        private void Clear_CLick(object sender, EventArgs e)
        {
            Label label = (Label)sender;
            DialogResult result = MessageBox.Show(string.Format("你要结束本次提示{0}吗？", label.Text.ToString()), "维护提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (DialogResult.No == result)
            {
                return;
            }
            Maintain maintain = Current.maintains.First(a => a.Name == label.Text);
            maintain.ClocKtime = DateTime.Now;
            maintain.IsCleared = true;
            label.Visible = false;
        }
    }
}
