namespace jtzhang163.Helper.Scaner
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
      this.components = new System.ComponentModel.Container();
      this.serialPort1 = new System.IO.Ports.SerialPort(this.components);
      this.richTextBox1 = new System.Windows.Forms.TextBox();
      this.button1 = new System.Windows.Forms.Button();
      this.textBox1 = new System.Windows.Forms.TextBox();
      this.button2 = new System.Windows.Forms.Button();
      this.cbNG = new System.Windows.Forms.CheckBox();
      this.label1 = new System.Windows.Forms.Label();
      this.SuspendLayout();
      // 
      // serialPort1
      // 
      this.serialPort1.PortName = "COM2";
      this.serialPort1.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.serialPort1_DataReceived);
      // 
      // richTextBox1
      // 
      this.richTextBox1.Location = new System.Drawing.Point(33, 65);
      this.richTextBox1.Multiline = true;
      this.richTextBox1.Name = "richTextBox1";
      this.richTextBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
      this.richTextBox1.Size = new System.Drawing.Size(316, 159);
      this.richTextBox1.TabIndex = 0;
      // 
      // button1
      // 
      this.button1.Location = new System.Drawing.Point(160, 23);
      this.button1.Name = "button1";
      this.button1.Size = new System.Drawing.Size(75, 23);
      this.button1.TabIndex = 1;
      this.button1.Text = "打开";
      this.button1.UseVisualStyleBackColor = true;
      this.button1.Click += new System.EventHandler(this.button1_Click);
      // 
      // textBox1
      // 
      this.textBox1.Location = new System.Drawing.Point(33, 244);
      this.textBox1.Name = "textBox1";
      this.textBox1.Size = new System.Drawing.Size(222, 21);
      this.textBox1.TabIndex = 2;
      // 
      // button2
      // 
      this.button2.Location = new System.Drawing.Point(278, 243);
      this.button2.Name = "button2";
      this.button2.Size = new System.Drawing.Size(71, 23);
      this.button2.TabIndex = 3;
      this.button2.Text = "发送";
      this.button2.UseVisualStyleBackColor = true;
      this.button2.Click += new System.EventHandler(this.button2_Click);
      // 
      // cbNG
      // 
      this.cbNG.AutoSize = true;
      this.cbNG.Location = new System.Drawing.Point(313, 26);
      this.cbNG.Name = "cbNG";
      this.cbNG.Size = new System.Drawing.Size(36, 16);
      this.cbNG.TabIndex = 4;
      this.cbNG.Text = "NG";
      this.cbNG.UseVisualStyleBackColor = true;
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(33, 28);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(89, 12);
      this.label1.TabIndex = 5;
      this.label1.Text = "COM2状态：关闭";
      // 
      // Form1
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(392, 287);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.cbNG);
      this.Controls.Add(this.button2);
      this.Controls.Add(this.textBox1);
      this.Controls.Add(this.button1);
      this.Controls.Add(this.richTextBox1);
      this.Name = "Form1";
      this.Text = "串口设备模拟器";
      this.ResumeLayout(false);
      this.PerformLayout();

        }

        #endregion

        private System.IO.Ports.SerialPort serialPort1;
        private System.Windows.Forms.TextBox richTextBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.CheckBox cbNG;
    private System.Windows.Forms.Label label1;
  }
}

