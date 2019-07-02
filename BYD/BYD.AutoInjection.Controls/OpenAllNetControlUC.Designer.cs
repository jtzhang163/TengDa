namespace BYD.AutoInjection.Controls
{
    partial class OpenAllNetControlUC
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
            this.btnOpenAllNetControl = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnOpenAllNetControl
            // 
            this.btnOpenAllNetControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnOpenAllNetControl.Location = new System.Drawing.Point(0, 0);
            this.btnOpenAllNetControl.Name = "btnOpenAllNetControl";
            this.btnOpenAllNetControl.Size = new System.Drawing.Size(186, 28);
            this.btnOpenAllNetControl.TabIndex = 0;
            this.btnOpenAllNetControl.Text = "打开全部网控";
            this.btnOpenAllNetControl.UseVisualStyleBackColor = true;
            this.btnOpenAllNetControl.Click += new System.EventHandler(this.BtnOpenAllNetControl_Click);
            // 
            // OpenAllNetControlUC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnOpenAllNetControl);
            this.Name = "OpenAllNetControlUC";
            this.Size = new System.Drawing.Size(186, 28);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnOpenAllNetControl;
    }
}
