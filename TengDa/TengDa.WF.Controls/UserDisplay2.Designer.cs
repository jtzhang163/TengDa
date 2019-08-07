namespace TengDa.WF.Controls
{
    partial class UserDisplay2
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
            this.gbUser = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel20 = new System.Windows.Forms.TableLayoutPanel();
            this.label5 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lbUserName = new System.Windows.Forms.Label();
            this.lbUserGroupName = new System.Windows.Forms.Label();
            this.gbUser.SuspendLayout();
            this.tableLayoutPanel20.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbUser
            // 
            this.gbUser.BackColor = System.Drawing.SystemColors.Control;
            this.gbUser.Controls.Add(this.tableLayoutPanel20);
            this.gbUser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbUser.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.gbUser.Location = new System.Drawing.Point(0, 0);
            this.gbUser.Name = "gbUser";
            this.gbUser.Size = new System.Drawing.Size(288, 115);
            this.gbUser.TabIndex = 16;
            this.gbUser.TabStop = false;
            this.gbUser.Text = "用户";
            // 
            // tableLayoutPanel20
            // 
            this.tableLayoutPanel20.ColumnCount = 2;
            this.tableLayoutPanel20.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 55F));
            this.tableLayoutPanel20.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel20.Controls.Add(this.label5, 0, 1);
            this.tableLayoutPanel20.Controls.Add(this.label2, 0, 0);
            this.tableLayoutPanel20.Controls.Add(this.lbUserName, 1, 0);
            this.tableLayoutPanel20.Controls.Add(this.lbUserGroupName, 1, 1);
            this.tableLayoutPanel20.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel20.Location = new System.Drawing.Point(3, 17);
            this.tableLayoutPanel20.Name = "tableLayoutPanel20";
            this.tableLayoutPanel20.RowCount = 2;
            this.tableLayoutPanel20.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel20.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel20.Size = new System.Drawing.Size(282, 95);
            this.tableLayoutPanel20.TabIndex = 7;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label5.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(10, 50);
            this.label5.Margin = new System.Windows.Forms.Padding(10, 3, 3, 3);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(42, 42);
            this.label5.TabIndex = 5;
            this.label5.Text = "组别：";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(10, 3);
            this.label2.Margin = new System.Windows.Forms.Padding(10, 3, 3, 3);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(42, 41);
            this.label2.TabIndex = 3;
            this.label2.Text = "用户：";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbUserName
            // 
            this.lbUserName.AutoSize = true;
            this.lbUserName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbUserName.Font = new System.Drawing.Font("黑体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbUserName.ForeColor = System.Drawing.Color.Green;
            this.lbUserName.Location = new System.Drawing.Point(58, 0);
            this.lbUserName.Name = "lbUserName";
            this.lbUserName.Size = new System.Drawing.Size(221, 47);
            this.lbUserName.TabIndex = 4;
            this.lbUserName.Text = "XXXXXXXX";
            this.lbUserName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbUserGroupName
            // 
            this.lbUserGroupName.AutoSize = true;
            this.lbUserGroupName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbUserGroupName.Font = new System.Drawing.Font("黑体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbUserGroupName.ForeColor = System.Drawing.Color.Green;
            this.lbUserGroupName.Location = new System.Drawing.Point(58, 47);
            this.lbUserGroupName.Name = "lbUserGroupName";
            this.lbUserGroupName.Size = new System.Drawing.Size(221, 48);
            this.lbUserGroupName.TabIndex = 6;
            this.lbUserGroupName.Text = "XXXXXXXX";
            this.lbUserGroupName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // UserDisplay2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gbUser);
            this.Name = "UserDisplay2";
            this.Size = new System.Drawing.Size(288, 115);
            this.gbUser.ResumeLayout(false);
            this.tableLayoutPanel20.ResumeLayout(false);
            this.tableLayoutPanel20.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbUser;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel20;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lbUserName;
        private System.Windows.Forms.Label lbUserGroupName;
    }
}
