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
            this.tlpFloor1001 = new System.Windows.Forms.TableLayoutPanel();
            this.lbFloorInfoTop1001 = new System.Windows.Forms.Label();
            this.pbRunTime1001 = new System.Windows.Forms.ProgressBar();
            this.lbFloorStatus1001 = new System.Windows.Forms.Label();
            this.tlpFloor1001.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpFloor1001
            // 
            this.tlpFloor1001.BackColor = System.Drawing.SystemColors.Control;
            this.tlpFloor1001.ColumnCount = 3;
            this.tlpFloor1001.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpFloor1001.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 1F));
            this.tlpFloor1001.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpFloor1001.Controls.Add(this.lbFloorInfoTop1001, 0, 0);
            this.tlpFloor1001.Controls.Add(this.pbRunTime1001, 0, 1);
            this.tlpFloor1001.Controls.Add(this.lbFloorStatus1001, 0, 2);
            this.tlpFloor1001.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpFloor1001.Location = new System.Drawing.Point(0, 0);
            this.tlpFloor1001.Margin = new System.Windows.Forms.Padding(0);
            this.tlpFloor1001.Name = "tlpFloor1001";
            this.tlpFloor1001.RowCount = 3;
            this.tlpFloor1001.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpFloor1001.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 8F));
            this.tlpFloor1001.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpFloor1001.Size = new System.Drawing.Size(150, 150);
            this.tlpFloor1001.TabIndex = 13;
            // 
            // lbFloorInfoTop1001
            // 
            this.lbFloorInfoTop1001.AutoSize = true;
            this.lbFloorInfoTop1001.BackColor = System.Drawing.Color.Transparent;
            this.tlpFloor1001.SetColumnSpan(this.lbFloorInfoTop1001, 3);
            this.lbFloorInfoTop1001.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbFloorInfoTop1001.Font = new System.Drawing.Font("黑体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbFloorInfoTop1001.ForeColor = System.Drawing.Color.Red;
            this.lbFloorInfoTop1001.Location = new System.Drawing.Point(0, 8);
            this.lbFloorInfoTop1001.Margin = new System.Windows.Forms.Padding(0, 8, 0, 8);
            this.lbFloorInfoTop1001.Name = "lbFloorInfoTop1001";
            this.lbFloorInfoTop1001.Size = new System.Drawing.Size(150, 55);
            this.lbFloorInfoTop1001.TabIndex = 13;
            this.lbFloorInfoTop1001.Text = "0.0℃ 10000Pa 0.0℃";
            this.lbFloorInfoTop1001.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pbRunTime1001
            // 
            this.pbRunTime1001.BackColor = System.Drawing.Color.Yellow;
            this.tlpFloor1001.SetColumnSpan(this.pbRunTime1001, 3);
            this.pbRunTime1001.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbRunTime1001.ForeColor = System.Drawing.Color.YellowGreen;
            this.pbRunTime1001.Location = new System.Drawing.Point(10, 71);
            this.pbRunTime1001.Margin = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.pbRunTime1001.Name = "pbRunTime1001";
            this.pbRunTime1001.Size = new System.Drawing.Size(130, 8);
            this.pbRunTime1001.TabIndex = 9;
            // 
            // lbFloorStatus1001
            // 
            this.lbFloorStatus1001.AutoSize = true;
            this.lbFloorStatus1001.BackColor = System.Drawing.Color.Transparent;
            this.tlpFloor1001.SetColumnSpan(this.lbFloorStatus1001, 3);
            this.lbFloorStatus1001.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbFloorStatus1001.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbFloorStatus1001.ForeColor = System.Drawing.SystemColors.WindowText;
            this.lbFloorStatus1001.Location = new System.Drawing.Point(0, 87);
            this.lbFloorStatus1001.Margin = new System.Windows.Forms.Padding(0, 8, 0, 8);
            this.lbFloorStatus1001.Name = "lbFloorStatus1001";
            this.lbFloorStatus1001.Size = new System.Drawing.Size(150, 55);
            this.lbFloorStatus1001.TabIndex = 17;
            this.lbFloorStatus1001.Text = "右 关闭 100/200 左";
            this.lbFloorStatus1001.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // FloorUC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tlpFloor1001);
            this.Name = "FloorUC";
            this.tlpFloor1001.ResumeLayout(false);
            this.tlpFloor1001.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpFloor1001;
        private System.Windows.Forms.Label lbFloorInfoTop1001;
        private System.Windows.Forms.ProgressBar pbRunTime1001;
        private System.Windows.Forms.Label lbFloorStatus1001;
    }
}
