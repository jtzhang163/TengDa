using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace jtzhang163.Helper.Scaner
{
  public partial class Form1 : Form
  {


    //SerialPort serialPort1 = new SerialPort("COM2", 9600, Parity.None, 8, StopBits.One);      //初始化串口设置  
    public delegate void Displaydelegate(byte[] InputBuf);
    Byte[] OutputBuf = new Byte[128];
    public Displaydelegate disp_delegate;

    public Form1()
    {
      disp_delegate = new Displaydelegate(DispUI);
      // serialPort1.DataReceived += new SerialDataReceivedEventHandler(Comm_DataReceived);
      InitializeComponent();
    }

    private void button1_Click(object sender, EventArgs e)
    {
      try
      {
        if (button1.Text == "打开")
        {
          serialPort1.Open();
          button1.Text = "关闭";
          label1.Text = "COM2状态：" + "打开";
          label1.ForeColor = Color.Lime;
        }
        else
        {
          serialPort1.Close();
          button1.Text = "打开";
          label1.Text = "COM2状态：" + "关闭";
          label1.ForeColor = SystemColors.WindowText;
        }

      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.Message, "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
        return;
      }
    }

    void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
    {

      Byte[] InputBuf = new Byte[128];

      try
      {
        serialPort1.Read(InputBuf, 0, serialPort1.BytesToRead); //读取缓冲区的数据
        System.Threading.Thread.Sleep(300);
        this.Invoke(disp_delegate, InputBuf);
      }
      catch (TimeoutException ex)         //超时处理  
      {
        MessageBox.Show(ex.ToString());
      }
    }

    public void DispUI(byte[] InputBuf)
    {
      // richTextBox1.Text = Convert.ToString(InputBuf);  
      ASCIIEncoding encoding = new ASCIIEncoding();
      string returnString = encoding.GetString(InputBuf);

      TextBoxShow("接收：" + returnString);

      ///新增测试自动返回数据
      ///
      if (returnString.IndexOf("T") > -1)
      {
        string str = cbNG.Checked ? "NG" : "%%A4FAB" + tmp++.ToString("D9") + "%";
        serialPort1.Write(str); //发送
        TextBoxShow("发送：" + str);
      }
      if (returnString.IndexOf("**-#") > -1)
      {
        //string str = "001-065-001-0.8761-0.8761-0.8761-0.8761-0.8761-0.8761-188.61-18\r";
        string str = string.Format("001-065-001-{0}-{1}-{2}-{3}-{4}-{5}-{6}-18\r",
          (Math.Sin(DateTime.Now.Second) + new Random().Next(1, 6)).ToString("#0.0000"),
          (Math.Sin(DateTime.Now.Second) + new Random().Next(1, 8)).ToString("#0.0000"),
          (Math.Sin(DateTime.Now.Second) + new Random().Next(1, 9)).ToString("#0.0000"),
          (Math.Sin(DateTime.Now.Second) + new Random().Next(1, 3)).ToString("#0.0000"),
          (Math.Sin(DateTime.Now.Second) + new Random().Next(1, 2)).ToString("#0.0000"),
          (Math.Sin(DateTime.Now.Second) + new Random().Next(1, 6)).ToString("#0.0000"),
          (Math.Cos(DateTime.Now.Second) + new Random().Next(1,100)).ToString("#000.00")
          );// "001-065-001-0.8761-0.8761-0.8761-0.8761-0.8761-0.8761-188.61-18\r";
        serialPort1.Write(str); //发送
        TextBoxShow("发送：" + str);
      }
    }


    private void TextBoxShow(string str)
    {
      richTextBox1.AppendText(string.Format("[{0}]{1}\r\n", DateTime.Now.ToString("HH:mm:ss"), str));
      richTextBox1.Focus();
      richTextBox1.Select(richTextBox1.TextLength, 0);
      richTextBox1.ScrollToCaret();
    }


    private int tmp = 0;

    private void button2_Click(object sender, EventArgs e)
    {
      try
      {
        serialPort1.Write(textBox1.Text); //发送
        System.Threading.Thread.Sleep(50);
      }
      catch (TimeoutException ex)         //超时处理  
      {
        MessageBox.Show(ex.ToString());
      }
    }
  }
}
