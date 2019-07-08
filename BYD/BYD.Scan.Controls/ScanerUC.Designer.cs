namespace BYD.Scan.Controls
{
    partial class ScanerUC
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
            this.tbCode2 = new System.Windows.Forms.TextBox();
            this.lbName = new System.Windows.Forms.Label();
            this.tbCode1 = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 90F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.tbCode2, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.lbName, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tbCode1, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(357, 55);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // tbCode2
            // 
            this.tbCode2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tbCode2.ForeColor = System.Drawing.Color.LimeGreen;
            this.tbCode2.Location = new System.Drawing.Point(123, 30);
            this.tbCode2.Name = "tbCode2";
            this.tbCode2.Size = new System.Drawing.Size(231, 21);
            this.tbCode2.TabIndex = 3;
            this.tbCode2.DoubleClick += new System.EventHandler(this.TbCode_DoubleClick);
            // 
            // lbName
            // 
            this.lbName.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lbName.AutoSize = true;
            this.lbName.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbName.Location = new System.Drawing.Point(3, 19);
            this.lbName.Name = "lbName";
            this.tableLayoutPanel1.SetRowSpan(this.lbName, 2);
            this.lbName.Size = new System.Drawing.Size(68, 17);
            this.lbName.TabIndex = 1;
            this.lbName.Text = "扫码枪名称";
            // 
            // tbCode1
            // 
            this.tbCode1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tbCode1.ForeColor = System.Drawing.Color.LimeGreen;
            this.tbCode1.Location = new System.Drawing.Point(123, 3);
            this.tbCode1.Name = "tbCode1";
            this.tbCode1.Size = new System.Drawing.Size(231, 21);
            this.tbCode1.TabIndex = 2;
            this.tbCode1.DoubleClick += new System.EventHandler(this.TbCode_DoubleClick);
            // 
            // ScanerUC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "ScanerUC";
            this.Size = new System.Drawing.Size(357, 55);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label lbName;
        private System.Windows.Forms.TextBox tbCode1;
        private System.Windows.Forms.TextBox tbCode2;
    }
}
