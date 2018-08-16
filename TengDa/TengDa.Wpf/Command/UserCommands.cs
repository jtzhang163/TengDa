using System.Windows.Input;

namespace TengDa.Wpf
{
    public class UserCommands
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