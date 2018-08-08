using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using TengDa.Wpf;

namespace Tafel.Hipot.App
{
    /// <summary>
    /// QueryCVLogUC.xaml 的交互逻辑
    /// </summary>
    public partial class QueryIDLogUC : UserControl
    {
        public QueryIDLogUC()
        {
            InitializeComponent();
            StartDateTimePicker.Value = DateTime.Now.AddDays(-1);
            StopDateTimePicker.Value = DateTime.Now;
            this.DataContext = UserIDLogViewModels;
        }

        public IEnumerable<UserIDLogViewModel> UserIDLogViewModels
        {
            get
            {
                var userIDLogViewModels = new List<UserIDLogViewModel>();
                using (var data = new InsulationContext())
                {
                    data.InsulationDataLogs.Where(ucvd => ucvd.RecordTime > StartDateTimePicker.Value && ucvd.RecordTime < StopDateTimePicker.Value).Take(maxDataCount.Value.Value).ToList().ForEach(c =>
                    {
                        userIDLogViewModels.Add(new UserIDLogViewModel
                        {
                            UserName = c.UserId > 0 ? Current.Users.FirstOrDefault(u => u.Id == c.UserId).Name : "未登录用户",
                            TesterName = AppCurrent.InsulationTester.Name,
                            Voltage = c.Voltage,
                            TimeSpan = c.TimeSpan,
                            Temperature = c.Temperature,
                            Resistance = c.Resistance,
                            IsUploaded = c.IsUploaded,
                            RecordTime = c.RecordTime
                        });
                    });
                }
                return userIDLogViewModels;
            }
        }


        private void BtnQuery_Click(object sender, RoutedEventArgs e)
        {
            this.DataContext = UserIDLogViewModels;
        }

        private void BtnExport_Click(object sender, RoutedEventArgs e)
        {
            ExcelHelper.Export(TengDa._Convert.ToDataTable<UserIDLogViewModel>(UserIDLogViewModels, true), "Hipot测试数据", StartDateTimePicker.Value ?? DateTime.Now, StopDateTimePicker.Value ?? DateTime.Now);
        }
    }
}
