namespace BYD.Scan.Controls
{
    partial class LineUC
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
            this.gbLine = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lineChildUC1 = new BYD.Scan.Controls.LineChildUC();
            this.lineChildUC2 = new BYD.Scan.Controls.LineChildUC();
            this.touchscreenUC1 = new BYD.Scan.Controls.TouchscreenUC();
            this.gbLine.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbLine
            // 
            this.gbLine.Controls.Add(this.tableLayoutPanel1);
            this.gbLine.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbLine.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.gbLine.Location = new System.Drawing.Point(0, 0);
            this.gbLine.Name = "gbLine";
            this.gbLine.Size = new System.Drawing.Size(702, 150);
            this.gbLine.TabIndex = 0;
            this.gbLine.TabStop = false;
            this.gbLine.Text = "1#线";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.lineChildUC1, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.lineChildUC2, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.touchscreenUC1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 22);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(696, 125);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // lineChildUC1
            // 
            this.lineChildUC1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lineChildUC1.Location = new System.Drawing.Point(123, 3);
            this.lineChildUC1.Name = "lineChildUC1";
            this.lineChildUC1.Size = new System.Drawing.Size(570, 56);
            this.lineChildUC1.TabIndex = 0;
            // 
            // lineChildUC2
            // 
            this.lineChildUC2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lineChildUC2.Location = new System.Drawing.Point(123, 65);
            this.lineChildUC2.Name = "lineChildUC2";
            this.lineChildUC2.Size = new System.Drawing.Size(570, 57);
            this.lineChildUC2.TabIndex = 1;
            // 
            // touchscreenUC1
            // 
            this.touchscreenUC1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.touchscreenUC1.Location = new System.Drawing.Point(3, 3);
            this.touchscreenUC1.Name = "touchscreenUC1";
            this.tableLayoutPanel1.SetRowSpan(this.touchscreenUC1, 2);
            this.touchscreenUC1.Size = new System.Drawing.Size(114, 119);
            this.touchscreenUC1.TabIndex = 2;
            // 
            // LineUC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gbLine);
            this.Name = "LineUC";
            this.Size = new System.Drawing.Size(702, 150);
            this.gbLine.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbLine;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private LineChildUC lineChildUC1;
        private LineChildUC lineChildUC2;
        private TouchscreenUC touchscreenUC1;
    }
}
