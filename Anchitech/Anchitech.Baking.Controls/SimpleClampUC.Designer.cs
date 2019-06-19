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
            this.tlpClamp = new System.Windows.Forms.TableLayoutPanel();
            this.lbClampCode = new System.Windows.Forms.Label();
            this.tlpClamp.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpClamp
            // 
            this.tlpClamp.BackColor = System.Drawing.Color.Transparent;
            this.tlpClamp.ColumnCount = 1;
            this.tlpClamp.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpClamp.Controls.Add(this.lbClampCode, 0, 0);
            this.tlpClamp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpClamp.Location = new System.Drawing.Point(0, 0);
            this.tlpClamp.Margin = new System.Windows.Forms.Padding(0);
            this.tlpClamp.Name = "tlpClamp";
            this.tlpClamp.RowCount = 1;
            this.tlpClamp.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpClamp.Size = new System.Drawing.Size(81, 47);
            this.tlpClamp.TabIndex = 0;
            // 
            // lbClampCode
            // 
            this.lbClampCode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbClampCode.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold);
            this.lbClampCode.ForeColor = System.Drawing.Color.Green;
            this.lbClampCode.Location = new System.Drawing.Point(7, 7);
            this.lbClampCode.Margin = new System.Windows.Forms.Padding(7);
            this.lbClampCode.Name = "lbClampCode";
            this.lbClampCode.Size = new System.Drawing.Size(67, 33);
            this.lbClampCode.TabIndex = 0;
            this.lbClampCode.Text = "XXXX";
            this.lbClampCode.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SimpleClampUC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tlpClamp);
            this.Name = "SimpleClampUC";
            this.Size = new System.Drawing.Size(81, 47);
            this.tlpClamp.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private TableLayoutPanel tlpClamp;
        private Label lbClampCode;

    }
}
