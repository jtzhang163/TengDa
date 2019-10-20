namespace CAMEL.Baking.Control
{
    partial class ScanerDebugUC
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

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnClampScanNgBackToFeeder = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.btnClampScanOkBackToFeeder = new System.Windows.Forms.Button();
            this.cbClampScaner = new System.Windows.Forms.ComboBox();
            this.btnClampScanStart = new System.Windows.Forms.Button();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox3
            // 
            this.groupBox3.BackColor = System.Drawing.SystemColors.Control;
            this.groupBox3.Controls.Add(this.btnClampScanNgBackToFeeder);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.btnClampScanOkBackToFeeder);
            this.groupBox3.Controls.Add(this.cbClampScaner);
            this.groupBox3.Controls.Add(this.btnClampScanStart);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Font = new System.Drawing.Font("黑体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox3.Location = new System.Drawing.Point(0, 0);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(30, 30, 30, 30);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(30, 30, 30, 30);
            this.groupBox3.Size = new System.Drawing.Size(343, 116);
            this.groupBox3.TabIndex = 5;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "夹具扫码枪手动调试";
            // 
            // btnClampScanNgBackToFeeder
            // 
            this.btnClampScanNgBackToFeeder.Location = new System.Drawing.Point(255, 72);
            this.btnClampScanNgBackToFeeder.Name = "btnClampScanNgBackToFeeder";
            this.btnClampScanNgBackToFeeder.Size = new System.Drawing.Size(41, 23);
            this.btnClampScanNgBackToFeeder.TabIndex = 6;
            this.btnClampScanNgBackToFeeder.Text = "NG";
            this.btnClampScanNgBackToFeeder.UseVisualStyleBackColor = true;
            this.btnClampScanNgBackToFeeder.Click += new System.EventHandler(this.BtnClampScanNgBackToFeeder_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(58, 77);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(119, 14);
            this.label2.TabIndex = 5;
            this.label2.Text = "结果反馈上料机：";
            // 
            // btnClampScanOkBackToFeeder
            // 
            this.btnClampScanOkBackToFeeder.Location = new System.Drawing.Point(194, 72);
            this.btnClampScanOkBackToFeeder.Name = "btnClampScanOkBackToFeeder";
            this.btnClampScanOkBackToFeeder.Size = new System.Drawing.Size(41, 23);
            this.btnClampScanOkBackToFeeder.TabIndex = 4;
            this.btnClampScanOkBackToFeeder.Text = "OK";
            this.btnClampScanOkBackToFeeder.UseVisualStyleBackColor = true;
            this.btnClampScanOkBackToFeeder.Click += new System.EventHandler(this.BtnClampScanOkBackToFeeder_Click);
            // 
            // cbClampScaner
            // 
            this.cbClampScaner.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbClampScaner.FormattingEnabled = true;
            this.cbClampScaner.Location = new System.Drawing.Point(58, 32);
            this.cbClampScaner.Name = "cbClampScaner";
            this.cbClampScaner.Size = new System.Drawing.Size(127, 22);
            this.cbClampScaner.TabIndex = 3;
            // 
            // btnClampScanStart
            // 
            this.btnClampScanStart.Location = new System.Drawing.Point(221, 31);
            this.btnClampScanStart.Name = "btnClampScanStart";
            this.btnClampScanStart.Size = new System.Drawing.Size(75, 23);
            this.btnClampScanStart.TabIndex = 0;
            this.btnClampScanStart.Text = "扫码";
            this.btnClampScanStart.UseVisualStyleBackColor = true;
            this.btnClampScanStart.Click += new System.EventHandler(this.BtnClampScanStart_Click);
            // 
            // ScanerDebugUC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox3);
            this.Font = new System.Drawing.Font("黑体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "ScanerDebugUC";
            this.Size = new System.Drawing.Size(343, 116);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btnClampScanNgBackToFeeder;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnClampScanOkBackToFeeder;
        private System.Windows.Forms.ComboBox cbClampScaner;
        private System.Windows.Forms.Button btnClampScanStart;
    }
}
