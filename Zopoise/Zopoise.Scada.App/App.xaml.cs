using Fluent;
using System.Windows;

namespace Zopoise.Scada.App
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        //protected override void OnStartup(StartupEventArgs e)
        //{
        //  Fluent.ThemeManager.ChangeAppTheme(this, "Black");
        // // base.OnStartup(e);
        //}
        protected override void OnStartup(StartupEventArgs e)
        {

            //Tuple<AppTheme, Accent> appStyle = ThemeManager.DetectAppStyle(Application.Current);
            //ThemeManager.ChangeAppStyle(Application.Current, ThemeManager.GetAccent("Blue"), ThemeManager.GetAppTheme("BaseLight"));
            ThemeManager.IsAutomaticWindowsAppModeSettingSyncEnabled = true;
            ThemeManager.SyncAppThemeWithWindowsAppModeSetting();
            base.OnStartup(e);
            ScreenTip.HelpPressed += OnScreenTipHelpPressed;
        }

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
