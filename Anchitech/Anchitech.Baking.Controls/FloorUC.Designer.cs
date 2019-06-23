namespace Anchitech.Baking.Controls
{
    partial class FloorUC
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FloorUC));
            this.tlpFloor = new System.Windows.Forms.TableLayoutPanel();
            this.lbInfoTop = new System.Windows.Forms.Label();
            this.pbRunTime = new System.Windows.Forms.ProgressBar();
            this.lbStatus = new System.Windows.Forms.Label();
            this.cmsFloor = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmRemoteControl = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmAlarmReset = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmOvenOpenDoor = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmOvenCloseDoor = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmLoadVacuum = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmCancelLoadVacuum = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmUploadVacuum = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmCancelUploadVacuum = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmStartBaking = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmStopBaking = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmClearRunTime = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmWatContentResult = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmWatContentTestOK = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmWatContentTestNG = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmParamSetting = new System.Windows.Forms.ToolStripMenuItem();
            this.tlpFloor.SuspendLayout();
            this.cmsFloor.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpFloor
            // 
            this.tlpFloor.BackColor = System.Drawing.SystemColors.Control;
            this.tlpFloor.ColumnCount = 3;
            this.tlpFloor.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpFloor.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 1F));
            this.tlpFloor.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpFloor.Controls.Add(this.lbInfoTop, 0, 0);
            this.tlpFloor.Controls.Add(this.pbRunTime, 0, 1);
            this.tlpFloor.Controls.Add(this.lbStatus, 0, 2);
            this.tlpFloor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpFloor.Location = new System.Drawing.Point(0, 0);
            this.tlpFloor.Margin = new System.Windows.Forms.Padding(0);
            this.tlpFloor.Name = "tlpFloor";
            this.tlpFloor.RowCount = 3;
            this.tlpFloor.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpFloor.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 5F));
            this.tlpFloor.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpFloor.Size = new System.Drawing.Size(150, 150);
            this.tlpFloor.TabIndex = 13;
            this.tlpFloor.CellPaint += new System.Windows.Forms.TableLayoutCellPaintEventHandler(this.TlpFloor_CellPaint);
            // 
            // lbInfoTop
            // 
            this.lbInfoTop.AutoSize = true;
            this.lbInfoTop.BackColor = System.Drawing.Color.Transparent;
            this.tlpFloor.SetColumnSpan(this.lbInfoTop, 3);
            this.lbInfoTop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbInfoTop.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbInfoTop.ForeColor = System.Drawing.Color.Red;
            this.lbInfoTop.Location = new System.Drawing.Point(0, 5);
            this.lbInfoTop.Margin = new System.Windows.Forms.Padding(0, 5, 0, 5);
            this.lbInfoTop.Name = "lbInfoTop";
            this.lbInfoTop.Size = new System.Drawing.Size(150, 62);
            this.lbInfoTop.TabIndex = 13;
            this.lbInfoTop.Text = "0.0℃ 10000Pa 0.0℃";
            this.lbInfoTop.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pbRunTime
            // 
            this.pbRunTime.BackColor = System.Drawing.Color.Yellow;
            this.tlpFloor.SetColumnSpan(this.pbRunTime, 3);
            this.pbRunTime.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbRunTime.ForeColor = System.Drawing.Color.YellowGreen;
            this.pbRunTime.Location = new System.Drawing.Point(10, 72);
            this.pbRunTime.Margin = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.pbRunTime.Name = "pbRunTime";
            this.pbRunTime.Size = new System.Drawing.Size(130, 5);
            this.pbRunTime.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.pbRunTime.TabIndex = 9;
            // 
            // lbStatus
            // 
            this.lbStatus.AutoSize = true;
            this.lbStatus.BackColor = System.Drawing.Color.Transparent;
            this.tlpFloor.SetColumnSpan(this.lbStatus, 3);
            this.lbStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbStatus.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbStatus.ForeColor = System.Drawing.SystemColors.WindowText;
            this.lbStatus.Location = new System.Drawing.Point(0, 82);
            this.lbStatus.Margin = new System.Windows.Forms.Padding(0, 5, 0, 5);
            this.lbStatus.Name = "lbStatus";
            this.lbStatus.Size = new System.Drawing.Size(150, 63);
            this.lbStatus.TabIndex = 17;
            this.lbStatus.Text = "右 关闭 100/200 左";
            this.lbStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cmsFloor
            // 
            this.cmsFloor.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmRemoteControl,
            this.tsmWatContentResult,
            this.tsmParamSetting});
            this.cmsFloor.Name = "cmsInOutOven";
            this.cmsFloor.Size = new System.Drawing.Size(181, 92);
            this.cmsFloor.Opening += new System.ComponentModel.CancelEventHandler(this.CmsFloor_Opening);
            // 
            // tsmRemoteControl
            // 
            this.tsmRemoteControl.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmAlarmReset,
            this.tsmOvenOpenDoor,
            this.tsmOvenCloseDoor,
            this.tsmLoadVacuum,
            this.tsmCancelLoadVacuum,
            this.tsmUploadVacuum,
            this.tsmCancelUploadVacuum,
            this.tsmStartBaking,
            this.tsmStopBaking,
            this.tsmClearRunTime});
            this.tsmRemoteControl.Name = "tsmRemoteControl";
            this.tsmRemoteControl.Size = new System.Drawing.Size(180, 22);
            this.tsmRemoteControl.Text = "远程控制";
            // 
            // tsmAlarmReset
            // 
            this.tsmAlarmReset.Name = "tsmAlarmReset";
            this.tsmAlarmReset.Size = new System.Drawing.Size(148, 22);
            this.tsmAlarmReset.Text = "报警复位";
            this.tsmAlarmReset.Click += new System.EventHandler(this.TsmAlarmReset_Click);
            // 
            // tsmOvenOpenDoor
            // 
            this.tsmOvenOpenDoor.Name = "tsmOvenOpenDoor";
            this.tsmOvenOpenDoor.Size = new System.Drawing.Size(148, 22);
            this.tsmOvenOpenDoor.Text = "开门";
            this.tsmOvenOpenDoor.Click += new System.EventHandler(this.TsmOvenOpenDoor_Click);
            // 
            // tsmOvenCloseDoor
            // 
            this.tsmOvenCloseDoor.Name = "tsmOvenCloseDoor";
            this.tsmOvenCloseDoor.Size = new System.Drawing.Size(148, 22);
            this.tsmOvenCloseDoor.Text = "关门";
            this.tsmOvenCloseDoor.Click += new System.EventHandler(this.TsmOvenCloseDoor_Click);
            // 
            // tsmLoadVacuum
            // 
            this.tsmLoadVacuum.Enabled = false;
            this.tsmLoadVacuum.Name = "tsmLoadVacuum";
            this.tsmLoadVacuum.Size = new System.Drawing.Size(148, 22);
            this.tsmLoadVacuum.Text = "抽真空";
            this.tsmLoadVacuum.Click += new System.EventHandler(this.TsmLoadVacuum_Click);
            // 
            // tsmCancelLoadVacuum
            // 
            this.tsmCancelLoadVacuum.Enabled = false;
            this.tsmCancelLoadVacuum.Name = "tsmCancelLoadVacuum";
            this.tsmCancelLoadVacuum.Size = new System.Drawing.Size(148, 22);
            this.tsmCancelLoadVacuum.Text = "取消抽真空";
            this.tsmCancelLoadVacuum.Visible = false;
            this.tsmCancelLoadVacuum.Click += new System.EventHandler(this.TsmCancelLoadVacuum_Click);
            // 
            // tsmUploadVacuum
            // 
            this.tsmUploadVacuum.Enabled = false;
            this.tsmUploadVacuum.Name = "tsmUploadVacuum";
            this.tsmUploadVacuum.Size = new System.Drawing.Size(148, 22);
            this.tsmUploadVacuum.Text = "泄真空";
            this.tsmUploadVacuum.Click += new System.EventHandler(this.TsmUploadVacuum_Click);
            // 
            // tsmCancelUploadVacuum
            // 
            this.tsmCancelUploadVacuum.Enabled = false;
            this.tsmCancelUploadVacuum.Name = "tsmCancelUploadVacuum";
            this.tsmCancelUploadVacuum.Size = new System.Drawing.Size(148, 22);
            this.tsmCancelUploadVacuum.Text = "取消泄真空";
            this.tsmCancelUploadVacuum.Visible = false;
            this.tsmCancelUploadVacuum.Click += new System.EventHandler(this.TsmCancelUploadVacuum_Click);
            // 
            // tsmStartBaking
            // 
            this.tsmStartBaking.Image = global::Anchitech.Baking.Controls.Properties.Resources.Continue_Green;
            this.tsmStartBaking.Name = "tsmStartBaking";
            this.tsmStartBaking.Size = new System.Drawing.Size(148, 22);
            this.tsmStartBaking.Text = "启动";
            this.tsmStartBaking.Click += new System.EventHandler(this.TsmStartBaking_Click);
            // 
            // tsmStopBaking
            // 
            this.tsmStopBaking.Image = ((System.Drawing.Image)(resources.GetObject("tsmStopBaking.Image")));
            this.tsmStopBaking.Name = "tsmStopBaking";
            this.tsmStopBaking.Size = new System.Drawing.Size(148, 22);
            this.tsmStopBaking.Text = "停止";
            this.tsmStopBaking.Click += new System.EventHandler(this.TsmStopBaking_Click);
            // 
            // tsmClearRunTime
            // 
            this.tsmClearRunTime.Name = "tsmClearRunTime";
            this.tsmClearRunTime.Size = new System.Drawing.Size(148, 22);
            this.tsmClearRunTime.Text = "运行时间清零";
            this.tsmClearRunTime.Visible = false;
            this.tsmClearRunTime.Click += new System.EventHandler(this.TsmClearRunTime_Click);
            // 
            // tsmWatContentResult
            // 
            this.tsmWatContentResult.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmWatContentTestOK,
            this.tsmWatContentTestNG});
            this.tsmWatContentResult.Name = "tsmWatContentResult";
            this.tsmWatContentResult.Size = new System.Drawing.Size(180, 22);
            this.tsmWatContentResult.Text = "水分测试结果";
            // 
            // tsmWatContentTestOK
            // 
            this.tsmWatContentTestOK.Name = "tsmWatContentTestOK";
            this.tsmWatContentTestOK.Size = new System.Drawing.Size(180, 22);
            this.tsmWatContentTestOK.Text = "结果OK";
            this.tsmWatContentTestOK.Click += new System.EventHandler(this.TsmWatContentTestOK_Click);
            // 
            // tsmWatContentTestNG
            // 
            this.tsmWatContentTestNG.Name = "tsmWatContentTestNG";
            this.tsmWatContentTestNG.Size = new System.Drawing.Size(180, 22);
            this.tsmWatContentTestNG.Text = "结果NG";
            this.tsmWatContentTestNG.Click += new System.EventHandler(this.TsmWatContentTestNG_Click);
            // 
            // tsmParamSetting
            // 
            this.tsmParamSetting.Name = "tsmParamSetting";
            this.tsmParamSetting.Size = new System.Drawing.Size(180, 22);
            this.tsmParamSetting.Text = "参数设置...";
            this.tsmParamSetting.Click += new System.EventHandler(this.TsmParamSetting_Click);
            // 
            // FloorUC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ContextMenuStrip = this.cmsFloor;
            this.Controls.Add(this.tlpFloor);
            this.Name = "FloorUC";
            this.tlpFloor.ResumeLayout(false);
            this.tlpFloor.PerformLayout();
            this.cmsFloor.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpFloor;
        private System.Windows.Forms.Label lbInfoTop;
        private System.Windows.Forms.ProgressBar pbRunTime;
        private System.Windows.Forms.Label lbStatus;
        private System.Windows.Forms.ContextMenuStrip cmsFloor;
        private System.Windows.Forms.ToolStripMenuItem tsmRemoteControl;
        private System.Windows.Forms.ToolStripMenuItem tsmAlarmReset;
        private System.Windows.Forms.ToolStripMenuItem tsmOvenOpenDoor;
        private System.Windows.Forms.ToolStripMenuItem tsmOvenCloseDoor;
        private System.Windows.Forms.ToolStripMenuItem tsmStartBaking;
        private System.Windows.Forms.ToolStripMenuItem tsmLoadVacuum;
        private System.Windows.Forms.ToolStripMenuItem tsmCancelLoadVacuum;
        private System.Windows.Forms.ToolStripMenuItem tsmUploadVacuum;
        private System.Windows.Forms.ToolStripMenuItem tsmCancelUploadVacuum;
        private System.Windows.Forms.ToolStripMenuItem tsmStopBaking;
        private System.Windows.Forms.ToolStripMenuItem tsmClearRunTime;
        private System.Windows.Forms.ToolStripMenuItem tsmWatContentResult;
        private System.Windows.Forms.ToolStripMenuItem tsmWatContentTestOK;
        private System.Windows.Forms.ToolStripMenuItem tsmWatContentTestNG;
        private System.Windows.Forms.ToolStripMenuItem tsmParamSetting;
    }
}
