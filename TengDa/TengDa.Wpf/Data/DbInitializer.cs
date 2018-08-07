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
