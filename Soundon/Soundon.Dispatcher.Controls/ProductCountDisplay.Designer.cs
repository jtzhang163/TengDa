namespace Soundon.Dispatcher.Controls
{
    partial class ProductCountDisplay
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
            this.label2 = new System.Windows.Forms.Label();
            this.btnProductCountClear1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.btnProductCountClear2 = new System.Windows.Forms.Button();
            this.lbLineName1 = new System.Windows.Forms.Label();
            this.lbLineName2 = new System.Windows.Forms.Label();
            this.lbProductCountAllLine2 = new System.Windows.Forms.Label();
            this.lbProductCountLine1 = new System.Windows.Forms.Label();
            this.lbProductCountAllLine1 = new System.Windows.Forms.Label();
            this.lbProductCountLine2 = new System.Windows.Forms.Label();
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
            this.gbYield.Text = "上料产量";
            // 
            // tableLayoutPanel23
            // 
            this.tableLayoutPanel23.ColumnCount = 4;
            this.tableLayoutPanel23.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30.76923F));
            this.tableLayoutPanel23.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 23.07692F));
            this.tableLayoutPanel23.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 23.07692F));
            this.tableLayoutPanel23.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 23.07692F));
            this.tableLayoutPanel23.Controls.Add(this.btnProductCountClear1, 3, 1);
            this.tableLayoutPanel23.Controls.Add(this.btnProductCountClear2, 3, 2);
            this.tableLayoutPanel23.Controls.Add(this.label1, 1, 0);
            this.tableLayoutPanel23.Controls.Add(this.label2, 2, 0);
            this.tableLayoutPanel23.Controls.Add(this.lbLineName1, 0, 1);
            this.tableLayoutPanel23.Controls.Add(this.lbProductCountLine1, 1, 1);
            this.tableLayoutPanel23.Controls.Add(this.lbProductCountAllLine1, 2, 1);
            this.tableLayoutPanel23.Controls.Add(this.lbProductCountAllLine2, 2, 2);
            this.tableLayoutPanel23.Controls.Add(this.lbProductCountLine2, 1, 2);
            this.tableLayoutPanel23.Controls.Add(this.lbLineName2, 0, 2);
            this.tableLayoutPanel23.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel23.Location = new System.Drawing.Point(3, 19);
            this.tableLayoutPanel23.Name = "tableLayoutPanel23";
            this.tableLayoutPanel23.RowCount = 3;
            this.tableLayoutPanel23.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33222F));
            this.tableLayoutPanel23.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33389F));
            this.tableLayoutPanel23.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33389F));
            this.tableLayoutPanel23.Size = new System.Drawing.Size(273, 106);
            this.tableLayoutPanel23.TabIndex = 14;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(150, 9);
            this.label2.Margin = new System.Windows.Forms.Padding(3);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 17);
            this.label2.TabIndex = 12;
            this.label2.Text = "总产量";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnProductCountClear1
            // 
            this.btnProductCountClear1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.btnProductCountClear1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnProductCountClear1.Location = new System.Drawing.Point(220, 41);
            this.btnProductCountClear1.Margin = new System.Windows.Forms.Padding(10, 3, 10, 3);
            this.btnProductCountClear1.Name = "btnProductCountClear1";
            this.btnProductCountClear1.Size = new System.Drawing.Size(43, 23);
            this.btnProductCountClear1.TabIndex = 13;
            this.btnProductCountClear1.Text = "清零(&C)";
            this.btnProductCountClear1.UseVisualStyleBackColor = true;
            this.btnProductCountClear1.Click += new System.EventHandler(this.BtnProductCountClear_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(87, 9);
            this.label1.Margin = new System.Windows.Forms.Padding(3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 17);
            this.label1.TabIndex = 11;
            this.label1.Text = "产量";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnProductCountClear2
            // 
            this.btnProductCountClear2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.btnProductCountClear2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnProductCountClear2.Location = new System.Drawing.Point(220, 76);
            this.btnProductCountClear2.Margin = new System.Windows.Forms.Padding(10, 3, 10, 3);
            this.btnProductCountClear2.Name = "btnProductCountClear2";
            this.btnProductCountClear2.Size = new System.Drawing.Size(43, 23);
            this.btnProductCountClear2.TabIndex = 18;
            this.btnProductCountClear2.Text = "清零(&C)";
            this.btnProductCountClear2.UseVisualStyleBackColor = true;
            this.btnProductCountClear2.Click += new System.EventHandler(this.BtnProductCountClear_Click);
            // 
            // lbLineName1
            // 
            this.lbLineName1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lbLineName1.AutoSize = true;
            this.lbLineName1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbLineName1.Location = new System.Drawing.Point(3, 44);
            this.lbLineName1.Margin = new System.Windows.Forms.Padding(3);
            this.lbLineName1.Name = "lbLineName1";
            this.lbLineName1.Size = new System.Drawing.Size(78, 17);
            this.lbLineName1.TabIndex = 19;
            this.lbLineName1.Text = "1#线";
            this.lbLineName1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbLineName2
            // 
            this.lbLineName2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lbLineName2.AutoSize = true;
            this.lbLineName2.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbLineName2.Location = new System.Drawing.Point(3, 79);
            this.lbLineName2.Margin = new System.Windows.Forms.Padding(3);
            this.lbLineName2.Name = "lbLineName2";
            this.lbLineName2.Size = new System.Drawing.Size(78, 17);
            this.lbLineName2.TabIndex = 20;
            this.lbLineName2.Text = "2#线";
            this.lbLineName2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbProductCountAllLine2
            // 
            this.lbProductCountAllLine2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lbProductCountAllLine2.AutoSize = true;
            this.lbProductCountAllLine2.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbProductCountAllLine2.ForeColor = System.Drawing.Color.LimeGreen;
            this.lbProductCountAllLine2.Location = new System.Drawing.Point(150, 79);
            this.lbProductCountAllLine2.Margin = new System.Windows.Forms.Padding(3);
            this.lbProductCountAllLine2.Name = "lbProductCountAllLine2";
            this.lbProductCountAllLine2.Size = new System.Drawing.Size(57, 17);
            this.lbProductCountAllLine2.TabIndex = 21;
            this.lbProductCountAllLine2.Text = "0";
            this.lbProductCountAllLine2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbProductCountLine1
            // 
            this.lbProductCountLine1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lbProductCountLine1.AutoSize = true;
            this.lbProductCountLine1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbProductCountLine1.ForeColor = System.Drawing.Color.LimeGreen;
            this.lbProductCountLine1.Location = new System.Drawing.Point(87, 44);
            this.lbProductCountLine1.Margin = new System.Windows.Forms.Padding(3);
            this.lbProductCountLine1.Name = "lbProductCountLine1";
            this.lbProductCountLine1.Size = new System.Drawing.Size(57, 17);
            this.lbProductCountLine1.TabIndex = 22;
            this.lbProductCountLine1.Text = "0";
            this.lbProductCountLine1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbProductCountAllLine1
            // 
            this.lbProductCountAllLine1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lbProductCountAllLine1.AutoSize = true;
            this.lbProductCountAllLine1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbProductCountAllLine1.ForeColor = System.Drawing.Color.LimeGreen;
            this.lbProductCountAllLine1.Location = new System.Drawing.Point(150, 44);
            this.lbProductCountAllLine1.Margin = new System.Windows.Forms.Padding(3);
            this.lbProductCountAllLine1.Name = "lbProductCountAllLine1";
            this.lbProductCountAllLine1.Size = new System.Drawing.Size(57, 17);
            this.lbProductCountAllLine1.TabIndex = 23;
            this.lbProductCountAllLine1.Text = "0";
            this.lbProductCountAllLine1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbProductCountLine2
            // 
            this.lbProductCountLine2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lbProductCountLine2.AutoSize = true;
            this.lbProductCountLine2.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbProductCountLine2.ForeColor = System.Drawing.Color.LimeGreen;
            this.lbProductCountLine2.Location = new System.Drawing.Point(87, 79);
            this.lbProductCountLine2.Margin = new System.Windows.Forms.Padding(3);
            this.lbProductCountLine2.Name = "lbProductCountLine2";
            this.lbProductCountLine2.Size = new System.Drawing.Size(57, 17);
            this.lbProductCountLine2.TabIndex = 24;
            this.lbProductCountLine2.Text = "0";
            this.lbProductCountLine2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
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
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnProductCountClear1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnProductCountClear2;
        private System.Windows.Forms.Label lbLineName2;
        private System.Windows.Forms.Label lbLineName1;
        private System.Windows.Forms.Label lbProductCountLine1;
        private System.Windows.Forms.Label lbProductCountAllLine1;
        private System.Windows.Forms.Label lbProductCountAllLine2;
        private System.Windows.Forms.Label lbProductCountLine2;
    }
}
