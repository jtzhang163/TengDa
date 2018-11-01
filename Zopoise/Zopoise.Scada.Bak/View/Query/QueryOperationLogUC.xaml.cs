using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using TengDa.Wpf;

namespace Zopoise.Scada.Bak
{
    /// <summary>
    /// QueryOperationLogUC.xaml 的交互逻辑
    /// </summary>
    public partial class QueryOperationLogUC : UserControl
    {
        public QueryOperationLogUC()
        {
            InitializeComponent();
            StartDateTimePicker.Value = DateTime.Now.AddDays(-1);
            StopDateTimePicker.Value = DateTime.Now;
            this.DataContext = Operations;
        }

        public IEnumerable<OperationViewModel> Operations
        {
            get
            {
                var operations = new List<OperationViewModel>();
                TengDa.Wpf.Context.OperationContext.Operations.Where(o => o.DateTime > StartDateTimePicker.Value && o.DateTime < StopDateTimePicker.Value).Take(maxDataCount.Value.Value).ToList().ForEach(o =>
                {
                    operations.Add(new OperationViewModel
                    {
                        Content = o.Content,
                        Time = o.DateTime,
                        UserName = o.User.Name
                    });
                });
                return operations;
            }
        }


        private void BtnQuery_Click(object sender, RoutedEventArgs e)
        {
            this.DataContext = Operations;
        }

        private void BtnExport_Click(object sender, RoutedEventArgs e)
        {
            ExcelHelper.Export(TengDa._Convert.ToDataTable<OperationViewModel>(Operations, true), "系统日志", StartDateTimePicker.Value ?? DateTime.Now, StopDateTimePicker.Value ?? DateTime.Now);
        }
    }
}
