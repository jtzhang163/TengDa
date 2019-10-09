using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Soundon.Dispatcher.App
{
    /// <summary>
    /// 程序版本
    /// </summary>
    public class Version
    {
        public static string AssemblyVersion
        {
            get
            {
                string assemblyVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
                //assemblyVersion = Regex.Match(assemblyVersion, @"^[\d]+.[\d]+.[\d]+.[\d]+").Value;
                assemblyVersion = Regex.Match(assemblyVersion, @"^[\d]+.[\d]+.[\d]+").Value;
                //只获取主版本和次版本
                //return assemblyVersion;
                return "V" + assemblyVersion;
            }
        }

        public DateTime VersionTime
        {
            get
            {
                return System.IO.File.GetLastWriteTime(this.GetType().Assembly.Location);
            }
        }
    }
}
