namespace BYD.Scan.Controls
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
            this.gbScanerDebug = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.btnStartScan = new System.Windows.Forms.Button();
            this.btnStopScan = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.cbScanerList = new System.Windows.Forms.ComboBox();
            this.gbScanerDebug.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbScanerDebug
            // 
            this.gbScanerDebug.Controls.Add(this.tableLayoutPanel1);
            this.gbScanerDebug.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbScanerDebug.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.gbScanerDebug.Location = new System.Drawing.Point(0, 0);
            this.gbScanerDebug.Name = "gbScanerDebug";
            this.gbScanerDebug.Size = new System.Drawing.Size(514, 189);
            this.gbScanerDebug.TabIndex = 0;
            this.gbScanerDebug.TabStop = false;
            this.gbScanerDebug.Text = "自动扫码枪调试";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.Controls.Add(this.btnStartScan, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnStopScan, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.cbScanerList, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 19);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(508, 167);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // btnStartScan
            // 
            this.btnStartScan.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnStartScan.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnStartScan.Location = new System.Drawing.Point(130, 113);
            this.btnStartScan.Name = "btnStartScan";
            this.btnStartScan.Size = new System.Drawing.Size(75, 23);
            this.btnStartScan.TabIndex = 0;
            this.btnStartScan.Text = "启动扫码";
            this.btnStartScan.UseVisualStyleBackColor = true;
            this.btnStartScan.Click += new System.EventHandler(this.BtnStartScan_Click);
            // 
            // btnStopScan
            // 
            this.btnStopScan.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnStopScan.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnStopScan.Location = new System.Drawing.Point(303, 113);
            this.btnStopScan.Name = "btnStopScan";
            this.btnStopScan.Size = new System.Drawing.Size(75, 23);
            this.btnStopScan.TabIndex = 1;
            this.btnStopScan.Text = "停止扫码";
            this.btnStopScan.UseVisualStyleBackColor = true;
            this.btnStopScan.Click += new System.EventHandler(this.BtnStopScan_Click);
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(33, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(91, 14);
            this.label1.TabIndex = 2;
            this.label1.Text = "选择扫码枪：";
            // 
            // cbScanerList
            // 
            this.cbScanerList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.cbScanerList, 2);
            this.cbScanerList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbScanerList.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cbScanerList.FormattingEnabled = true;
            this.cbScanerList.Location = new System.Drawing.Point(130, 30);
            this.cbScanerList.Name = "cbScanerList";
            this.cbScanerList.Size = new System.Drawing.Size(248, 22);
            this.cbScanerList.TabIndex = 3;
            this.cbScanerList.SelectedIndexChanged += new System.EventHandler(this.CbScanerList_SelectedIndexChanged);
            // 
            // ScanerDebugUC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gbScanerDebug);
            this.Name = "ScanerDebugUC";
            this.Size = new System.Drawing.Size(514, 189);
            this.gbScanerDebug.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbScanerDebug;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button btnStartScan;
        private System.Windows.Forms.Button btnStopScan;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbScanerList;
    }
}
