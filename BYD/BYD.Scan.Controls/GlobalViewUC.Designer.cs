namespace BYD.Scan.Controls
{
    partial class GlobalViewUC
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
            this.lineUC4 = new BYD.Scan.Controls.LineUC();
            this.lineUC3 = new BYD.Scan.Controls.LineUC();
            this.lineUC2 = new BYD.Scan.Controls.LineUC();
            this.lineUC1 = new BYD.Scan.Controls.LineUC();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.lineUC4, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.lineUC3, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.lineUC2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.lineUC1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(597, 247);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // lineUC4
            // 
            this.lineUC4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lineUC4.Location = new System.Drawing.Point(3, 186);
            this.lineUC4.Name = "lineUC4";
            this.lineUC4.Size = new System.Drawing.Size(591, 58);
            this.lineUC4.TabIndex = 3;
            // 
            // lineUC3
            // 
            this.lineUC3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lineUC3.Location = new System.Drawing.Point(3, 125);
            this.lineUC3.Name = "lineUC3";
            this.lineUC3.Size = new System.Drawing.Size(591, 55);
            this.lineUC3.TabIndex = 2;
            // 
            // lineUC2
            // 
            this.lineUC2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lineUC2.Location = new System.Drawing.Point(3, 64);
            this.lineUC2.Name = "lineUC2";
            this.lineUC2.Size = new System.Drawing.Size(591, 55);
            this.lineUC2.TabIndex = 1;
            // 
            // lineUC1
            // 
            this.lineUC1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lineUC1.Location = new System.Drawing.Point(3, 3);
            this.lineUC1.Name = "lineUC1";
            this.lineUC1.Size = new System.Drawing.Size(591, 55);
            this.lineUC1.TabIndex = 0;
            // 
            // GlobalViewUC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "GlobalViewUC";
            this.Size = new System.Drawing.Size(597, 247);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private LineUC lineUC4;
        private LineUC lineUC3;
        private LineUC lineUC2;
        private LineUC lineUC1;
    }
}
