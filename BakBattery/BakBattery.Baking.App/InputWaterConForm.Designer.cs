namespace BakBattery.Baking.App
{
    partial class InputWaterConForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InputWaterConForm));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lbFromStation = new System.Windows.Forms.Label();
            this.lbTrayCode = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.tbResult = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.lbTip = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35F));
            this.tableLayoutPanel1.Controls.Add(this.lbFromStation, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.lbTrayCode, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.btnOK, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.tbResult, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.label4, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.lbTip, 0, 4);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(415, 231);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // lbFromStation
            // 
            this.lbFromStation.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lbFromStation.AutoSize = true;
            this.lbFromStation.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbFromStation.ForeColor = System.Drawing.Color.Green;
            this.lbFromStation.Location = new System.Drawing.Point(148, 68);
            this.lbFromStation.Name = "lbFromStation";
            this.lbFromStation.Size = new System.Drawing.Size(65, 19);
            this.lbFromStation.TabIndex = 6;
            this.lbFromStation.Text = "来源炉腔";
            // 
            // lbTrayCode
            // 
            this.lbTrayCode.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lbTrayCode.AutoSize = true;
            this.lbTrayCode.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbTrayCode.ForeColor = System.Drawing.Color.Green;
            this.lbTrayCode.Location = new System.Drawing.Point(148, 16);
            this.lbTrayCode.Name = "lbTrayCode";
            this.lbTrayCode.Size = new System.Drawing.Size(65, 19);
            this.lbTrayCode.TabIndex = 5;
            this.lbTrayCode.Text = "托盘条码";
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(63, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 19);
            this.label1.TabIndex = 0;
            this.label1.Text = "托盘条码：";
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(63, 68);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(79, 19);
            this.label2.TabIndex = 1;
            this.label2.Text = "来源炉腔：";
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(63, 120);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(79, 19);
            this.label3.TabIndex = 2;
            this.label3.Text = "水分结果：";
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnOK.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnOK.Location = new System.Drawing.Point(148, 177);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 28);
            this.btnOK.TabIndex = 3;
            this.btnOK.Text = "确定(&Y)";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.BtnOK_Click);
            // 
            // tbResult
            // 
            this.tbResult.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tbResult.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbResult.ForeColor = System.Drawing.Color.Green;
            this.tbResult.Location = new System.Drawing.Point(148, 115);
            this.tbResult.Name = "tbResult";
            this.tbResult.Size = new System.Drawing.Size(118, 29);
            this.tbResult.TabIndex = 7;
            this.tbResult.Text = "123.45";
            this.tbResult.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TbResult_KeyPress);
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(272, 120);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 19);
            this.label4.TabIndex = 8;
            this.label4.Text = "PPM";
            // 
            // lbTip
            // 
            this.lbTip.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lbTip.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.lbTip, 3);
            this.lbTip.ForeColor = System.Drawing.Color.Green;
            this.lbTip.Location = new System.Drawing.Point(193, 213);
            this.lbTip.Name = "lbTip";
            this.lbTip.Size = new System.Drawing.Size(29, 12);
            this.lbTip.TabIndex = 4;
            this.lbTip.Text = "结果";
            // 
            // InputWaterConForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(415, 231);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "InputWaterConForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "水含量结果手动输入";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label lbTip;
        private System.Windows.Forms.Label lbTrayCode;
        private System.Windows.Forms.Label lbFromStation;
        private System.Windows.Forms.TextBox tbResult;
        private System.Windows.Forms.Label label4;
    }
}