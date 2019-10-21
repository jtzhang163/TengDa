namespace CAMEL.Baking.Control
{
    partial class ClampManageUC
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
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.lbClampCode = new System.Windows.Forms.Label();
            this.tbClampCodeNew = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cbOvenStations = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cbOvens = new System.Windows.Forms.ComboBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox3
            // 
            this.groupBox3.BackColor = System.Drawing.SystemColors.Control;
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.lbClampCode);
            this.groupBox3.Controls.Add(this.tbClampCodeNew);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Controls.Add(this.cbOvenStations);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.cbOvens);
            this.groupBox3.Controls.Add(this.btnOK);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Font = new System.Drawing.Font("黑体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox3.Location = new System.Drawing.Point(0, 0);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(30);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(30);
            this.groupBox3.Size = new System.Drawing.Size(352, 231);
            this.groupBox3.TabIndex = 5;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "工位夹具管理";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(55, 148);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(105, 14);
            this.label4.TabIndex = 11;
            this.label4.Text = "修改夹具条码：";
            // 
            // lbClampCode
            // 
            this.lbClampCode.AutoSize = true;
            this.lbClampCode.Font = new System.Drawing.Font("黑体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbClampCode.ForeColor = System.Drawing.Color.LimeGreen;
            this.lbClampCode.Location = new System.Drawing.Point(163, 118);
            this.lbClampCode.Name = "lbClampCode";
            this.lbClampCode.Size = new System.Drawing.Size(71, 14);
            this.lbClampCode.TabIndex = 10;
            this.lbClampCode.Text = "LXY00001";
            // 
            // tbClampCodeNew
            // 
            this.tbClampCodeNew.Font = new System.Drawing.Font("黑体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbClampCodeNew.ForeColor = System.Drawing.Color.Green;
            this.tbClampCodeNew.Location = new System.Drawing.Point(163, 145);
            this.tbClampCodeNew.Name = "tbClampCodeNew";
            this.tbClampCodeNew.Size = new System.Drawing.Size(100, 23);
            this.tbClampCodeNew.TabIndex = 9;
            this.tbClampCodeNew.Text = "LXY00001";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(55, 118);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(105, 14);
            this.label1.TabIndex = 8;
            this.label1.Text = "当前夹具条码：";
            // 
            // cbOvenStations
            // 
            this.cbOvenStations.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbOvenStations.FormattingEnabled = true;
            this.cbOvenStations.Location = new System.Drawing.Point(163, 73);
            this.cbOvenStations.Name = "cbOvenStations";
            this.cbOvenStations.Size = new System.Drawing.Size(100, 22);
            this.cbOvenStations.TabIndex = 7;
            this.cbOvenStations.SelectedIndexChanged += new System.EventHandler(this.CbOvenStations_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(55, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 14);
            this.label2.TabIndex = 5;
            this.label2.Text = "选择工位：";
            // 
            // cbOvens
            // 
            this.cbOvens.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbOvens.FormattingEnabled = true;
            this.cbOvens.Location = new System.Drawing.Point(55, 73);
            this.cbOvens.Name = "cbOvens";
            this.cbOvens.Size = new System.Drawing.Size(98, 22);
            this.cbOvens.TabIndex = 3;
            this.cbOvens.SelectedIndexChanged += new System.EventHandler(this.CbOvens_SelectedIndexChanged);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(163, 186);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "确定";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.BtnOK_Click);
            // 
            // ClampManageUC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox3);
            this.Font = new System.Drawing.Font("黑体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "ClampManageUC";
            this.Size = new System.Drawing.Size(352, 231);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbOvens;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.TextBox tbClampCodeNew;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbOvenStations;
        private System.Windows.Forms.Label lbClampCode;
        private System.Windows.Forms.Label label4;
    }
}
