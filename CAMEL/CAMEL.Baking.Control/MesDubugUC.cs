using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace CAMEL.Baking.Control
{
    public partial class MesDubugUC : UserControl
    {
        public MesDubugUC()
        {
            InitializeComponent();
        }

        public void Init()
        {
            this.cbInterfaceName.Items.AddRange(new string[] {
                "身份验证接口",
                "电芯与托盘信息查询接口",
                "记录设备状态接口",
                "二次高温数据上传接口"
            });
            
            this.cbInterfaceName.SelectedIndex = 0;
        }

        private void BtnUpload_Click(object sender, EventArgs e)
        {
            var selectName = this.cbInterfaceName.SelectedItem.ToString();
            var request = this.tbRequest.Text;
            var response = "";

            new Thread(()=> {

                this.BeginInvoke(new MethodInvoker(() => {
                    this.btnUpload.Enabled = false;
                }));

                if (selectName == "身份验证接口")
                {
                    response = MES.IdentityVerification(request);
                    Current.option.MesManuIdentityVerificationInput = request;
                }
                else if (selectName == "电芯与托盘信息查询接口")
                {
                    response = MES.GetTrayBindingInfo(request);
                    Current.option.MesManuGetTrayBindingInfoInput = request;
                }
                else if (selectName == "记录设备状态接口")
                {
                    response = MES.RecordDeviceStatus(request);
                    Current.option.MesManuRecordDeviceStatusInput = request;
                }
                else if (selectName == "二次高温数据上传接口")
                {
                    response = MES.UploadSecondaryHighTempData(request);
                    Current.option.MesManuUploadSecondaryHighTempDataInput = request;
                }

                this.BeginInvoke(new MethodInvoker(() => {
                    this.tbResponse.Text = response;
                    this.btnUpload.Enabled = true;
                }));

            }).Start();
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            this.tbResponse.Clear();
        }

        private void CbInterfaceName_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectName = this.cbInterfaceName.SelectedItem.ToString();
            var request = "";;
            if (selectName == "身份验证接口")
            {
                request = Current.option.MesManuIdentityVerificationInput;
            }
            else if (selectName == "电芯与托盘信息查询接口")
            {
                request = Current.option.MesManuGetTrayBindingInfoInput;
            }
            else if (selectName == "记录设备状态接口")
            {
                request = Current.option.MesManuRecordDeviceStatusInput;
            }
            else if (selectName == "二次高温数据上传接口")
            {
                request = Current.option.MesManuUploadSecondaryHighTempDataInput;
            }
            this.tbRequest.Text = request;
        }
    }
}
