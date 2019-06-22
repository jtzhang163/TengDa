using System.Windows.Forms;

namespace Anchitech.Baking.Controls
{
    partial class TransferUC
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lbFromStation = new System.Windows.Forms.Label();
            this.lbName = new System.Windows.Forms.Label();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.simpleClampUC1 = new Anchitech.Baking.Controls.SimpleClampUC();
            this.cmsTransfer = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmGetSampleFinished = new System.Windows.Forms.ToolStripMenuItem();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.cmsTransfer.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.lbFromStation, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.lbName, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(123, 114);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // lbFromStation
            // 
            this.lbFromStation.AutoSize = true;
            this.lbFromStation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbFromStation.Font = new System.Drawing.Font("微软雅黑", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbFromStation.ForeColor = System.Drawing.Color.Violet;
            this.lbFromStation.Location = new System.Drawing.Point(3, 89);
            this.lbFromStation.Name = "lbFromStation";
            this.lbFromStation.Size = new System.Drawing.Size(117, 25);
            this.lbFromStation.TabIndex = 26;
            this.lbFromStation.Text = "来源";
            this.lbFromStation.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbName
            // 
            this.lbName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbName.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbName.Location = new System.Drawing.Point(3, 0);
            this.lbName.Name = "lbName";
            this.lbName.Size = new System.Drawing.Size(117, 25);
            this.lbName.TabIndex = 9;
            this.lbName.Text = "转移台";
            this.lbName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel2.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Outset;
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.simpleClampUC1, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 28);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(117, 58);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // simpleClampUC1
            // 
            this.simpleClampUC1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.simpleClampUC1.Location = new System.Drawing.Point(2, 2);
            this.simpleClampUC1.Margin = new System.Windows.Forms.Padding(0);
            this.simpleClampUC1.Name = "simpleClampUC1";
            this.simpleClampUC1.Size = new System.Drawing.Size(113, 54);
            this.simpleClampUC1.TabIndex = 0;
            // 
            // cmsTransfer
            // 
            this.cmsTransfer.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1});
            this.cmsTransfer.Name = "cmsInOutOven";
            this.cmsTransfer.Size = new System.Drawing.Size(181, 48);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmGetSampleFinished});
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(180, 22);
            this.toolStripMenuItem1.Text = "控制";
            // 
            // tsmGetSampleFinished
            // 
            this.tsmGetSampleFinished.Name = "tsmGetSampleFinished";
            this.tsmGetSampleFinished.Size = new System.Drawing.Size(180, 22);
            this.tsmGetSampleFinished.Text = "取完水分可回炉";
            this.tsmGetSampleFinished.Click += new System.EventHandler(this.TsmGetSampleFinished_Click);
            // 
            // TransferUC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.ContextMenuStrip = this.cmsTransfer;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "TransferUC";
            this.Size = new System.Drawing.Size(123, 114);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.cmsTransfer.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label lbName;
        private SimpleClampUC simpleClampUC1;
        private Label lbFromStation;
        private ContextMenuStrip cmsTransfer;
        private ToolStripMenuItem toolStripMenuItem1;
        private ToolStripMenuItem tsmGetSampleFinished;
    }
}
