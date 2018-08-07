using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TengDa;
using TengDa.Encrypt;
using TengDa.WF;

namespace Veken.Baking.App
{
    public partial class MesOfflineVerify : Form
    {
        public MesOfflineVerify()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            string name = this.tbName.Text.Trim();
            string pwd = this.tbPwd.Text.Trim();
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(pwd))
            {
                Tip.Alert("用户名或密码为空");
                return;
            }

            User user = new User();
            string msg = string.Empty;
            List<User> list = User.GetList(string.Format("SELECT * FROM [dbo].[TengDa.Users] WHERE [Name] = '{0}' AND [Password] = '{1}'", name, Base64.EncodeBase64(pwd)), out msg);
            if (list.Count() < 1)
            {
                Tip.Alert("用户名或密码错误");
                return;
            }

            List<User> listManagers = list.Where(u => u.Group.Level > 2).ToList();
            if (listManagers.Count < 1)
            {
                Tip.Alert(string.Format("用户 {0} 权限不足！", name));
                return;
            }
            user = listManagers[0];
            if (!user.IsEnable)
            {
                Tip.Alert("该用户尚未审核");
                return;
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
