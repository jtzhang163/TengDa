namespace BYD.Scan.Controls
{
    partial class TouchscreenDebugUC
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
            this.gbTouchscreenDebug = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.cbAddr = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cbTouchscreenList = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbWriteValue = new System.Windows.Forms.TextBox();
            this.btnWrite = new System.Windows.Forms.Button();
            this.btnRead = new System.Windows.Forms.Button();
            this.tbReadResult = new System.Windows.Forms.TextBox();
            this.gbTouchscreenDebug.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbTouchscreenDebug
            // 
            this.gbTouchscreenDebug.Controls.Add(this.tableLayoutPanel1);
            this.gbTouchscreenDebug.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbTouchscreenDebug.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.gbTouchscreenDebug.Location = new System.Drawing.Point(0, 0);
            this.gbTouchscreenDebug.Name = "gbTouchscreenDebug";
            this.gbTouchscreenDebug.Size = new System.Drawing.Size(432, 206);
            this.gbTouchscreenDebug.TabIndex = 1;
            this.gbTouchscreenDebug.TabStop = false;
            this.gbTouchscreenDebug.Text = "触摸屏调试";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 130F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel1.Controls.Add(this.cbAddr, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.cbTouchscreenList, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnRead, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.tbReadResult, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.tbWriteValue, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.btnWrite, 3, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 19);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(426, 184);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // cbAddr
            // 
            this.cbAddr.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.cbAddr, 2);
            this.cbAddr.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbAddr.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cbAddr.FormattingEnabled = true;
            this.cbAddr.Items.AddRange(new object[] {
            "0 4WUB001 【D900(A线)】",
            "1 4WUB002 【D901(A线)】",
            "2 4WUB003 【D902(A线)】",
            "3 4WUB004",
            "4 4WUB005",
            "5 4WUB006",
            "6 4WUB007 【D900(C线)】",
            "7 4WUB008 【D901(C线)】",
            "8 4WUB009 【D902(C线)】"});
            this.cbAddr.Location = new System.Drawing.Point(133, 80);
            this.cbAddr.Name = "cbAddr";
            this.cbAddr.Size = new System.Drawing.Size(200, 22);
            this.cbAddr.TabIndex = 5;
            this.cbAddr.SelectedIndexChanged += new System.EventHandler(this.CbAddr_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(36, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(91, 14);
            this.label1.TabIndex = 2;
            this.label1.Text = "选择触摸屏：";
            // 
            // cbTouchscreenList
            // 
            this.cbTouchscreenList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.cbTouchscreenList, 2);
            this.cbTouchscreenList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbTouchscreenList.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cbTouchscreenList.FormattingEnabled = true;
            this.cbTouchscreenList.Location = new System.Drawing.Point(133, 19);
            this.cbTouchscreenList.Name = "cbTouchscreenList";
            this.cbTouchscreenList.Size = new System.Drawing.Size(200, 22);
            this.cbTouchscreenList.TabIndex = 3;
            this.cbTouchscreenList.SelectedIndexChanged += new System.EventHandler(this.CbTouchscreenList_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(78, 84);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(49, 14);
            this.label2.TabIndex = 4;
            this.label2.Text = "地址：";
            // 
            // tbWriteValue
            // 
            this.tbWriteValue.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.tbWriteValue.ForeColor = System.Drawing.Color.LimeGreen;
            this.tbWriteValue.Location = new System.Drawing.Point(284, 141);
            this.tbWriteValue.Name = "tbWriteValue";
            this.tbWriteValue.Size = new System.Drawing.Size(49, 23);
            this.tbWriteValue.TabIndex = 7;
            this.tbWriteValue.Text = "0";
            // 
            // btnWrite
            // 
            this.btnWrite.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnWrite.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnWrite.Location = new System.Drawing.Point(339, 141);
            this.btnWrite.Name = "btnWrite";
            this.btnWrite.Size = new System.Drawing.Size(75, 23);
            this.btnWrite.TabIndex = 0;
            this.btnWrite.Text = "写入";
            this.btnWrite.UseVisualStyleBackColor = true;
            this.btnWrite.Click += new System.EventHandler(this.BtnWrite_Click);
            // 
            // btnRead
            // 
            this.btnRead.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnRead.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnRead.Location = new System.Drawing.Point(52, 141);
            this.btnRead.Name = "btnRead";
            this.btnRead.Size = new System.Drawing.Size(75, 23);
            this.btnRead.TabIndex = 1;
            this.btnRead.Text = "读取";
            this.btnRead.UseVisualStyleBackColor = true;
            this.btnRead.Click += new System.EventHandler(this.BtnRead_Click);
            // 
            // tbReadResult
            // 
            this.tbReadResult.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.tbReadResult.ForeColor = System.Drawing.Color.LimeGreen;
            this.tbReadResult.Location = new System.Drawing.Point(133, 141);
            this.tbReadResult.Name = "tbReadResult";
            this.tbReadResult.Size = new System.Drawing.Size(49, 23);
            this.tbReadResult.TabIndex = 6;
            // 
            // TouchscreenDebugUC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gbTouchscreenDebug);
            this.Name = "TouchscreenDebugUC";
            this.Size = new System.Drawing.Size(432, 206);
            this.gbTouchscreenDebug.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbTouchscreenDebug;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button btnWrite;
        private System.Windows.Forms.Button btnRead;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbTouchscreenList;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbAddr;
        private System.Windows.Forms.TextBox tbReadResult;
        private System.Windows.Forms.TextBox tbWriteValue;
    }
}
