namespace CAMEL.Baking.Control
{
    partial class BatteryBindInitUC
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
            this.btnBatteryBindInit = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnBatteryBindInit
            // 
            this.btnBatteryBindInit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnBatteryBindInit.Location = new System.Drawing.Point(0, 0);
            this.btnBatteryBindInit.Name = "btnBatteryBindInit";
            this.btnBatteryBindInit.Size = new System.Drawing.Size(186, 28);
            this.btnBatteryBindInit.TabIndex = 0;
            this.btnBatteryBindInit.Text = "电池绑定初始化";
            this.btnBatteryBindInit.UseVisualStyleBackColor = true;
            this.btnBatteryBindInit.Click += new System.EventHandler(this.BtnBatteryBindInit_Click);
            // 
            // BatteryBindInitUC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnBatteryBindInit);
            this.Name = "BatteryBindInitUC";
            this.Size = new System.Drawing.Size(186, 28);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnBatteryBindInit;
    }
}
