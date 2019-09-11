namespace CAMEL.Baking.Control
{
    partial class MaintainUC
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
            this.Clear1 = new System.Windows.Forms.Label();
            this.Clear2 = new System.Windows.Forms.Label();
            this.Clear3 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // Clear1
            // 
            this.Clear1.AutoSize = true;
            this.Clear1.BackColor = System.Drawing.Color.Red;
            this.Clear1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Clear1.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Clear1.ForeColor = System.Drawing.Color.White;
            this.Clear1.Location = new System.Drawing.Point(102, 0);
            this.Clear1.Margin = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.Clear1.Name = "Clear1";
            this.Clear1.Size = new System.Drawing.Size(72, 44);
            this.Clear1.TabIndex = 0;
            this.Clear1.Text = "LABEL1";
            this.Clear1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.Clear1.Click += new System.EventHandler(this.Clear_CLick);
            // 
            // Clear2
            // 
            this.Clear2.AutoSize = true;
            this.Clear2.BackColor = System.Drawing.Color.Red;
            this.Clear2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Clear2.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Clear2.ForeColor = System.Drawing.Color.White;
            this.Clear2.Location = new System.Drawing.Point(194, 0);
            this.Clear2.Margin = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.Clear2.Name = "Clear2";
            this.Clear2.Size = new System.Drawing.Size(72, 44);
            this.Clear2.TabIndex = 1;
            this.Clear2.Text = "LABEL2";
            this.Clear2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.Clear2.Click += new System.EventHandler(this.Clear_CLick);
            // 
            // Clear3
            // 
            this.Clear3.AutoSize = true;
            this.Clear3.BackColor = System.Drawing.Color.Red;
            this.Clear3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Clear3.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Clear3.ForeColor = System.Drawing.Color.White;
            this.Clear3.Location = new System.Drawing.Point(286, 0);
            this.Clear3.Margin = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.Clear3.Name = "Clear3";
            this.Clear3.Size = new System.Drawing.Size(72, 44);
            this.Clear3.TabIndex = 1;
            this.Clear3.Text = "LABEL3";
            this.Clear3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.Clear3.Click += new System.EventHandler(this.Clear_CLick);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.Controls.Add(this.Clear3, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.Clear1, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.Clear2, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(368, 44);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("黑体", 10.5F);
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(2, 0);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 44);
            this.label1.TabIndex = 2;
            this.label1.Text = "维护提示：";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // MaintainUC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "MaintainUC";
            this.Size = new System.Drawing.Size(368, 44);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label Clear1;
        private System.Windows.Forms.Label Clear2;
        private System.Windows.Forms.Label Clear3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label1;
    }
}
