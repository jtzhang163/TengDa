using System.Data;
using System.Windows.Controls;

namespace TengDa.Wpf
{
    public class DataHelper
    {
        public static DataTable DataGridToDataTable(DataGrid dataGrid)
        {
            //http://blog.csdn.net/queenpong/article/details/46840271
            DataTable dt = new DataTable();

            for (int i = 0; i < dataGrid.Columns.Count; i++)
            {
                //if (dataGrid.Columns[i].Visibility == System.Windows.Visibility.Visible)//只导出可见列  
                //{
                dt.Columns.Add(dataGrid.Columns[i].Header.ToString());//构建表头  

                //}
            }

            for (int i = 0; i < dataGrid.Items.Count; i++)
            {
                int columnsIndex = 0;
                DataRow row = dt.NewRow();
                for (int j = 0; j < dataGrid.Columns.Count; j++)
                {
                    //if (dataGrid.Columns[j].Visibility == System.Windows.Visibility.Visible)
                    //{
                    if (dataGrid.Items[i] != null && (dataGrid.Columns[j].GetCellContent(dataGrid.Items[i]) as TextBlock) != null)//填充可见列数据  
                    {
                        row[columnsIndex] = (dataGrid.Columns[j].GetCellContent(dataGrid.Items[i]) as TextBlock).Text.ToString();
                    }
                    else
                    {
                        row[columnsIndex] = "";
                    }
                    columnsIndex++;
                    //}
                }
                dt.Rows.Add(row);
            }

            return dt;
        }
    }
}
