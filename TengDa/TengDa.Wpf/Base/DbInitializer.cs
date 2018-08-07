using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace TengDa.Wpf
{
    public class DbInitializer
    {
        public virtual void Initialize()
        {
            Database.SetInitializer(new UserInitializer());
            Database.SetInitializer(new OptionInitializer());
            Database.SetInitializer(new YieldInitializer());
        }
    }
}
