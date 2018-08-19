using InteractiveDataDisplay.WPF;
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
            if (AppCurrent.AppIsRun)
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

            if (Current.App.RunStatus != TengDa.RunStatus.闲置)
            {
                OperationHelper.ShowTips("系统正在运行，请先停止、复位后关闭！", true);
                e.Cancel = true;
                return;
            }
            //OperationHelper.ShowTips("关闭软件");
            base.OnClosing(e);
        }

        private void Init()
        {

            this.DataContext = Current.App;

            this.wpScaner.DataContext = Current.Scaner;

            this.dpTester.DataContext = Current.Tester;

            this.dpCollector.DataContext = Current.Collector;

            Current.MainWindow = this;

            StartDateTimePicker.Value = DateTime.Now.AddHours(-1);
            StopDateTimePicker.Value = DateTime.Now;

            AppCurrent.IsTerminalInitFinished = true;

            AppCurrent.YieldNow.FeedingOKContent = "测试数";
            AppCurrent.YieldNow.BlankingOKContent = "上传数";
            if (AppCurrent.YieldNow.StartTime == TengDa.Common.DefaultTime)
            {
                AppCurrent.YieldNow.StartTime = DateTime.Now;
            }

            OperationHelper.ShowTips("打开软件");

            MES.GetInfo();
            InitTimer();
        }

        private void InitTimer()
        {
            //界面画图定时器
            InitDrawingGraph();
            //更新
            Current.TimerUpdateTime.Elapsed += delegate { Current.App.TimeNow = DateTime.Now; };
            Current.TimerUpdateTime.Start();

            Current.TimerCheckTesterInfo.Elapsed += new System.Timers.ElapsedEventHandler(new TimerRun().CheckTesterInfo);
            Current.TimerCheckTesterInfo.Start();

            Current.TimerCheckCollectorInfo.Elapsed += new System.Timers.ElapsedEventHandler(new TimerRun().CheckCollectorInfo);
            Current.TimerCheckCollectorInfo.Start();

            Current.TimerCheckCoolerInfo.Elapsed += new System.Timers.ElapsedEventHandler(new TimerRun().CheckCoolerInfo);
            Current.TimerCheckCoolerInfo.Start();

            Current.TimerCheckScanerInfo.Elapsed += new System.Timers.ElapsedEventHandler(new TimerRun().CheckScanerInfo);
            Current.TimerCheckScanerInfo.Start();

            Current.TimerCheckDataInfo.Elapsed += new System.Timers.ElapsedEventHandler(new TimerRun().CheckDataInfo);
            Current.TimerCheckDataInfo.Start();

            Current.TimerCheckMesInfo.Elapsed += new System.Timers.ElapsedEventHandler(new TimerRun().CheckMesInfo);
            Current.TimerCheckMesInfo.Start();
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
            if (AppCurrent.User.Id < 0)
            {
                if (Verify.Show("尚未登录提示", "请先登录"))
                {
                    LoginWindow window = new LoginWindow();
                    window.ShowDialog();
                }
                return;
            }

            if (AppCurrent.IsRunning)
            {
                OperationHelper.ShowTips("系统已经在运行，请勿重复启动！");
                return;
            }

            if (CommunicateControl.CommunicateStart())
            {
                AppCurrent.IsRunning = true;
                Current.App.RunStatus = TengDa.RunStatus.运行;
                OperationHelper.ShowTips("成功启动运行！");
            }

        }

        private void BtnStop_Click(object sender, RoutedEventArgs e)
        {
            if (AppCurrent.User.Id < 0)
            {
                if (Verify.Show("尚未登录提示", "请先登录"))
                {
                    LoginWindow window = new LoginWindow();
                    window.ShowDialog();
                }
                return;
            }

            if (!AppCurrent.IsRunning)
            {
                OperationHelper.ShowTips("系统已暂停运行，请勿重复点击！");
                return;
            }

            AppCurrent.IsRunning = false;
            OperationHelper.ShowTips("成功停止运行！");
            Current.App.RunStatus = TengDa.RunStatus.暂停;
        }

        private void BtnReset_Click(object sender, RoutedEventArgs e)
        {
            if (AppCurrent.User.Id < 0)
            {
                if (Verify.Show("尚未登录提示", "请先登录"))
                {
                    LoginWindow window = new LoginWindow();
                    window.ShowDialog();                  
                }
                return;
            }

            if (AppCurrent.IsRunning)
            {
                OperationHelper.ShowTips("请先停止运行！", isShowMessageBox: true);
                return;
            }

            if (CommunicateControl.CommunicateStop())
            {
                OperationHelper.ShowTips("成功复位！");
                Current.App.RunStatus = TengDa.RunStatus.闲置;
            }
        }

        //private DispatcherTimer timer = new DispatcherTimer();

        //private void AnimatedPlot(object sender, EventArgs e)
        //{

        //    if (Current.IsRunning && Current.App.GraphShowMode == GraphShowMode.实时数据)
        //    {
        //        var lgResistance = (LineGraph)linesResistance.Children[0];
        //        lgResistance.Plot(Current.ShowDataOrder, Current.ShowResistanceData);

        //        var lgVoltage = (LineGraph)linesVoltage.Children[0];
        //        lgVoltage.Plot(Current.ShowDataOrder, Current.ShowVoltageData);

        //        var lgTemperature = (LineGraph)linesTemperature.Children[0];
        //        lgTemperature.Plot(Current.ShowDataOrder, Current.ShowTemperatureData);

        //        var lgTimeSpan = (LineGraph)linesTimeSpan.Children[0];
        //        lgTimeSpan.Plot(Current.ShowDataOrder, Current.ShowTimeSpanData);
        //    }
        //}

        private void InitDrawingGraph()
        {

            var lgResistance = new LineGraph();
            linesResistance.Children.Add(lgResistance);
            lgResistance.Stroke = new SolidColorBrush(Color.FromArgb(255, 0, 255, 0));
            lgResistance.Description = String.Format("电阻");
            lgResistance.StrokeThickness = 2;


            var lgTemperature = new LineGraph();
            linesTemperature.Children.Add(lgTemperature);
            lgTemperature.Stroke = new SolidColorBrush(Color.FromArgb(255, 0, 255, 0));
            lgTemperature.Description = String.Format("温度");
            lgTemperature.StrokeThickness = 2;


            //timer.Interval = TimeSpan.FromMilliseconds(1000);
            //timer.Tick += new EventHandler(AnimatedPlot);
            //timer.IsEnabled = true;
        }

        #region 显示历史数据

        private static List<double> ShowDataOrder = new List<double>();

        private static List<double> ShowResistanceData = new List<double>();

        private static List<double> ShowTemperatureData = new List<double>();

        private void BtnShowHistoryData_Click(object sender, RoutedEventArgs e)
        {
            using (var data = new InsulationContext())
            {
                var dataLogs = data.DataLogs.Where(d => d.DateTime > StartDateTimePicker.Value && d.DateTime < StopDateTimePicker.Value).Take(maxDataCount.Value.Value).ToList();
                if (dataLogs.Count < 1)
                {
                    OperationHelper.ShowTips("该时间范围没数据！",true);
                    return;
                }

                ShowDataOrder.Clear();
                ShowResistanceData.Clear();
                ShowTemperatureData.Clear();

                int order = 1;
                foreach (var cvData in dataLogs)
                {
                    ShowDataOrder.Add(order);
                    ShowResistanceData.Add(cvData.Resistance);
                    ShowTemperatureData.Add(cvData.Temperature);
                    order++;
                }

            }


            var lgResistance = (LineGraph)linesResistance.Children[0];
            lgResistance.Plot(ShowDataOrder, ShowResistanceData);

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
}
