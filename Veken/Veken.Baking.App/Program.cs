using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Veken.Baking.App
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new LoginForm());
            Application.Run(new ManageSystem());
            // Application.Run(new InOvenForm());
        }
    }
}