using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using TengDa.Wpf;

namespace Zopoise.Scada.App
{
    /// <summary>
    /// QueryCVLogUC.xaml 的交互逻辑
    /// </summary>
    public partial class QueryCVLogUC : UserControl
    {
        public QueryCVLogUC()
        {
            InitializeComponent();
            StartDateTimePicker.Value = DateTime.Now;
            StopDateTimePicker.Value = DateTime.Now;
            this.DataContext = UserCVDatas;
        }

        public IEnumerable<UserCVDataViewModel> UserCVDatas
        {
            get
            {
                var userCVDatas = new List<UserCVDataViewModel>();
                using (var data = new CurrentVoltageDataContext())
                {
                    data.CurrentVoltageDatas.ToList().ForEach(c =>
                    {
                        userCVDatas.Add(new UserCVDataViewModel
                        {
                            UserName = c.UserId > 0 ? Current.Users.FirstOrDefault(u => u.Id == c.UserId).Name : "未登录用户",
                            TesterName = AppCurrent.Testers.FirstOrDefault(t => t.Id == c.TesterId).Name,
                            Voltage = c.Voltage,
                            Current1 = c.Currents[0],
                            Current2 = c.Currents[1],
                            Current3 = c.Currents[2],
                            Current4 = c.Currents[3],
                            Current5 = c.Currents[4],
                            Current6 = c.Currents[5],
                            CurrentType = c.CurrentType,
                            RecordTime = c.RecordTime
                        });
                    });
                }
                return userCVDatas.Where(ucvd => ucvd.RecordTime > StartDateTimePicker.Value && ucvd.RecordTime < StopDateTimePicker.Value);
            }
        }


        private void BtnQuery_Click(object sender, RoutedEventArgs e)
        {
            this.DataContext = UserCVDatas;
        }

        private void BtnExport_Click(object sender, RoutedEventArgs e)
        {
            ExcelHelper.Export(TengDa._Convert.ToDataTable<UserCVDataViewModel>(UserCVDatas, true), "电流电压历史数据", StartDateTimePicker.Value ?? DateTime.Now, StopDateTimePicker.Value ?? DateTime.Now);
        }
    }
}
