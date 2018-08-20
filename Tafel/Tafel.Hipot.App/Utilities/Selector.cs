using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using TengDa.Wpf;

namespace Tafel.Hipot.App.Utilities
{
    public class UserTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item != null && item is User)
            {
                var user = item as User;
                switch (user.Role.Level)
                {
                    case 4: return (container as FrameworkElement).FindResource("superAdminTemplate") as DataTemplate;
                    case 3: return (container as FrameworkElement).FindResource("adminTemplate") as DataTemplate;
                    case 2: return (container as FrameworkElement).FindResource("maintainerTemplate") as DataTemplate;
                    default: return (container as FrameworkElement).FindResource("operatorTemplate") as DataTemplate;
                }
            }
            return null;
        }
    }
}
