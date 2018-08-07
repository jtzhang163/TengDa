using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TengDa.Wpf
{
    public static class UserCommands
    {
        private static RoutedUICommand login;
        public static ICommand Login
        {
            get
            {
                if (login == null)
                {
                    login = new RoutedUICommand("login", "Login", typeof(UserCommands));
                    login.InputGestures.Add(new KeyGesture(Key.I, ModifierKeys.Alt));
                }
                return login;
            }
        }
    }
}
