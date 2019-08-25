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

namespace CAMEL.RGV.Touchscreen.View
{
    /// <summary>
    /// MainPageUC.xaml 的交互逻辑
    /// </summary>
    public partial class MainPageUC : UserControl
    {
        public MainPageUC()
        {
            InitializeComponent();
            this.DataContext = Current.RGV;

            AddHandler();
        }

        /// <summary>
        /// 添加鼠标按下抬起事件，在xaml中声明无反应
        /// </summary>
        private void AddHandler()
        {
            this.btn调度无效.AddHandler(Button.MouseDownEvent, new MouseButtonEventHandler(触摸按钮_MouseDown), true);
            this.btn调度无效.AddHandler(Button.MouseUpEvent, new MouseButtonEventHandler(触摸按钮_MouseUp), true);
            this.btn蜂鸣停止.AddHandler(Button.MouseDownEvent, new MouseButtonEventHandler(触摸按钮_MouseDown), true);
            this.btn蜂鸣停止.AddHandler(Button.MouseUpEvent, new MouseButtonEventHandler(触摸按钮_MouseUp), true);
            this.btn复位.AddHandler(Button.MouseDownEvent, new MouseButtonEventHandler(触摸按钮_MouseDown), true);
            this.btn复位.AddHandler(Button.MouseUpEvent, new MouseButtonEventHandler(触摸按钮_MouseUp), true);
            this.btn手动状态.AddHandler(Button.MouseDownEvent, new MouseButtonEventHandler(触摸按钮_MouseDown), true);
            this.btn手动状态.AddHandler(Button.MouseUpEvent, new MouseButtonEventHandler(触摸按钮_MouseUp), true);
        }

        private void BtnAlarmParam_Click(object sender, RoutedEventArgs e)
        {
            new AlarmParamWindow().ShowDialog();
        }

        private void BtnAutoManuParam_Click(object sender, RoutedEventArgs e)
        {
            new AutoManuParamWindow().ShowDialog();
        }

        private void BtnShowLine1Layout_Click(object sender, RoutedEventArgs e)
        {
            new Line1LayoutWindow().ShowDialog();
        }

        private void BtnShowLine2Layout_Click(object sender, RoutedEventArgs e)
        {
            new Line2LayoutWindow().ShowDialog();
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Current.RGV.DisConnect(out string msg);
            Window.GetWindow(this).Close();
            Application.Current.Shutdown();
        }

        private void 触摸按钮_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Current.Option.IsPad) return;   //运行在平板时不触发
            MouseOrTouchDownOrUp.MouseOrTouchDown(sender);
        }

        private void 触摸按钮_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (Current.Option.IsPad) return;   //运行在平板时不触发
            MouseOrTouchDownOrUp.MouseOrTouchUp(sender);
        }

        private void 触摸按钮_TouchEnter(object sender, TouchEventArgs e)
        {
            if (!Current.Option.IsPad) return;  //运行在平板时触发
            MouseOrTouchDownOrUp.MouseOrTouchDown(sender);
        }

        private void 触摸按钮_TouchLeave(object sender, TouchEventArgs e)
        {
            if (!Current.Option.IsPad) return;  //运行在平板时触发
            MouseOrTouchDownOrUp.MouseOrTouchUp(sender);
        }
    }
}
