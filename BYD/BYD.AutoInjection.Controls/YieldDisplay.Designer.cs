namespace BYD.AutoInjection.Controls
{
    partial class YieldDisplay
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
            this.gbYield = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel23 = new System.Windows.Forms.TableLayoutPanel();
            this.lbBlankingOK = new System.Windows.Forms.Label();
            this.btnYieldClear = new System.Windows.Forms.Button();
            this.lbFeedingOK = new System.Windows.Forms.Label();
            this.lbShowFeedingOK = new System.Windows.Forms.Label();
            this.lbShowBlankingOK = new System.Windows.Forms.Label();
            this.label62 = new System.Windows.Forms.Label();
            this.lbClearYieldTime = new System.Windows.Forms.Label();
            this.gbYield.SuspendLayout();
            this.tableLayoutPanel23.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbYield
            // 
            this.gbYield.BackColor = System.Drawing.SystemColors.Control;
            this.gbYield.Controls.Add(this.tableLayoutPanel23);
            this.gbYield.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbYield.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.gbYield.Location = new System.Drawing.Point(0, 0);
            this.gbYield.Margin = new System.Windows.Forms.Padding(15, 0, 15, 0);
            this.gbYield.Name = "gbYield";
            this.gbYield.Size = new System.Drawing.Size(279, 128);
            this.gbYield.TabIndex = 13;
            this.gbYield.TabStop = false;
            this.gbYield.Text = "产量";
            // 
            // tableLayoutPanel23
            // 
            this.tableLayoutPanel23.ColumnCount = 3;
            this.tableLayoutPanel23.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tableLayoutPanel23.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel23.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel23.Controls.Add(this.lbBlankingOK, 0, 1);
            this.tableLayoutPanel23.Controls.Add(this.btnYieldClear, 2, 0);
            this.tableLayoutPanel23.Controls.Add(this.lbFeedingOK, 0, 0);
            this.tableLayoutPanel23.Controls.Add(this.lbShowFeedingOK, 1, 0);
            this.tableLayoutPanel23.Controls.Add(this.lbShowBlankingOK, 1, 1);
            this.tableLayoutPanel23.Controls.Add(this.label62, 0, 2);
            this.tableLayoutPanel23.Controls.Add(this.lbClearYieldTime, 1, 2);
            this.tableLayoutPanel23.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel23.Location = new System.Drawing.Point(3, 19);
            this.tableLayoutPanel23.Name = "tableLayoutPanel23";
            this.tableLayoutPanel23.RowCount = 3;
            this.tableLayoutPanel23.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel23.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel23.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel23.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel23.Size = new System.Drawing.Size(273, 106);
            this.tableLayoutPanel23.TabIndex = 14;
            // 
            // lbBlankingOK
            // 
            this.lbBlankingOK.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lbBlankingOK.AutoSize = true;
            this.lbBlankingOK.Location = new System.Drawing.Point(25, 44);
            this.lbBlankingOK.Margin = new System.Windows.Forms.Padding(25, 3, 3, 3);
            this.lbBlankingOK.Name = "lbBlankingOK";
            this.lbBlankingOK.Size = new System.Drawing.Size(76, 17);
            this.lbBlankingOK.TabIndex = 12;
            this.lbBlankingOK.Text = "下料数(夹具)";
            // 
            // btnYieldClear
            // 
            this.btnYieldClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.btnYieldClear.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnYieldClear.Location = new System.Drawing.Point(200, 23);
            this.btnYieldClear.Margin = new System.Windows.Forms.Padding(10, 3, 10, 3);
            this.btnYieldClear.Name = "btnYieldClear";
            this.tableLayoutPanel23.SetRowSpan(this.btnYieldClear, 2);
            this.btnYieldClear.Size = new System.Drawing.Size(63, 23);
            this.btnYieldClear.TabIndex = 13;
            this.btnYieldClear.Text = "清零(&C)";
            this.btnYieldClear.UseVisualStyleBackColor = true;
            this.btnYieldClear.Click += new System.EventHandler(this.btnYieldClear_Click);
            // 
            // lbFeedingOK
            // 
            this.lbFeedingOK.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lbFeedingOK.AutoSize = true;
            this.lbFeedingOK.Location = new System.Drawing.Point(25, 9);
            this.lbFeedingOK.Margin = new System.Windows.Forms.Padding(25, 3, 3, 3);
            this.lbFeedingOK.Name = "lbFeedingOK";
            this.lbFeedingOK.Size = new System.Drawing.Size(76, 17);
            this.lbFeedingOK.TabIndex = 11;
            this.lbFeedingOK.Text = "上料数(夹具)";
            // 
            // lbShowFeedingOK
            // 
            this.lbShowFeedingOK.AutoSize = true;
            this.lbShowFeedingOK.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbShowFeedingOK.Font = new System.Drawing.Font("黑体", 12F, System.Drawing.FontStyle.Bold);
            this.lbShowFeedingOK.ForeColor = System.Drawing.Color.Green;
            this.lbShowFeedingOK.Location = new System.Drawing.Point(112, 0);
            this.lbShowFeedingOK.Name = "lbShowFeedingOK";
            this.lbShowFeedingOK.Size = new System.Drawing.Size(75, 35);
            this.lbShowFeedingOK.TabIndex = 16;
            this.lbShowFeedingOK.Text = "0";
            this.lbShowFeedingOK.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbShowBlankingOK
            // 
            this.lbShowBlankingOK.AutoSize = true;
            this.lbShowBlankingOK.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbShowBlankingOK.Font = new System.Drawing.Font("黑体", 12F, System.Drawing.FontStyle.Bold);
            this.lbShowBlankingOK.ForeColor = System.Drawing.Color.Green;
            this.lbShowBlankingOK.Location = new System.Drawing.Point(112, 35);
            this.lbShowBlankingOK.Name = "lbShowBlankingOK";
            this.lbShowBlankingOK.Size = new System.Drawing.Size(75, 35);
            this.lbShowBlankingOK.TabIndex = 17;
            this.lbShowBlankingOK.Text = "0";
            this.lbShowBlankingOK.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label62
            // 
            this.label62.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label62.AutoSize = true;
            this.label62.Location = new System.Drawing.Point(25, 79);
            this.label62.Margin = new System.Windows.Forms.Padding(25, 3, 3, 3);
            this.label62.Name = "label62";
            this.label62.Size = new System.Drawing.Size(56, 17);
            this.label62.TabIndex = 12;
            this.label62.Text = "起始时间";
            // 
            // lbClearYieldTime
            // 
            this.lbClearYieldTime.AutoSize = true;
            this.tableLayoutPanel23.SetColumnSpan(this.lbClearYieldTime, 2);
            this.lbClearYieldTime.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbClearYieldTime.Font = new System.Drawing.Font("黑体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbClearYieldTime.ForeColor = System.Drawing.Color.ForestGreen;
            this.lbClearYieldTime.Location = new System.Drawing.Point(112, 70);
            this.lbClearYieldTime.Name = "lbClearYieldTime";
            this.lbClearYieldTime.Size = new System.Drawing.Size(158, 36);
            this.lbClearYieldTime.TabIndex = 15;
            this.lbClearYieldTime.Text = "2000/10/10 00:00";
            this.lbClearYieldTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // YieldDisplay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gbYield);
            this.Name = "YieldDisplay";
            this.Size = new System.Drawing.Size(279, 128);
            this.gbYield.ResumeLayout(false);
            this.tableLayoutPanel23.ResumeLayout(false);
            this.tableLayoutPanel23.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbYield;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel23;
        private System.Windows.Forms.Label lbClearYieldTime;
        private System.Windows.Forms.Label lbBlankingOK;
        private System.Windows.Forms.Button btnYieldClear;
        private System.Windows.Forms.Label lbFeedingOK;
        private System.Windows.Forms.Label label62;
        private System.Windows.Forms.Label lbShowFeedingOK;
        private System.Windows.Forms.Label lbShowBlankingOK;
    }
}
