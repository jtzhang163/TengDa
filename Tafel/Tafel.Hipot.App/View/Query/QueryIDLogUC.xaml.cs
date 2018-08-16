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
                    data.DataLogs.Where(ucvd => ucvd.DateTime > StartDateTimePicker.Value && ucvd.DateTime < StopDateTimePicker.Value).Take(maxDataCount.Value.Value).ToList().ForEach(c =>
                    {
                        userIDLogViewModels.Add(new UserIDLogViewModel
                        {
                            UserName = c.User.Id > 0 ? TengDa.Wpf.Context.UserContext.Users.FirstOrDefault(u => u.Id == c.User.Id).Name : "未登录用户",
                            TesterName = Current.Tester.Name,
                            Voltage = c.Voltage,
                            TimeSpan = c.TimeSpan,
                            Temperature = c.Temperature,
                            Resistance = c.Resistance,
                            IsUploaded = c.IsUploaded,
                            DateTime = c.DateTime
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
