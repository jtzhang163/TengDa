namespace Anchitech.Baking.Controls
{
    partial class TorVUC
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
            this.lbValue = new System.Windows.Forms.Label();
            this.lbSubject = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel1.Controls.Add(this.lbValue, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.lbSubject, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(182, 36);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // lbValue
            // 
            this.lbValue.AutoSize = true;
            this.lbValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbValue.Font = new System.Drawing.Font("Consolas", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbValue.ForeColor = System.Drawing.Color.Green;
            this.lbValue.Location = new System.Drawing.Point(130, 0);
            this.lbValue.Name = "lbValue";
            this.lbValue.Size = new System.Drawing.Size(49, 36);
            this.lbValue.TabIndex = 1;
            this.lbValue.Text = "0";
            this.lbValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbSubject
            // 
            this.lbSubject.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lbSubject.AutoSize = true;
            this.lbSubject.Location = new System.Drawing.Point(83, 12);
            this.lbSubject.Name = "lbSubject";
            this.lbSubject.Size = new System.Drawing.Size(41, 12);
            this.lbSubject.TabIndex = 0;
            this.lbSubject.Text = "主题：";
            // 
            // TorVUC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "TorVUC";
            this.Size = new System.Drawing.Size(182, 36);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label lbSubject;
        private System.Windows.Forms.Label lbValue;
    }
}
