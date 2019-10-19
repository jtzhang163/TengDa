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
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CAMEL.RGV.Touchscreen
{
    /// <summary>
    /// KeyBoradNumber.xaml 的交互逻辑
    /// </summary>
    public partial class KeyBoradNumber : Window
    {
 
        public KeyBoradNumber()
        {
            InitializeComponent();
          

        }
        public KeyBoradNumber(Point xy)
        {
            InitializeComponent();
            this.Left = xy.X + 270;
            this.Top = xy.Y + 350;
            WindowInteropHelper wndHelper = new WindowInteropHelper(this);
            IntPtr HWND = wndHelper.Handle;
            int GWL = -20;
            Win32API.SetWindowLong(HWND, GWL, (IntPtr)(0x8000000));
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button btnNum = sender as Button;
            Win32API.keybd_event(byte.Parse(btnNum.Tag.ToString()), 0, 0, 0);
            if (btnNum.Tag.ToString() == "13")
            {
                this.Close();
            }
        }
        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            if (new Rect(new Point(0, 0), new Size(Width, 10)).Contains(e.GetPosition(this))&&e.LeftButton==MouseButtonState.Pressed)
                this.DragMove();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.Topmost = true;
          
        }
    }
}
