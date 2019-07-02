namespace BYD.AutoInjection.Controls
{
    partial class RobotUC
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
            this.panelRobot = new System.Windows.Forms.Panel();
            this.tableLayoutPanel21 = new System.Windows.Forms.TableLayoutPanel();
            this.lbClampCode = new System.Windows.Forms.Label();
            this.lbInfo = new System.Windows.Forms.Label();
            this.lbName = new System.Windows.Forms.Label();
            this.cmsRobot = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmManuGetStation = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmManuPutStation = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmTaskIsFinish = new System.Windows.Forms.ToolStripMenuItem();
            this.panelRobot.SuspendLayout();
            this.tableLayoutPanel21.SuspendLayout();
            this.cmsRobot.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelRobot
            // 
            this.panelRobot.BackColor = System.Drawing.SystemColors.Control;
            this.panelRobot.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelRobot.Controls.Add(this.tableLayoutPanel21);
            this.panelRobot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelRobot.Location = new System.Drawing.Point(0, 0);
            this.panelRobot.Margin = new System.Windows.Forms.Padding(3, 8, 3, 8);
            this.panelRobot.Name = "panelRobot";
            this.panelRobot.Size = new System.Drawing.Size(150, 150);
            this.panelRobot.TabIndex = 2;
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
            // cmsRobot
            // 
            this.cmsRobot.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmManuGetStation,
            this.tsmManuPutStation,
            this.tsmTaskIsFinish});
            this.cmsRobot.Name = "cmsRobot";
            this.cmsRobot.Size = new System.Drawing.Size(181, 92);
            this.cmsRobot.Opening += new System.ComponentModel.CancelEventHandler(this.CmsRobot_Opening);
            // 
            // tsmManuGetStation
            // 
            this.tsmManuGetStation.Name = "tsmManuGetStation";
            this.tsmManuGetStation.Size = new System.Drawing.Size(180, 22);
            this.tsmManuGetStation.Text = "手动【取盘】";
            this.tsmManuGetStation.DropDownOpening += new System.EventHandler(this.tsmManuStation_DropDownOpening);
            // 
            // tsmManuPutStation
            // 
            this.tsmManuPutStation.Name = "tsmManuPutStation";
            this.tsmManuPutStation.Size = new System.Drawing.Size(180, 22);
            this.tsmManuPutStation.Text = "手动【放盘】";
            this.tsmManuPutStation.DropDownOpening += new System.EventHandler(this.tsmManuStation_DropDownOpening);
            // 
            // tsmTaskIsFinish
            // 
            this.tsmTaskIsFinish.Name = "tsmTaskIsFinish";
            this.tsmTaskIsFinish.Size = new System.Drawing.Size(180, 22);
            this.tsmTaskIsFinish.Text = "取放盘已结束...";
            this.tsmTaskIsFinish.Click += new System.EventHandler(this.TsmTaskIsFinish_Click);
            // 
            // RobotUC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ContextMenuStrip = this.cmsRobot;
            this.Controls.Add(this.panelRobot);
            this.Name = "RobotUC";
            this.panelRobot.ResumeLayout(false);
            this.tableLayoutPanel21.ResumeLayout(false);
            this.tableLayoutPanel21.PerformLayout();
            this.cmsRobot.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelRobot;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel21;
        private System.Windows.Forms.Label lbClampCode;
        private System.Windows.Forms.Label lbInfo;
        private System.Windows.Forms.Label lbName;
        private System.Windows.Forms.ContextMenuStrip cmsRobot;
        private System.Windows.Forms.ToolStripMenuItem tsmManuGetStation;
        private System.Windows.Forms.ToolStripMenuItem tsmManuPutStation;
        private System.Windows.Forms.ToolStripMenuItem tsmTaskIsFinish;
    }
}
