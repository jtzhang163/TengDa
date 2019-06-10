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
            this.tlpFloor = new System.Windows.Forms.TableLayoutPanel();
            this.lbInfoTop = new System.Windows.Forms.Label();
            this.pbRunTime = new System.Windows.Forms.ProgressBar();
            this.lbStatus = new System.Windows.Forms.Label();
            this.tlpFloor.SuspendLayout();
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
            // 
            // lbInfoTop
            // 
            this.lbInfoTop.AutoSize = true;
            this.lbInfoTop.BackColor = System.Drawing.Color.Transparent;
            this.tlpFloor.SetColumnSpan(this.lbInfoTop, 3);
            this.lbInfoTop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbInfoTop.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbInfoTop.ForeColor = System.Drawing.Color.Red;
            this.lbInfoTop.Location = new System.Drawing.Point(0, 8);
            this.lbInfoTop.Margin = new System.Windows.Forms.Padding(0, 8, 0, 8);
            this.lbInfoTop.Name = "lbInfoTop";
            this.lbInfoTop.Size = new System.Drawing.Size(150, 56);
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
            this.lbStatus.Location = new System.Drawing.Point(0, 85);
            this.lbStatus.Margin = new System.Windows.Forms.Padding(0, 8, 0, 8);
            this.lbStatus.Name = "lbStatus";
            this.lbStatus.Size = new System.Drawing.Size(150, 57);
            this.lbStatus.TabIndex = 17;
            this.lbStatus.Text = "右 关闭 100/200 左";
            this.lbStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // FloorUC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tlpFloor);
            this.Name = "FloorUC";
            this.tlpFloor.ResumeLayout(false);
            this.tlpFloor.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpFloor;
        private System.Windows.Forms.Label lbInfoTop;
        private System.Windows.Forms.ProgressBar pbRunTime;
        private System.Windows.Forms.Label lbStatus;
    }
}
