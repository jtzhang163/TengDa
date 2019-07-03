namespace BYD.Scan.Controls
{
    partial class OvenParamUC
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
            this.tbNewValue = new System.Windows.Forms.TextBox();
            this.lbContent = new System.Windows.Forms.Label();
            this.tbOldValue = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableLayoutPanel1.Controls.Add(this.tbNewValue, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.lbContent, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tbOldValue, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(497, 43);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // tbNewValue
            // 
            this.tbNewValue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tbNewValue.Location = new System.Drawing.Point(432, 11);
            this.tbNewValue.Margin = new System.Windows.Forms.Padding(15, 3, 15, 3);
            this.tbNewValue.MaxLength = 5;
            this.tbNewValue.Name = "tbNewValue";
            this.tbNewValue.Size = new System.Drawing.Size(50, 21);
            this.tbNewValue.TabIndex = 2;
            this.tbNewValue.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TbNewValue_KeyPress);
            // 
            // lbContent
            // 
            this.lbContent.AutoSize = true;
            this.lbContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbContent.Location = new System.Drawing.Point(3, 0);
            this.lbContent.Name = "lbContent";
            this.lbContent.Size = new System.Drawing.Size(331, 43);
            this.lbContent.TabIndex = 0;
            this.lbContent.Text = "参数内容";
            this.lbContent.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbOldValue
            // 
            this.tbOldValue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tbOldValue.Enabled = false;
            this.tbOldValue.Location = new System.Drawing.Point(352, 11);
            this.tbOldValue.Margin = new System.Windows.Forms.Padding(15, 3, 15, 3);
            this.tbOldValue.Name = "tbOldValue";
            this.tbOldValue.Size = new System.Drawing.Size(50, 21);
            this.tbOldValue.TabIndex = 1;
            // 
            // OvenParamUC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "OvenParamUC";
            this.Size = new System.Drawing.Size(497, 43);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label lbContent;
        private System.Windows.Forms.TextBox tbNewValue;
        private System.Windows.Forms.TextBox tbOldValue;
    }
}
