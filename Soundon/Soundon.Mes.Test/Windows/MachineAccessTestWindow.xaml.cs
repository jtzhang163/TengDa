using Soundon.Mes.Test.MachineAccessTestServiceService;
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
using System.Windows.Shapes;

namespace Soundon.Mes.Test.Windows
{
    /// <summary>
    /// MachineAccessTestWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MachineAccessTestWindow : Window
    {

        private const string URL_TEST = "http://10.10.156.11:50000/sapdevwebservice/MachineAccessTestServiceService?wsdl";
        private const string URL_NORMAL = "http://10.10.180.13:50000/sapdevwebservice/MachineAccessTestServiceService?wsdl";

        private string Url = URL_TEST;


        public MachineAccessTestWindow()
        {
            InitializeComponent();
            this.url.Text = this.Url;
        }


        private void btnUpload_Click(object sender, RoutedEventArgs e)
        {

            var client = new MachineAccessTestServiceClient();

            var url = "http://10.10.156.11:50000/sapdevwebservice/MachineAccessTestServiceService?wsdl";
            client.Endpoint.Address = new System.ServiceModel.EndpointAddress(url);

            client.ClientCredentials.UserName.UserName = "wcw";
            client.ClientCredentials.UserName.Password = "garen105778";

            //sapint sap12345
            //wcw garen105778

            try
            {
                var request = new machineAccessTestRequest()
                {
                    site = "1003",                      //this.site.Text.Trim(),
                    resource = "3CKX0001"               //this.resource.Text.Trim()
                };

                var response = client.getResourceDescription(new getResourceDescription() { pRequest = request });
                this.status.Text = response.@return.status;
                this.message.Text = response.@return.message;
                this.description.Text = response.@return.description;
            }
            catch(Exception ex)
            {
                this.status.Text = "ERROR";
                this.message.Text = ex.Message;
                this.description.Text = ex.ToString();
            }

        }

        private void RadioButton_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (this.url == null) return;

            var radioButton = sender as RadioButton;
            if (!radioButton.IsChecked.Value) return;

            this.Url = radioButton.Content.ToString() == "测试" ? URL_TEST : URL_NORMAL;
            this.url.Text = this.Url;
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            this.status.Text = "";
            this.message.Text = "";
            this.description.Text = "";
        }
    }
}
