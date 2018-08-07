using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TengDa.Wpf;

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
