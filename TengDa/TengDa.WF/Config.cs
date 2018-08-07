using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace TengDa.WF
{
    public class Config
    {
        private static string dbTableNamePre = string.Empty;
        public static string DbTableNamePre
        {
            get
            {
                if (string.IsNullOrEmpty(dbTableNamePre))
                {
                    dbTableNamePre = ConfigurationManager.AppSettings["DbTableNamePre"];
                }
                return dbTableNamePre;
            }
            set
            {
                dbTableNamePre = value;
            }
        }
    }
}
