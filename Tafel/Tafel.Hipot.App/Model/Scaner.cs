using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Configuration;
using System.Data.Entity;
using System.Threading;
using TengDa.Wpf;

namespace Tafel.Hipot.App
{
    /// <summary>
    /// 扫码枪
    /// </summary>
    [DisplayName("扫码枪")]
    public class Scaner : EthernetTerminal
    {

        public Scaner() : this(-1)
        {

        }
        public Scaner(int id)
        {
            this.Id = Id;
        }


        public void GetInfo()
        {

        }
    }

}
