namespace CAMEL.Baking.Control
{
    partial class RGVUC
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
            this.components = new System.ComponentModel.Container();
            this.panelRGV = new System.Windows.Forms.Panel();
            this.tableLayoutPanel21 = new System.Windows.Forms.TableLayoutPanel();
            this.lbClampCode = new System.Windows.Forms.Label();
            this.lbInfo = new System.Windows.Forms.Label();
            this.lbName = new System.Windows.Forms.Label();
            this.cmsRGV = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmManuGetStation = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmManuPutStation = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiTransAutoManu = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiStart = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiPause = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiReset = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiStop = new System.Windows.Forms.ToolStripMenuItem();
            this.panelRGV.SuspendLayout();
            this.tableLayoutPanel21.SuspendLayout();
            this.cmsRGV.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelRGV
            // 
            this.panelRGV.BackColor = System.Drawing.SystemColors.Control;
            this.panelRGV.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelRGV.Controls.Add(this.tableLayoutPanel21);
            this.panelRGV.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelRGV.Location = new System.Drawing.Point(0, 0);
            this.panelRGV.Margin = new System.Windows.Forms.Padding(3, 8, 3, 8);
            this.panelRGV.Name = "panelRGV";
            this.panelRGV.Size = new System.Drawing.Size(150, 150);
            this.panelRGV.TabIndex = 2;
            // 
            // tableLayoutPanel21
            // 
            this.tableLayoutPanel21.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel21.ColumnCount = 1;
            this.tableLayoutPanel21.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel21.Controls.Add(this.lbClampCode, 0, 2);
            this.tableLayoutPanel21.Controls.Add(this.lbInfo, 0, 1);
            this.tableLayoutPanel21.Controls.Add(this.lbName, 0, 0);
            this.tableLayoutPanel21.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel21.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel21.Name = "tableLayoutPanel21";
            this.tableLayoutPanel21.RowCount = 3;
            this.tableLayoutPanel21.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel21.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tableLayoutPanel21.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel21.Size = new System.Drawing.Size(148, 148);
            this.tableLayoutPanel21.TabIndex = 0;
            // 
            // lbClampCode
            // 
            this.lbClampCode.AutoSize = true;
            this.lbClampCode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbClampCode.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbClampCode.ForeColor = System.Drawing.Color.Transparent;
            this.lbClampCode.Location = new System.Drawing.Point(0, 103);
            this.lbClampCode.Margin = new System.Windows.Forms.Padding(0);
            this.lbClampCode.Name = "lbClampCode";
            this.lbClampCode.Size = new System.Drawing.Size(148, 45);
            this.lbClampCode.TabIndex = 3;
            this.lbClampCode.Text = "夹具条码";
            this.lbClampCode.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbInfo
            // 
            this.lbInfo.AutoSize = true;
            this.lbInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbInfo.Font = new System.Drawing.Font("黑体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbInfo.ForeColor = System.Drawing.Color.Lime;
            this.lbInfo.Location = new System.Drawing.Point(3, 44);
            this.lbInfo.Name = "lbInfo";
            this.lbInfo.Size = new System.Drawing.Size(142, 59);
            this.lbInfo.TabIndex = 1;
            this.lbInfo.Text = "取/放盘";
            this.lbInfo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbName
            // 
            this.lbName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbName.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbName.Location = new System.Drawing.Point(0, 0);
            this.lbName.Margin = new System.Windows.Forms.Padding(0);
            this.lbName.Name = "lbName";
            this.lbName.Size = new System.Drawing.Size(148, 44);
            this.lbName.TabIndex = 0;
            this.lbName.Text = "XXXX";
            this.lbName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cmsRGV
            // 
            this.cmsRGV.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmManuGetStation,
            this.tsmManuPutStation,
            this.tsmiTransAutoManu,
            this.tsmiStart,
            this.tsmiPause,
            this.tsmiReset,
            this.tsmiStop});
            this.cmsRGV.Name = "cmsRGV";
            this.cmsRGV.Size = new System.Drawing.Size(181, 180);
            this.cmsRGV.Opening += new System.ComponentModel.CancelEventHandler(this.CmsRGV_Opening);
            // 
            // tsmManuGetStation
            // 
            this.tsmManuGetStation.Name = "tsmManuGetStation";
            this.tsmManuGetStation.Size = new System.Drawing.Size(148, 22);
            this.tsmManuGetStation.Text = "手动【取盘】";
            this.tsmManuGetStation.DropDownOpening += new System.EventHandler(this.tsmManuStation_DropDownOpening);
            // 
            // tsmManuPutStation
            // 
            this.tsmManuPutStation.Name = "tsmManuPutStation";
            this.tsmManuPutStation.Size = new System.Drawing.Size(148, 22);
            this.tsmManuPutStation.Text = "手动【放盘】";
            this.tsmManuPutStation.DropDownOpening += new System.EventHandler(this.tsmManuStation_DropDownOpening);
            // 
            // tsmiTransAutoManu
            // 
            this.tsmiTransAutoManu.Image = global::CAMEL.Baking.Control.Properties.Resources.Switch_green;
            this.tsmiTransAutoManu.Name = "tsmiTransAutoManu";
            this.tsmiTransAutoManu.Size = new System.Drawing.Size(180, 22);
            this.tsmiTransAutoManu.Text = "切换为自动";
            this.tsmiTransAutoManu.Click += new System.EventHandler(this.TsmiTransAutoManu_Click);
            // 
            // tsmiStart
            // 
            this.tsmiStart.Image = global::CAMEL.Baking.Control.Properties.Resources.Continue_Green;
            this.tsmiStart.Name = "tsmiStart";
            this.tsmiStart.Size = new System.Drawing.Size(148, 22);
            this.tsmiStart.Text = "启动";
            this.tsmiStart.Click += new System.EventHandler(this.TsmiStart_Click);
            // 
            // tsmiPause
            // 
            this.tsmiPause.Image = global::CAMEL.Baking.Control.Properties.Resources.Stop_Red;
            this.tsmiPause.Name = "tsmiPause";
            this.tsmiPause.Size = new System.Drawing.Size(148, 22);
            this.tsmiPause.Text = "停止";
            this.tsmiPause.Click += new System.EventHandler(this.TsmiPause_Click);
            // 
            // tsmiReset
            // 
            this.tsmiReset.Image = global::CAMEL.Baking.Control.Properties.Resources.Reset_green;
            this.tsmiReset.Name = "tsmiReset";
            this.tsmiReset.Size = new System.Drawing.Size(148, 22);
            this.tsmiReset.Text = "复位";
            this.tsmiReset.Click += new System.EventHandler(this.TsmiReset_Click);
            // 
            // tsmiStop
            // 
            this.tsmiStop.Image = global::CAMEL.Baking.Control.Properties.Resources.emergency_stop;
            this.tsmiStop.Name = "tsmiStop";
            this.tsmiStop.Size = new System.Drawing.Size(148, 22);
            this.tsmiStop.Text = "急停";
            this.tsmiStop.Click += new System.EventHandler(this.TsmiStop_Click);
            // 
            // RGVUC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ContextMenuStrip = this.cmsRGV;
            this.Controls.Add(this.panelRGV);
            this.Name = "RGVUC";
            this.panelRGV.ResumeLayout(false);
            this.tableLayoutPanel21.ResumeLayout(false);
            this.tableLayoutPanel21.PerformLayout();
            this.cmsRGV.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelRGV;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel21;
        private System.Windows.Forms.Label lbClampCode;
        private System.Windows.Forms.Label lbInfo;
        private System.Windows.Forms.Label lbName;
        private System.Windows.Forms.ContextMenuStrip cmsRGV;
        private System.Windows.Forms.ToolStripMenuItem tsmManuGetStation;
        private System.Windows.Forms.ToolStripMenuItem tsmManuPutStation;
        private System.Windows.Forms.ToolStripMenuItem tsmiStart;
        private System.Windows.Forms.ToolStripMenuItem tsmiPause;
        private System.Windows.Forms.ToolStripMenuItem tsmiReset;
        private System.Windows.Forms.ToolStripMenuItem tsmiStop;
        private System.Windows.Forms.ToolStripMenuItem tsmiTransAutoManu;
    }
}
