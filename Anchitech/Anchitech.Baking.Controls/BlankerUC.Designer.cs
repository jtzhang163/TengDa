﻿namespace Anchitech.Baking.Controls
{
    partial class BlankerUC
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BlankerUC));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.pbTriLamp = new System.Windows.Forms.PictureBox();
            this.lbName = new System.Windows.Forms.Label();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.lbStationName1 = new System.Windows.Forms.Label();
            this.lbStationName2 = new System.Windows.Forms.Label();
            this.cmsBlanker = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmPutFinished2 = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmPutFinished1 = new System.Windows.Forms.ToolStripMenuItem();
            this.simpleClampUC1 = new Anchitech.Baking.Controls.SimpleClampUC();
            this.simpleClampUC2 = new Anchitech.Baking.Controls.SimpleClampUC();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbTriLamp)).BeginInit();
            this.tableLayoutPanel3.SuspendLayout();
            this.cmsBlanker.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(141, 268);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel2.Controls.Add(this.pbTriLamp, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.lbName, 0, 0);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(141, 25);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // pbTriLamp
            // 
            this.pbTriLamp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbTriLamp.Image = ((System.Drawing.Image)(resources.GetObject("pbTriLamp.Image")));
            this.pbTriLamp.Location = new System.Drawing.Point(118, 2);
            this.pbTriLamp.Margin = new System.Windows.Forms.Padding(2);
            this.pbTriLamp.Name = "pbTriLamp";
            this.pbTriLamp.Size = new System.Drawing.Size(21, 21);
            this.pbTriLamp.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbTriLamp.TabIndex = 32;
            this.pbTriLamp.TabStop = false;
            // 
            // lbName
            // 
            this.lbName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbName.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbName.Location = new System.Drawing.Point(3, 0);
            this.lbName.Name = "lbName";
            this.lbName.Size = new System.Drawing.Size(110, 25);
            this.lbName.TabIndex = 9;
            this.lbName.Text = "下料机";
            this.lbName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel3.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Outset;
            this.tableLayoutPanel3.ColumnCount = 1;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Controls.Add(this.simpleClampUC1, 0, 3);
            this.tableLayoutPanel3.Controls.Add(this.lbStationName1, 0, 2);
            this.tableLayoutPanel3.Controls.Add(this.lbStationName2, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.simpleClampUC2, 0, 1);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 28);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 4;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(135, 237);
            this.tableLayoutPanel3.TabIndex = 1;
            // 
            // lbStationName1
            // 
            this.lbStationName1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbStationName1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbStationName1.Location = new System.Drawing.Point(2, 119);
            this.lbStationName1.Margin = new System.Windows.Forms.Padding(0);
            this.lbStationName1.Name = "lbStationName1";
            this.lbStationName1.Size = new System.Drawing.Size(131, 20);
            this.lbStationName1.TabIndex = 17;
            this.lbStationName1.Text = "17#下料位";
            this.lbStationName1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbStationName2
            // 
            this.lbStationName2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbStationName2.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbStationName2.Location = new System.Drawing.Point(2, 2);
            this.lbStationName2.Margin = new System.Windows.Forms.Padding(0);
            this.lbStationName2.Name = "lbStationName2";
            this.lbStationName2.Size = new System.Drawing.Size(131, 20);
            this.lbStationName2.TabIndex = 16;
            this.lbStationName2.Text = "17#下料位";
            this.lbStationName2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cmsBlanker
            // 
            this.cmsBlanker.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmPutFinished2,
            this.tsmPutFinished1});
            this.cmsBlanker.Name = "cmsInOutOven";
            this.cmsBlanker.Size = new System.Drawing.Size(181, 70);
            this.cmsBlanker.Opening += new System.ComponentModel.CancelEventHandler(this.CmsBlanker_Opening);
            // 
            // tsmPutFinished2
            // 
            this.tsmPutFinished2.Name = "tsmPutFinished2";
            this.tsmPutFinished2.Size = new System.Drawing.Size(180, 22);
            this.tsmPutFinished2.Text = "工位2放盘完成";
            this.tsmPutFinished2.Click += new System.EventHandler(this.TsmPutFinished_Click);
            // 
            // tsmPutFinished1
            // 
            this.tsmPutFinished1.Name = "tsmPutFinished1";
            this.tsmPutFinished1.Size = new System.Drawing.Size(180, 22);
            this.tsmPutFinished1.Text = "工位1放盘完成";
            this.tsmPutFinished1.Click += new System.EventHandler(this.TsmPutFinished_Click);
            // 
            // simpleClampUC1
            // 
            this.simpleClampUC1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.simpleClampUC1.Location = new System.Drawing.Point(2, 141);
            this.simpleClampUC1.Margin = new System.Windows.Forms.Padding(0);
            this.simpleClampUC1.Name = "simpleClampUC1";
            this.simpleClampUC1.Size = new System.Drawing.Size(131, 94);
            this.simpleClampUC1.TabIndex = 18;
            // 
            // simpleClampUC2
            // 
            this.simpleClampUC2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.simpleClampUC2.Location = new System.Drawing.Point(2, 24);
            this.simpleClampUC2.Margin = new System.Windows.Forms.Padding(0);
            this.simpleClampUC2.Name = "simpleClampUC2";
            this.simpleClampUC2.Size = new System.Drawing.Size(131, 93);
            this.simpleClampUC2.TabIndex = 15;
            // 
            // BlankerUC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ContextMenuStrip = this.cmsBlanker;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "BlankerUC";
            this.Size = new System.Drawing.Size(141, 268);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbTriLamp)).EndInit();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.cmsBlanker.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label lbName;
        private System.Windows.Forms.PictureBox pbTriLamp;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private SimpleClampUC simpleClampUC1;
        private System.Windows.Forms.Label lbStationName1;
        private System.Windows.Forms.Label lbStationName2;
        private SimpleClampUC simpleClampUC2;
        private System.Windows.Forms.ContextMenuStrip cmsBlanker;
        private System.Windows.Forms.ToolStripMenuItem tsmPutFinished2;
        private System.Windows.Forms.ToolStripMenuItem tsmPutFinished1;
    }
}
