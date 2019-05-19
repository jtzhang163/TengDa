using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.IO;

namespace Soundon.Mes.Test
{
    public class Common
    {
        private static string username;
        public static string Username
        {
            get
            {
                if (string.IsNullOrEmpty(username))
                {
                    username = ConfigurationManager.AppSettings["username"].ToString();
                }
                return username;
            }
        }


        private static string password;
        public static string Password
        {
            get
            {
                if (string.IsNullOrEmpty(password))
                {
                    password = ConfigurationManager.AppSettings["password"].ToString();
                }
                return password;
            }
        }

        public static string MachineAccess_URL_TEST = ConfigurationManager.AppSettings["MachineAccess_URL_TEST"].ToString();
        public static string MachineAccess_URL_NORMAL = ConfigurationManager.AppSettings["MachineAccess_URL_NORMAL"].ToString();

        public static string Executing_URL_TEST = ConfigurationManager.AppSettings["Executing_URL_TEST"].ToString();
        public static string Executing_URL_NORMAL = ConfigurationManager.AppSettings["Executing_URL_NORMAL"].ToString();

        public static MainWindow MainWindow;

        public static void WriteLog(string log)
        {
            File.AppendAllText("测试日志.log", log + "\r\n");
            MainWindow.log.Text += log + "\r\n";
        }
    }
}
