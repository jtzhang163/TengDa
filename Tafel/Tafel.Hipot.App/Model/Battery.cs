using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TengDa.Wpf;

namespace Tafel.Hipot.App
{
    public class Battery : Product
    {
        public Battery():this(-1)
        {

        }

        public Battery(int id)
        {
            this.Id = id;
        }

        public DateTime ScanTime { get; set; }
    }
}
