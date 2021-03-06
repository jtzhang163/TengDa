﻿using CAMEL.RGV.Touchscreen.Util;
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

namespace CAMEL.RGV.Touchscreen.View
{
    /// <summary>
    /// ConnectorUC.xaml 的交互逻辑
    /// </summary>
    public partial class ConnectorUC : UserControl
    {
        public ConnectorUC()
        {
            InitializeComponent();
            this.DataContext = Current.RGV;
            this.btnDisConnect.IsEnabled = false;
        }

        private void BtnConnect_Click(object sender, RoutedEventArgs e)
        {
            if (Current.RGV.Connect(out string msg))
            {
                Speech.Voice("连接成功");
                this.selectRGV.IsEnabled = false;
                this.btnConnect.IsEnabled = false;
                this.btnDisConnect.IsEnabled = true;
            }
            else
            {
                Speech.Voice("连接PLC出错");
                MessageBox.Show("连接PLC出错：" + msg, "异常提示", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void BtnDisConnect_Click(object sender, RoutedEventArgs e)
        {
            if (Current.RGV.DisConnect(out string msg))
            {
                Speech.Voice("断开连接成功");
                this.selectRGV.IsEnabled = true;
                this.btnConnect.IsEnabled = true;
                this.btnDisConnect.IsEnabled = false;
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Current.RGV.IP = (sender as ComboBox).SelectedIndex == 0 ? Current.Option.RGV1_IP : Current.Option.RGV2_IP;
        }
    }
}
