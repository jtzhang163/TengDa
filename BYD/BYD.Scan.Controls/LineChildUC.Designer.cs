namespace BYD.Scan.Controls
{
    partial class LineChildUC
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.scanerUC2 = new BYD.Scan.Controls.ScanerUC();
            this.scanerUC1 = new BYD.Scan.Controls.ScanerUC();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.scanerUC2, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.scanerUC1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(494, 32);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // scanerUC2
            // 
            this.scanerUC2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scanerUC2.Location = new System.Drawing.Point(250, 3);
            this.scanerUC2.Name = "scanerUC2";
            this.scanerUC2.Size = new System.Drawing.Size(241, 26);
            this.scanerUC2.TabIndex = 1;
            // 
            // scanerUC1
            // 
            this.scanerUC1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scanerUC1.Location = new System.Drawing.Point(3, 3);
            this.scanerUC1.Name = "scanerUC1";
            this.scanerUC1.Size = new System.Drawing.Size(241, 26);
            this.scanerUC1.TabIndex = 0;
            // 
            // LineChildUC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "LineChildUC";
            this.Size = new System.Drawing.Size(494, 32);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private ScanerUC scanerUC1;
        private ScanerUC scanerUC2;
    }
}
