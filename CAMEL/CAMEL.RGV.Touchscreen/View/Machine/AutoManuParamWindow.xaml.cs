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
using System.Windows.Shapes;

namespace CAMEL.RGV.Touchscreen.View
{
    /// <summary>
    /// AutoManuParamWindow.xaml 的交互逻辑
    /// </summary>
    public partial class AutoManuParamWindow : Window
    {
        public AutoManuParamWindow()
        {
            InitializeComponent();
            this.DataContext = Current.RGV;
            this.lbCurrentTime.DataContext = Current.Option;

            AddHandler();
        }

        /// <summary>
        /// 添加鼠标按下抬起事件，在xaml中声明无反应
        /// </summary>
        private void AddHandler()
        {
            this.btn行走JOG正转.AddHandler(Button.MouseDownEvent, new MouseButtonEventHandler(触摸按钮_MouseDown), true);
            this.btn行走JOG正转.AddHandler(Button.MouseUpEvent, new MouseButtonEventHandler(触摸按钮_MouseUp), true);
            this.btn行走JOG反转.AddHandler(Button.MouseDownEvent, new MouseButtonEventHandler(触摸按钮_MouseDown), true);
            this.btn行走JOG反转.AddHandler(Button.MouseUpEvent, new MouseButtonEventHandler(触摸按钮_MouseUp), true);
            this.btn升降JOG上升.AddHandler(Button.MouseDownEvent, new MouseButtonEventHandler(触摸按钮_MouseDown), true);
            this.btn升降JOG上升.AddHandler(Button.MouseUpEvent, new MouseButtonEventHandler(触摸按钮_MouseUp), true);
            this.btn升降JOG降下.AddHandler(Button.MouseDownEvent, new MouseButtonEventHandler(触摸按钮_MouseDown), true);
            this.btn升降JOG降下.AddHandler(Button.MouseUpEvent, new MouseButtonEventHandler(触摸按钮_MouseUp), true);
            this.btn货叉JOG正转.AddHandler(Button.MouseDownEvent, new MouseButtonEventHandler(触摸按钮_MouseDown), true);
            this.btn货叉JOG正转.AddHandler(Button.MouseUpEvent, new MouseButtonEventHandler(触摸按钮_MouseUp), true);
            this.btn货叉原点.AddHandler(Button.MouseDownEvent, new MouseButtonEventHandler(触摸按钮_MouseDown), true);
            this.btn货叉原点.AddHandler(Button.MouseUpEvent, new MouseButtonEventHandler(触摸按钮_MouseUp), true);
            this.btn货叉JOG反转.AddHandler(Button.MouseDownEvent, new MouseButtonEventHandler(触摸按钮_MouseDown), true);
            this.btn货叉JOG反转.AddHandler(Button.MouseUpEvent, new MouseButtonEventHandler(触摸按钮_MouseUp), true);
            this.btn行走测试.AddHandler(Button.MouseDownEvent, new MouseButtonEventHandler(触摸按钮_MouseDown), true);
            this.btn行走测试.AddHandler(Button.MouseUpEvent, new MouseButtonEventHandler(触摸按钮_MouseUp), true);
            this.btn升降1测试.AddHandler(Button.MouseDownEvent, new MouseButtonEventHandler(触摸按钮_MouseDown), true);
            this.btn升降1测试.AddHandler(Button.MouseUpEvent, new MouseButtonEventHandler(触摸按钮_MouseUp), true);
            this.btn升降2测试.AddHandler(Button.MouseDownEvent, new MouseButtonEventHandler(触摸按钮_MouseDown), true);
            this.btn升降2测试.AddHandler(Button.MouseUpEvent, new MouseButtonEventHandler(触摸按钮_MouseUp), true);
            this.btn货叉测试.AddHandler(Button.MouseDownEvent, new MouseButtonEventHandler(触摸按钮_MouseDown), true);
            this.btn货叉测试.AddHandler(Button.MouseUpEvent, new MouseButtonEventHandler(触摸按钮_MouseUp), true);
            this.btn参数写入.AddHandler(Button.MouseDownEvent, new MouseButtonEventHandler(触摸按钮_MouseDown), true);
            this.btn参数写入.AddHandler(Button.MouseUpEvent, new MouseButtonEventHandler(触摸按钮_MouseUp), true);
            this.btn手动状态.AddHandler(Button.MouseDownEvent, new MouseButtonEventHandler(触摸按钮_MouseDown), true);
            this.btn手动状态.AddHandler(Button.MouseUpEvent, new MouseButtonEventHandler(触摸按钮_MouseUp), true);
            this.btn启动.AddHandler(Button.MouseDownEvent, new MouseButtonEventHandler(触摸按钮_MouseDown), true);
            this.btn启动.AddHandler(Button.MouseUpEvent, new MouseButtonEventHandler(触摸按钮_MouseUp), true);
            this.btn复位.AddHandler(Button.MouseDownEvent, new MouseButtonEventHandler(触摸按钮_MouseDown), true);
            this.btn复位.AddHandler(Button.MouseUpEvent, new MouseButtonEventHandler(触摸按钮_MouseUp), true);
            this.btn停止.AddHandler(Button.MouseDownEvent, new MouseButtonEventHandler(触摸按钮_MouseDown), true);
            this.btn停止.AddHandler(Button.MouseUpEvent, new MouseButtonEventHandler(触摸按钮_MouseUp), true);
            this.btn急停.AddHandler(Button.MouseDownEvent, new MouseButtonEventHandler(触摸按钮_MouseDown), true);
            this.btn急停.AddHandler(Button.MouseUpEvent, new MouseButtonEventHandler(触摸按钮_MouseUp), true);
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
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
