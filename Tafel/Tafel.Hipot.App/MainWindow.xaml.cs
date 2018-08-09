﻿using InteractiveDataDisplay.WPF;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using TengDa.Wpf;

namespace Tafel.Hipot.App
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Fluent.RibbonWindow
    {

        public MainWindow()
        {
            #region 检查程序是否重复启动
            if (Current.AppIsRun)
            {
                if (Xceed.Wpf.Toolkit.MessageBox.Show("当前程序已经在运行，请勿重复启动！", "提示", MessageBoxButton.OK, MessageBoxImage.Information) == MessageBoxResult.OK)
                {
                    System.Environment.Exit(0);
                    return;
                }
            }
            #endregion
            InitializeComponent();
            Init();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (Current.IsRunning)
            {
                Current.ShowTips("系统正在运行，请先停止！", true);
                e.Cancel = true;
                return;
            }

            base.OnClosing(e);
        }


        private void Init()
        {

            new AppDbInitializer().Initialize();

            this.DataContext = AppCurrent.AppViewModel;

            AppCurrent.MainWindow = this;

            TimerInit();
            //当前时间显示
            Current.ShowTips("打开软件");

            StartDateTimePicker.Value = DateTime.Now.AddHours(-1);
            StopDateTimePicker.Value = DateTime.Now;

            Current.IsTerminalInitFinished = true;
        }


        private void TimerInit()
        {
            //界面画图定时器
            InitDrawingGraph();
            //更新
            AppCurrent.TimerUpdateTime.Elapsed += delegate { AppCurrent.AppViewModel.TimeNow = DateTime.Now; };
            AppCurrent.TimerUpdateTime.Start();

            AppCurrent.TimerCheckTesterInfo.Elapsed += new System.Timers.ElapsedEventHandler(new TimerRun().CheckTesterInfo);
            AppCurrent.TimerCheckTesterInfo.Start();

            AppCurrent.TimerCheckMesInfo.Elapsed += new System.Timers.ElapsedEventHandler(new TimerRun().CheckMesInfo);
            AppCurrent.TimerCheckMesInfo.Start();
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //数据库初始化
            using (var data = new UserContext())
            {
                foreach (var ug in data.Roles)
                {
                    Console.WriteLine("{0} ", ug.Name);
                }
                Console.WriteLine("finished!");
            }
            using (var data = new OptionContext())
            {
                foreach (var ug in data.Options)
                {
                    Console.WriteLine("{0} ", ug.Value);
                }
                Console.WriteLine("finished!");
            }
        }


        private void BtnTest_Click(object sender, RoutedEventArgs e)
        {


        }

        private void OnQueryOperationLog(object sender, ExecutedRoutedEventArgs e)
        {
            var uc = new QueryOperationLogUC()
            {

            };
            var isContain = false;
            foreach (TabItem tabItem in this.MainTabControl.Items)
            {
                if (tabItem.Header.ToString() == "系统日志") { isContain = true; }
            }

            if (!isContain)
            {
                this.MainTabControl.SelectedIndex = this.MainTabControl.Items.Add(new TabItem { Header = "系统日志", Content = uc });
            }
            else
            {
                for (var i = 0; i < this.MainTabControl.Items.Count; i++)
                {
                    if ((this.MainTabControl.Items[i] as TabItem).Header.ToString() == "系统日志")
                    {
                        this.MainTabControl.SelectedIndex = i;
                        return;
                    }
                }
            }
        }

        private void OnQueryIDLog(object sender, ExecutedRoutedEventArgs e)
        {
            var uc = new QueryIDLogUC()
            {

            };
            var isContain = false;
            foreach (TabItem tabItem in this.MainTabControl.Items)
            {
                if (tabItem.Header.ToString() == "测试数据查询") { isContain = true; }
            }

            if (!isContain)
            {
                this.MainTabControl.SelectedIndex = this.MainTabControl.Items.Add(new TabItem { Header = "测试数据查询", Content = uc });
            }
            else
            {
                for (var i = 0; i < this.MainTabControl.Items.Count; i++)
                {
                    if ((this.MainTabControl.Items[i] as TabItem).Header.ToString() == "测试数据查询")
                    {
                        this.MainTabControl.SelectedIndex = i;
                        return;
                    }
                }
            }
        }

        private void BtnBaseSetting_Click(object sender, RoutedEventArgs e)
        {
            AppBackstage.IsOpen = true;
            SettingsBackstageTabItem.IsSelected = true;
        }

        private void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            if (Current.User.Id < 0)
            {
                if (Verify.Show("尚未登录提示", "请先登录"))
                {
                    LoginWindow window = new LoginWindow();
                    window.ShowDialog();
                }
                return;
            }

            if (Current.IsRunning)
            {
                Current.ShowTips("系统已经在运行，请勿重复启动！");
                return;
            }

            if (CommunicateControl.CommunicateStart())
            {
                Current.IsRunning = true;
                AppCurrent.AppViewModel.RunStatus = TengDa.RunStatus.运行;
                Current.ShowTips("成功启动运行！");
            }

        }

        private void BtnStop_Click(object sender, RoutedEventArgs e)
        {
            if (Current.User.Id < 0)
            {
                if (Verify.Show("尚未登录提示", "请先登录"))
                {
                    LoginWindow window = new LoginWindow();
                    window.ShowDialog();
                }
                return;
            }

            if (!Current.IsRunning)
            {
                Current.ShowTips("系统已暂停运行，请勿重复点击！");
                return;
            }

            Current.IsRunning = false;
            Current.ShowTips("成功停止运行！");
            AppCurrent.AppViewModel.RunStatus = TengDa.RunStatus.暂停;
        }

        private void BtnReset_Click(object sender, RoutedEventArgs e)
        {
            if (Current.User.Id < 0)
            {
                if (Verify.Show("尚未登录提示", "请先登录"))
                {
                    LoginWindow window = new LoginWindow();
                    window.ShowDialog();                  
                }
                return;
            }

            if (Current.IsRunning)
            {
                Current.ShowTips("请先停止运行！", isShowMessageBox: true);
                return;
            }

            if (CommunicateControl.CommunicateStop())
            {
                Current.ShowTips("成功复位！");
                AppCurrent.AppViewModel.RunStatus = TengDa.RunStatus.闲置;
            }
        }

        //private DispatcherTimer timer = new DispatcherTimer();

        //private void AnimatedPlot(object sender, EventArgs e)
        //{

        //    if (Current.IsRunning && AppCurrent.AppViewModel.GraphShowMode == GraphShowMode.实时数据)
        //    {
        //        var lgResistance = (LineGraph)linesResistance.Children[0];
        //        lgResistance.Plot(AppCurrent.ShowDataOrder, AppCurrent.ShowResistanceData);

        //        var lgVoltage = (LineGraph)linesVoltage.Children[0];
        //        lgVoltage.Plot(AppCurrent.ShowDataOrder, AppCurrent.ShowVoltageData);

        //        var lgTemperature = (LineGraph)linesTemperature.Children[0];
        //        lgTemperature.Plot(AppCurrent.ShowDataOrder, AppCurrent.ShowTemperatureData);

        //        var lgTimeSpan = (LineGraph)linesTimeSpan.Children[0];
        //        lgTimeSpan.Plot(AppCurrent.ShowDataOrder, AppCurrent.ShowTimeSpanData);
        //    }
        //}

        private void InitDrawingGraph()
        {

            var lgResistance = new LineGraph();
            linesResistance.Children.Add(lgResistance);
            lgResistance.Stroke = new SolidColorBrush(Color.FromArgb(255, 0, 255, 0));
            lgResistance.Description = String.Format("电阻");
            lgResistance.StrokeThickness = 2;

            var lgVoltage = new LineGraph();
            linesVoltage.Children.Add(lgVoltage);
            lgVoltage.Stroke = new SolidColorBrush(Color.FromArgb(255, 0, 255, 0));
            lgVoltage.Description = String.Format("电压");
            lgVoltage.StrokeThickness = 2;

            var lgTemperature = new LineGraph();
            linesTemperature.Children.Add(lgTemperature);
            lgTemperature.Stroke = new SolidColorBrush(Color.FromArgb(255, 0, 255, 0));
            lgTemperature.Description = String.Format("温度");
            lgTemperature.StrokeThickness = 2;

            var lgTimeSpan = new LineGraph();
            linesTimeSpan.Children.Add(lgTimeSpan);
            lgTimeSpan.Stroke = new SolidColorBrush(Color.FromArgb(255, 0, 255, 0));
            lgTimeSpan.Description = String.Format("测试时长");
            lgTimeSpan.StrokeThickness = 2;


            //timer.Interval = TimeSpan.FromMilliseconds(1000);
            //timer.Tick += new EventHandler(AnimatedPlot);
            //timer.IsEnabled = true;
        }

        #region 显示历史数据

        private static List<double> ShowDataOrder = new List<double>();

        private static List<double> ShowVoltageData = new List<double>();

        private static List<double> ShowResistanceData = new List<double>();

        private static List<double> ShowTemperatureData = new List<double>();

        private static List<double> ShowTimeSpanData = new List<double>();

        private void BtnShowHistoryData_Click(object sender, RoutedEventArgs e)
        {
            using (var data = new InsulationContext())
            {
                var insulationDataLogs = data.InsulationDataLogs.Where(d => d.RecordTime > StartDateTimePicker.Value && d.RecordTime < StopDateTimePicker.Value).Take(maxDataCount.Value.Value).ToList();
                if (insulationDataLogs.Count < 1)
                {
                    Current.ShowTips("该时间范围没数据！",true);
                    return;
                }

                ShowDataOrder.Clear();
                ShowVoltageData.Clear();
                ShowResistanceData.Clear();
                ShowTemperatureData.Clear();
                ShowTimeSpanData.Clear();

                int order = 1;
                foreach (var cvData in insulationDataLogs)
                {
                    ShowDataOrder.Add(order);
                    ShowVoltageData.Add(cvData.Voltage);
                    ShowResistanceData.Add(cvData.Resistance);
                    ShowTemperatureData.Add(cvData.Temperature);
                    ShowTimeSpanData.Add(cvData.TimeSpan);

                    order++;
                }

            }


            var lgResistance = (LineGraph)linesResistance.Children[0];
            lgResistance.Plot(ShowDataOrder, ShowResistanceData);

            var lgVoltage = (LineGraph)linesVoltage.Children[0];
            lgVoltage.Plot(ShowDataOrder, ShowVoltageData);

            var lgTimeSpan = (LineGraph)linesTimeSpan.Children[0];
            lgTimeSpan.Plot(ShowDataOrder, ShowTimeSpanData);

            var lgTemperature = (LineGraph)linesTemperature.Children[0];
            lgTemperature.Plot(ShowDataOrder, ShowTemperatureData);

        }

        #endregion

        private void BtnCloseTabItem_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            string header = btn.Tag.ToString();
            foreach (TabItem item in MainTabControl.Items)
            {
                string _header = item.Header.ToString();
                if (_header == header && _header != "数据曲线")
                {
                    MainTabControl.Items.Remove(item);
                    break;
                }
            }
        }
    }



    public class VisibilityToCheckedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return ((Visibility)value) == Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return ((bool)value) ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}