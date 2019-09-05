namespace CAMEL.Baking.Control
{
    partial class ActivationUC
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
            this.lbActivationMsg = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lbActivationMsg
            // 
            this.lbActivationMsg.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lbActivationMsg.AutoSize = true;
            this.lbActivationMsg.BackColor = System.Drawing.Color.Red;
            this.lbActivationMsg.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbActivationMsg.ForeColor = System.Drawing.Color.White;
            this.lbActivationMsg.Location = new System.Drawing.Point(14, 13);
            this.lbActivationMsg.Name = "lbActivationMsg";
            this.lbActivationMsg.Size = new System.Drawing.Size(266, 21);
            this.lbActivationMsg.TabIndex = 0;
            this.lbActivationMsg.Text = "程序即将过期，点击此处输入激活码";
            this.lbActivationMsg.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbActivationMsg.Click += new System.EventHandler(this.LbActivationMsg_Click);
            // 
            // ActivationUC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lbActivationMsg);
            this.Name = "ActivationUC";
            this.Size = new System.Drawing.Size(300, 50);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lbActivationMsg;
    }
}
