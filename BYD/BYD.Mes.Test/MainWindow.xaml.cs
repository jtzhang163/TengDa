using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
using Anchitech.Mes.Test.Windows;

namespace Anchitech.Mes.Test
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void clearLog_Click(object sender, RoutedEventArgs e)
        {
            this.log.Text = "";
        }

        private void MachineAccessTest_Click(object sender, RoutedEventArgs e)
        {
            new MachineAccessTestWindow().Show();
        }

        private void JudgeSfcRepeatCode_Click(object sender, RoutedEventArgs e)
        {
            //new JudgeSfcRepeatCodeWindow().Show();
        }

        private void InSite_Click(object sender, RoutedEventArgs e)
        {
            //new BakingEnterSiteWindow().Show();
        }

        private void OutSite_Click(object sender, RoutedEventArgs e)
        {
            //new BakingOutSiteWindow().Show();
        }

        private void BakingNg_Click(object sender, RoutedEventArgs e)
        {
            //new BakingNgWindow().Show();
        }

        private void MachineStatus_Click(object sender, RoutedEventArgs e)
        {
            //new MachineStatusWindow().Show();
        }

    }
}
