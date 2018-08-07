using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TengDa.Encrypt;

namespace TengDa.Wpf
{
    public class UserViewModel
    {
        public static bool Login(string name, string password)
        {
            var user = new User();
            using (var data = new UserContext())
            {
                var entityPassword = Base64.EncodeBase64(password);
                user = data.Users.FirstOrDefault(u => u.Name == name && u.Password == entityPassword) ?? new User();
                user.LastLoginTime = DateTime.Now;
                user.LoginTimes++;
                data.SaveChanges();
            }
            Current.User = user;
            return user.Id > 0;
        }

        public static bool Logout()
        {
            Current.User = new User(-1);
            return true;
        }
    }
}