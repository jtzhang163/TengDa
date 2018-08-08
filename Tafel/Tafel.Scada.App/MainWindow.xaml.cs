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

            base.OnClosing(e);
        }

        public void OnClose(object sender, ExecutedRoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        public void OnShowLoginView(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void Init()
        {

            new AppDbInitializer().Initialize();

            this.DataContext = AppCurrent.AppViewModel;


            TimerInit();
            //当前时间显示
            Current.ShowTips("打开软件");

            StartDateTimePicker.Value = DateTime.Now;
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

        private void OnQueryCVLog(object sender, ExecutedRoutedEventArgs e)
        {
            var uc = new QueryIDLogUC()
            {

            };
            var isContain = false;
            foreach (TabItem tabItem in this.MainTabControl.Items)
            {
                if (tabItem.Header.ToString() == "电流电压查询") { isContain = true; }
            }

            if (!isContain)
            {
                this.MainTabControl.SelectedIndex = this.MainTabControl.Items.Add(new TabItem { Header = "电流电压查询", Content = uc });
            }
            else
            {
                for (var i = 0; i < this.MainTabControl.Items.Count; i++)
                {
                    if ((this.MainTabControl.Items[i] as TabItem).Header.ToString() == "电流电压查询")
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

        private DispatcherTimer timer = new DispatcherTimer();

        private void AnimatedPlot(object sender, EventArgs e)
        {

            if (Current.IsRunning && AppCurrent.AppViewModel.GraphShowMode == GraphShowMode.实时数据)
            {
                for (int i = 0; i < 1; i++)
                {
                    var lgCurrents = (LineGraph)linesCurrents.Children[i];
                    lgCurrents.Plot(AppCurrent.ShowDataOrder, AppCurrent.ShowCurrentsData[i]);
                }

                var lgCurrent = (LineGraph)linesVoltage.Children[0];
                lgCurrent.Plot(AppCurrent.ShowDataOrder, AppCurrent.ShowVoltageData);
            }

        }

        private Color[] chartColors = new Color[] { Colors.Blue, Colors.Lime, Colors.Green, Colors.Cyan, Colors.Red, Colors.Purple };

        private void InitDrawingGraph()
        {

            for (int i = 0; i < 1; i++)
            {
                var lgCurrents = new LineGraph();
                linesCurrents.Children.Add(lgCurrents);
                lgCurrents.Stroke = new SolidColorBrush(chartColors[i]);
                lgCurrents.Description = String.Format("{0}-第{0}路电流", i + 1);
                lgCurrents.StrokeThickness = 2;
            }


            var lgVoltage = new LineGraph();
            linesVoltage.Children.Add(lgVoltage);
            lgVoltage.Stroke = new SolidColorBrush(Color.FromArgb(255, 0, 255, 0));
            lgVoltage.Description = String.Format("电压");
            lgVoltage.StrokeThickness = 2;


            timer.Interval = TimeSpan.FromMilliseconds(AppCurrent.Option.CommunicatorCommunicateInterval);
            timer.Tick += new EventHandler(AnimatedPlot);
            timer.IsEnabled = true;
        }

        #region 显示历史数据

        private static List<double> ShowDataOrder = new List<double>();

        private List<double> ShowVoltageData = new List<double>();

        private static List<double>[] ShowCurrentsData = new List<double>[1];

        private void BtnShowHistoryData_Click(object sender, RoutedEventArgs e)
        {
            //using (var data = new CurrentVoltageDataContext())
            //{
            //    var CurrentVoltageDatas = data.CurrentVoltageDatas.Where(d => d.RecordTime > StartDateTimePicker.Value && d.RecordTime < StopDateTimePicker.Value).ToList();
            //    if (CurrentVoltageDatas.Count < 1)
            //    {
            //        Current.ShowTips("该时间范围没数据！");
            //        return;
            //    }


            //    ShowDataOrder.Clear();
            //    ShowVoltageData.Clear();
            //    for (int i = 0; i < 1; i++)
            //    {
            //        ShowCurrentsData[i] = new List<double>();
            //    }

            //    int order = 1;
            //    foreach (var cvData in CurrentVoltageDatas)
            //    {
            //        ShowDataOrder.Add(order++);
            //        ShowVoltageData.Add(cvData.Voltage);
            //        for (int i = 0; i < 1; i++)
            //        {
            //            ShowCurrentsData[i].Add(cvData.Currents[i]);
            //        }
            //    }

            //}

            //for (int i = 0; i < 1; i++)
            //{
            //    var lgCurrents = (LineGraph)linesCurrents.Children[i];
            //    lgCurrents.Plot(ShowDataOrder, ShowCurrentsData[i]);
            //}

            //var lgCurrent = (LineGraph)linesVoltage.Children[0];
            //lgCurrent.Plot(ShowDataOrder, ShowVoltageData);
        }

        #endregion
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
