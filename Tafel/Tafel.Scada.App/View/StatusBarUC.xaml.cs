using System;
using System.Windows.Controls;
using System.Windows.Input;

namespace Zopoise.Scada.App.View
{
    /// <summary>
    /// StatusBarUC.xaml 的交互逻辑
    /// </summary>
    public partial class StatusBarUC : UserControl
    {
        public StatusBarUC()
        {
            InitializeComponent();
            Init();
        }

        System.Timers.Timer timerUpdateTime = null;
        private void Init()
        {

            //当前时间显示
            statusBar.DataContext = AppCurrent.AppViewModel;
            timerUpdateTime = new System.Timers.Timer(1000);
            timerUpdateTime.Elapsed += delegate
            {
                AppCurrent.AppViewModel.TimeNow = DateTime.Now;
            };
            timerUpdateTime.Start();
        }

        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            AppCurrent.AppViewModel.MainWindowsBackstageIsOpen = true;
        }
    }
}
