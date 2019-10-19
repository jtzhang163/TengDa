using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAMEL.RGV.Touchscreen
{
   public class ExternalProgram
   {
        Process process;
        public  bool StartProgram()
        {
            string path = string.Format(System.IO.Directory.GetCurrentDirectory() + @"\{0}.exe",Current.Option.Program_Name);
            if (System.IO.File.Exists(path))
            {
                try
                { 
                    if (process==null)
                    {
                        //if (!process.HasExited)
                        //{

                        //}
                       process=Process.Start(path);
                    }
                    return true;
                }
                catch (Exception e)
                {
                }
            }
                return false;
        }
   }
}
