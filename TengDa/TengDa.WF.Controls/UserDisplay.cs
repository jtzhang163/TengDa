using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TengDa.WF.Controls
{
    public partial class UserDisplay : UserControl
    {
        public UserDisplay()
        {
            InitializeComponent();
        }
        public void DisplayUserInfo()
        {
            if (string.IsNullOrEmpty(TengDa.WF.Current.user.Number))
            {
                lbUserName.Text = TengDa.WF.Current.user.Name;
            }
            else
            {
                lbUserName.Text = string.Format("{0}[{1}]", TengDa.WF.Current.user.Name, TengDa.WF.Current.user.Number);
            }
            lbUserGroupName.Text = TengDa.WF.Current.user.Group.Name;
        }
    }
}
