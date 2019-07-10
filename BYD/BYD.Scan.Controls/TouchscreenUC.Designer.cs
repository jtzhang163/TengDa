namespace BYD.Scan.Controls
{
    partial class TouchscreenUC
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
            this.lbIsReadyScan1 = new System.Windows.Forms.Label();
            this.lbName = new System.Windows.Forms.Label();
            this.lbIsReadyScan2 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.lbIsReadyScan1, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.lbName, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.lbIsReadyScan2, 1, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(128, 115);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // lbIsReadyScan1
            // 
            this.lbIsReadyScan1.AutoSize = true;
            this.lbIsReadyScan1.BackColor = System.Drawing.SystemColors.Control;
            this.lbIsReadyScan1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbIsReadyScan1.Location = new System.Drawing.Point(23, 0);
            this.lbIsReadyScan1.Name = "lbIsReadyScan1";
            this.lbIsReadyScan1.Size = new System.Drawing.Size(102, 42);
            this.lbIsReadyScan1.TabIndex = 1;
            this.lbIsReadyScan1.Text = "请求扫码";
            this.lbIsReadyScan1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbName
            // 
            this.lbName.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lbName.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.lbName, 2);
            this.lbName.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.lbName.Location = new System.Drawing.Point(3, 48);
            this.lbName.Name = "lbName";
            this.lbName.Size = new System.Drawing.Size(68, 17);
            this.lbName.TabIndex = 0;
            this.lbName.Text = "触摸屏名称";
            // 
            // lbIsReadyScan2
            // 
            this.lbIsReadyScan2.AutoSize = true;
            this.lbIsReadyScan2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbIsReadyScan2.Location = new System.Drawing.Point(23, 72);
            this.lbIsReadyScan2.Name = "lbIsReadyScan2";
            this.lbIsReadyScan2.Size = new System.Drawing.Size(102, 43);
            this.lbIsReadyScan2.TabIndex = 2;
            this.lbIsReadyScan2.Text = "请求扫码";
            this.lbIsReadyScan2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // TouchscreenUC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "TouchscreenUC";
            this.Size = new System.Drawing.Size(128, 115);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label lbName;
        private System.Windows.Forms.Label lbIsReadyScan1;
        private System.Windows.Forms.Label lbIsReadyScan2;
    }
}
