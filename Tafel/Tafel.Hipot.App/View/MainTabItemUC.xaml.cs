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
using InteractiveDataDisplay.WPF;
using System.ComponentModel;
using System.Windows.Threading;
using TengDa.Wpf;

namespace Tafel.Hipot.App
{
    /// <summary>
    /// MainTabItemUC.xaml 的交互逻辑
    /// </summary>
    public partial class MainTabItemUC : UserControl
    {
        public MainTabItemUC()
        {
            InitializeComponent();
            this.DataContext = Current.App;

            this.dpTester.DataContext = Current.Tester;

            this.dpCollector.DataContext = Current.Collector;

            this.StartDateTimePicker.Value = DateTime.Now.AddHours(-1);
            this.StopDateTimePicker.Value = DateTime.Now;

            Current.MainTabItemUC = this;

            //界面画图定时器
            InitDrawingGraph();

        }

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
                    OperationHelper.ShowTips("该时间范围没数据！", true);
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
    }
}
