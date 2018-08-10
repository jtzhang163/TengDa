using Fluent;
using System;
using System.Windows;

namespace Tafel.Hipot.App
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            Fluent.ThemeManager.ChangeAppTheme(this, "Silver");
            //Silver Blue Black
            base.OnStartup(e);
        }
        //protected override void OnStartup(StartupEventArgs e)
        //{

        //    //Tuple<AppTheme, Accent> appStyle = ThemeManager.DetectAppStyle(Application.Current);
        //    ThemeManager.ChangeAppStyle(Application.Current, ThemeManager.GetAccent("Blue"), ThemeManager.GetAppTheme("BaseDark"));
        //    ThemeManager.IsAutomaticWindowsAppModeSettingSyncEnabled = true;
        //    ThemeManager.SyncAppThemeWithWindowsAppModeSetting();
        //    base.OnStartup(e);
        //    ScreenTip.HelpPressed += OnScreenTipHelpPressed;

        //    //You can choose between these available color schemes:
        //    //“Red”, “Green”, “Blue”, “Purple”, “Orange”, “Lime”, “Emerald”, “Teal”, “Cyan”, “Cobalt”, “Indigo”, “Violet”, “Pink”, “Magenta”, “Crimson”, “Amber”, “Yellow”, “Brown”, “Olive”, “Steel”, “Mauve”, “Taupe”, “Sienna”
        //}

        /// <summary>
        /// Handles F1 pressed on ScreenTip with help capability
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Arguments</param>
        static void OnScreenTipHelpPressed(object sender, ScreenTipHelpEventArgs e)
        {
            // Show help according the given help topic
            // (here just show help topic as string)
            MessageBox.Show(e.HelpTopic.ToString());
        }
    }
}
