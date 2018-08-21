using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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

            if (Current.App.RunStatus == TengDa.RunStatus.运行)
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

            this.MainTabControl.SelectedIndex = this.MainTabControl.Items.Add(new TabItem { Header = "主界面", Content = new MainTabItemUC() });

            Current.MainWindow = this;
        }

        private void InitTimer()
        {
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
            var isContain = false;
            foreach (TabItem tabItem in this.MainTabControl.Items)
            {
                if (tabItem.Header.ToString() == "系统日志") { isContain = true; }
            }

            if (!isContain)
            {
                this.MainTabControl.SelectedIndex = this.MainTabControl.Items.Add(new TabItem { Header = "系统日志", Content = new QueryOperationLogUC() });
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
            var isContain = false;
            foreach (TabItem tabItem in this.MainTabControl.Items)
            {
                if (tabItem.Header.ToString() == "测试数据查询") { isContain = true; }
            }

            if (!isContain)
            {
                this.MainTabControl.SelectedIndex = this.MainTabControl.Items.Add(new TabItem { Header = "测试数据查询", Content = new QueryIDLogUC() });
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

            if (Current.App.RunStatus == TengDa.RunStatus.运行)
            {
                Tip.Alert("系统正在运行，复位无效，请停止运行后再执行复位操作！");
                return;
            }

            if (Current.App.RunStatus == TengDa.RunStatus.闲置)
            {
                Tip.Alert("系统尚未启动，复位操作无效！");
                return;
            }

            if (CommunicateControl.CommunicateStop())
            {
                OperationHelper.ShowTips("成功复位！");
                Current.App.RunStatus = TengDa.RunStatus.闲置;
            }
        }

        private void BtnCloseTabItem_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            string header = btn.Tag.ToString();
            foreach (TabItem item in MainTabControl.Items)
            {
                string _header = item.Header.ToString();
                if (_header == header && _header != "主界面")
                {
                    MainTabControl.Items.Remove(item);
                    break;
                }
            }
        }
    }
}
