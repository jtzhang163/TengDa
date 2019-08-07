using CAMEL.Mes.Test.MesService;
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

namespace CAMEL.Mes.Test.Windows
{
    /// <summary>
    /// MachineAccessTestWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MachineAccessTestWindow : Window
    {

        private string Url = Common.MachineAccess_URL_TEST;

        public MachineAccessTestWindow()
        {
            InitializeComponent();
            this.url.Text = this.Url;
        }


        private void btnUpload_Click(object sender, RoutedEventArgs e)
        {

            var wsProxy = new MesService.EquipService();
            //wsProxy.Credentials = new NetworkCredential(Common.Username, Common.Password, null);
            //wsProxy.PreAuthenticate = true;
            //wsProxy.Timeout = 2000;
            //wsProxy.Url = this.Url;

            try
            {

                var response1 = wsProxy.TestConnection();

                var request2 = "";
                //{"Barcode":"36ANCCB23140160N18E01C18E04H1000784","MachineCode":"BK02-04-01","TrayNo":"","StartTime":"2019\/6\/21 14:19:12","EndTime":"2019\/6\/21 14:19:12","Temperature":92.3,"Vacuum":12.3}
                var response2 = wsProxy.UploadBakingData(request2);

                var request3 = "";
                //[{"MachCode":"BK01-04-02","MachStatus":"99","StepProdLotNo":null,"MachTrouble":null}]
                //[{"MachCode":"BK01-04-02","MachStatus":"99","StepProdLotNo":null,"MachTrouble":null},{"MachCode":"BK01-04-01","MachStatus":"99","StepProdLotNo":null,"MachTrouble":null}]
                var response3 = wsProxy.UploadMultiMachStateListInfo(request3);
            }
            catch (Exception ex)
            {

            }
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (this.url == null) return;

            var radioButton = sender as RadioButton;
            if (!radioButton.IsChecked.Value) return;

            this.Url = radioButton.Content.ToString() == "测试" ? Common.MachineAccess_URL_TEST : Common.MachineAccess_URL_NORMAL;
            this.url.Text = this.Url;
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            this.status.Text = "";
            this.message.Text = "";
            this.description.Text = "";
        }

        private void ShowLog()
        {
            Common.WriteLog(string.Format("==== 上位机访问测试接口 == time：{0:yyyy-MM-dd HH:mm:ss} ====", DateTime.Now));
            Common.WriteLog("--- 参数 ----");
            Common.WriteLog(string.Format("服务地址 ： {0}", this.Url));
            Common.WriteLog(string.Format("站点 ： {0}", this.site.Text));
            Common.WriteLog(string.Format("设备编号 ： {0}", this.resource.Text));
            Common.WriteLog("--- 结果 ----");
            Common.WriteLog(string.Format("状态 ： {0}", this.status.Text));
            Common.WriteLog(string.Format("消息 ： {0}", this.message.Text));
            Common.WriteLog(string.Format("设备描述信息 ： {0}", this.description.Text));
            Common.WriteLog("");
        }
    }
}
