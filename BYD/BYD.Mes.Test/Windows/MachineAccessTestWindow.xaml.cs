using BYD.Mes.Test.MesService;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Anchitech.Mes.Test.Windows
{
    /// <summary>
    /// MachineAccessTestWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MachineAccessTestWindow : Window
    {


        public MachineAccessTestWindow()
        {
            InitializeComponent();
        }


        private void btnUpload_Click(object sender, RoutedEventArgs e)
        {

            var service = new BYD.Mes.Test.MesService.AutoLineService();

            MySoapHelper header = new MySoapHelper();

            header.userName = "";
            header.passWord = "";

            service.MySoapHelperValue = header;

            try
            {
               var response = service.PassStationCheck("","","","");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ShowLog()
        {
        }
    }
}
