namespace Anchitech.Baking.Controls
{
    partial class MachineStatusUC
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MachineStatusUC));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.pbLamp = new System.Windows.Forms.PictureBox();
            this.lbMachineName = new System.Windows.Forms.Label();
            this.cbIsEnable = new System.Windows.Forms.CheckBox();
            this.tbStatus = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbLamp)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 18F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 66.66666F));
            this.tableLayoutPanel1.Controls.Add(this.pbLamp, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.lbMachineName, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.cbIsEnable, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tbStatus, 3, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(318, 30);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // pbLamp
            // 
            this.pbLamp.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.pbLamp.Image = ((System.Drawing.Image)(resources.GetObject("pbLamp.Image")));
            this.pbLamp.Location = new System.Drawing.Point(109, 8);
            this.pbLamp.Margin = new System.Windows.Forms.Padding(0);
            this.pbLamp.Name = "pbLamp";
            this.pbLamp.Size = new System.Drawing.Size(17, 13);
            this.pbLamp.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbLamp.TabIndex = 24;
            this.pbLamp.TabStop = false;
            // 
            // lbMachineName
            // 
            this.lbMachineName.AutoSize = true;
            this.lbMachineName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbMachineName.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbMachineName.Location = new System.Drawing.Point(21, 0);
            this.lbMachineName.Name = "lbMachineName";
            this.lbMachineName.Size = new System.Drawing.Size(85, 30);
            this.lbMachineName.TabIndex = 23;
            this.lbMachineName.Text = "XXXXX";
            this.lbMachineName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cbIsEnable
            // 
            this.cbIsEnable.AutoSize = true;
            this.cbIsEnable.Dock = System.Windows.Forms.DockStyle.Left;
            this.cbIsEnable.Location = new System.Drawing.Point(0, 0);
            this.cbIsEnable.Margin = new System.Windows.Forms.Padding(0);
            this.cbIsEnable.Name = "cbIsEnable";
            this.cbIsEnable.Size = new System.Drawing.Size(15, 30);
            this.cbIsEnable.TabIndex = 22;
            this.cbIsEnable.UseVisualStyleBackColor = true;
            this.cbIsEnable.CheckedChanged += new System.EventHandler(this.CbIsEnable_CheckedChanged);
            // 
            // tbStatus
            // 
            this.tbStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tbStatus.BackColor = System.Drawing.SystemColors.Control;
            this.tbStatus.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbStatus.Font = new System.Drawing.Font("黑体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbStatus.ForeColor = System.Drawing.Color.Green;
            this.tbStatus.Location = new System.Drawing.Point(134, 7);
            this.tbStatus.Margin = new System.Windows.Forms.Padding(0);
            this.tbStatus.Name = "tbStatus";
            this.tbStatus.ReadOnly = true;
            this.tbStatus.Size = new System.Drawing.Size(184, 16);
            this.tbStatus.TabIndex = 25;
            this.tbStatus.Text = "未连接";
            // 
            // MachineStatusUC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "MachineStatusUC";
            this.Size = new System.Drawing.Size(318, 30);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbLamp)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.CheckBox cbIsEnable;
        private System.Windows.Forms.Label lbMachineName;
        private System.Windows.Forms.PictureBox pbLamp;
        private System.Windows.Forms.TextBox tbStatus;
    }
}
