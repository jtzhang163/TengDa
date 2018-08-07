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
    /// QueryOperationLogUC.xaml 的交互逻辑
    /// </summary>
    public partial class QueryOperationLogUC : UserControl
    {
        public QueryOperationLogUC()
        {
            InitializeComponent();
            StartDateTimePicker.Value = DateTime.Now;
            StopDateTimePicker.Value = DateTime.Now;
            this.DataContext = UserOperations;
        }

        public IEnumerable<UserOperationViewModel> UserOperations
        {
            get
            {
                var userOperations = new List<UserOperationViewModel>();
                Current.Operations.ForEach(o =>
                {
                    userOperations.Add(new UserOperationViewModel
                    {
                        Content = o.Content,
                        Time = o.Time,
                        UserName = o.UserId > 0 ? Current.Users.FirstOrDefault(u => u.Id == o.UserId).Name : "未登录用户"
                    });
                });
                return userOperations.Where(uo => uo.Time > StartDateTimePicker.Value && uo.Time < StopDateTimePicker.Value);
            }
        }


        private void BtnQuery_Click(object sender, RoutedEventArgs e)
        {
            this.DataContext = UserOperations;
        }

        private void BtnExport_Click(object sender, RoutedEventArgs e)
        {
            ExcelHelper.Export(TengDa._Convert.ToDataTable<UserOperationViewModel>(UserOperations, true), "系统日志", StartDateTimePicker.Value ?? DateTime.Now, StopDateTimePicker.Value ?? DateTime.Now);
            //ExcelHelper.Export(DataHelper.DataGridToDataTable(ResultDataGrid), "系统日志", StartDateTimePicker.Value ?? DateTime.Now, StopDateTimePicker.Value ?? DateTime.Now);
        }
    }
}
