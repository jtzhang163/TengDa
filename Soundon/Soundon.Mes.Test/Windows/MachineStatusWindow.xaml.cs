﻿using Soundon.Mes.Test.ExecutingWebReference;
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

namespace Soundon.Mes.Test.Windows
{
    /// <summary>
    /// BakingOutSiteWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MachineStatusWindow : Window
    {

        private string Url = Common.Executing_URL_TEST;

        public MachineStatusWindow()
        {
            InitializeComponent();
            this.url.Text = this.Url;
        }

        private void btnUpload_Click(object sender, RoutedEventArgs e)
        {

            var wsProxy = new ExecutingServiceService();
            wsProxy.Credentials = new NetworkCredential(Common.Username, Common.Password, null);
            wsProxy.PreAuthenticate = true;
            wsProxy.Timeout = 2000;
            wsProxy.Url = this.Url;

            //转换SFC
            var sfcs = this.sfcs.Text.Split(',', ' ', '\r', '\n');
            var sfcDatas = new List<MachStatus>();
            for (var i = 0; i < sfcs.Length; i++)
            {
                if (sfcs[i].Length > 0)
                {
                    var source_status = sfcs[i].Split(':');
                    if (source_status.Length > 1)
                    {
                        sfcDatas.Add(new MachStatus()
                        {
                            RESOURCE = source_status[0],
                            STATUS = source_status[1]
                        });
                    }
                    else
                    {
                        MessageBox.Show("输入数据有误" + sfcs[i]);
                        return;
                    }
                }
            }

            try
            {
                var request = new executingServiceRequest()
                {
                    site = this.site.Text.Trim(),
                    serviceCode = "ResourceStatusChangeService",
                    data = JsonHelper.SerializeObject(new ExecuteData() {
                        RESOURCE_LIST = sfcDatas.ToArray()
                    })              
                };

                var response = wsProxy.execute(new execute() { pRequest = request });
                this.status.Text = response.@return.status;
                this.message.Text = response.@return.message;
            }
            catch (System.Exception ex)
            {
                this.status.Text = "ERROR";
                this.message.Text = ex.ToString();
            }
            finally
            {
                if (this.status.Text.Trim().ToLower() == "true")
                {
                    this.status.Foreground = Brushes.Green;
                    this.message.Foreground = Brushes.Green;
                }
                else
                {
                    this.status.Foreground = Brushes.Red;
                    this.message.Foreground = Brushes.Red;
                }
            }
            ShowLog();
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (this.url == null) return;

            var radioButton = sender as RadioButton;
            if (!radioButton.IsChecked.Value) return;

            this.Url = radioButton.Content.ToString() == "测试" ? Common.Executing_URL_TEST : Common.Executing_URL_NORMAL;
            this.url.Text = this.Url;
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            this.status.Text = "";
            this.message.Text = "";
        }

        private void ShowLog()
        {
            Common.WriteLog(string.Format("==== 设备状态变化接口 == time：{0:yyyy-MM-dd HH:mm:ss} ====", DateTime.Now));
            Common.WriteLog("--- 参数 ----");
            Common.WriteLog(string.Format("服务地址 ： {0}", this.Url));
            Common.WriteLog(string.Format("工厂代码 ： {0}", this.site.Text));
            Common.WriteLog(string.Format("设备状态 ： {0}", this.sfcs.Text));
            Common.WriteLog("--- 结果 ----");
            Common.WriteLog(string.Format("状态 ： {0}", this.status.Text));
            Common.WriteLog(string.Format("消息 ： {0}", this.message.Text));
            Common.WriteLog("");
        }
    }
}
