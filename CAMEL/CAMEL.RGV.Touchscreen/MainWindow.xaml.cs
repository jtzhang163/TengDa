using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace CAMEL.RGV.Touchscreen
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = Current.Option;
            Init();
        }


        private void Init()
        {
            Current.RGV.IP = Current.Option.RGV1_IP;
            Current.RGV.Port = 9600;

            
        }

        private static void ReadRgvInfoTimerInvokeFunc(object obj)
        {
            Current.RGV.GetInfo();
        }

        private Timer ReadRgvInfoTimeTimer = new Timer(new TimerCallback(ReadRgvInfoTimerInvokeFunc), null, 2000, 1000);

        private Timer RefreshCurrentTimeTimer = new Timer(new TimerCallback(RefreshCurrentTime), null, 0, 1000);

        private static void RefreshCurrentTime(object o)
        {
            Current.Option.CurrentTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }
    }
}
