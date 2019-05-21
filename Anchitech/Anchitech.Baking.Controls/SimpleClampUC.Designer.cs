using System.Windows.Forms;

namespace Anchitech.Baking.Controls
{
    partial class SimpleClampUC
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
            this.lbClampCode = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lbClampCode
            // 
            this.lbClampCode.BackColor = System.Drawing.Color.Transparent;
            this.lbClampCode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbClampCode.Font = new System.Drawing.Font("Microsoft YaHei UI", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbClampCode.ForeColor = System.Drawing.Color.Green;
            this.lbClampCode.Location = new System.Drawing.Point(0, 0);
            this.lbClampCode.Margin = new System.Windows.Forms.Padding(0);
            this.lbClampCode.Name = "lbClampCode";
            this.lbClampCode.Size = new System.Drawing.Size(81, 47);
            this.lbClampCode.TabIndex = 4;
            this.lbClampCode.Text = "XXX";
            this.lbClampCode.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SimpleClampUC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lbClampCode);
            this.Name = "SimpleClampUC";
            this.Size = new System.Drawing.Size(81, 47);
            this.ResumeLayout(false);

        }

        #endregion

        private Label LabelClampCode
        {
            get
            {
                return this.lbClampCode;
            }
            set
            {
                this.lbClampCode = value;
            }
        }

        private System.Windows.Forms.Label lbClampCode;
    }
}
