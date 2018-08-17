using System.Configuration;

namespace Jtzhang.Helper.TcpServer
{
    public class AppHelper
    {
        public static void SetConfig(string key,string value)
        {
            Configuration cfa = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            cfa.AppSettings.Settings[key].Value = value;
            cfa.Save();
        }
    }
}
