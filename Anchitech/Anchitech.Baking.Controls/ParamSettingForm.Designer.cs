namespace Anchitech.Baking.Controls
{
    partial class ParamSettingForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ParamSettingForm));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.btnSetParam = new System.Windows.Forms.Button();
            this.tlpParamList = new System.Windows.Forms.TableLayoutPanel();
            this.btnGetParam = new System.Windows.Forms.Button();
            this.btnSetDefaultValue = new System.Windows.Forms.Button();
            this.lbGetStatus = new System.Windows.Forms.Label();
            this.lbSetStatus = new System.Windows.Forms.Label();
            this.btnGetDefaultValue = new System.Windows.Forms.Button();
            this.ovenParamUC009 = new Anchitech.Baking.Controls.OvenParamUC();
            this.ovenParamUC008 = new Anchitech.Baking.Controls.OvenParamUC();
            this.ovenParamUC007 = new Anchitech.Baking.Controls.OvenParamUC();
            this.ovenParamUC006 = new Anchitech.Baking.Controls.OvenParamUC();
            this.ovenParamUC005 = new Anchitech.Baking.Controls.OvenParamUC();
            this.ovenParamUC004 = new Anchitech.Baking.Controls.OvenParamUC();
            this.ovenParamUC003 = new Anchitech.Baking.Controls.OvenParamUC();
            this.ovenParamUC002 = new Anchitech.Baking.Controls.OvenParamUC();
            this.ovenParamUC001 = new Anchitech.Baking.Controls.OvenParamUC();
            this.ovenParamUC011 = new Anchitech.Baking.Controls.OvenParamUC();
            this.ovenParamUC013 = new Anchitech.Baking.Controls.OvenParamUC();
            this.ovenParamUC014 = new Anchitech.Baking.Controls.OvenParamUC();
            this.ovenParamUC015 = new Anchitech.Baking.Controls.OvenParamUC();
            this.ovenParamUC016 = new Anchitech.Baking.Controls.OvenParamUC();
            this.ovenParamUC017 = new Anchitech.Baking.Controls.OvenParamUC();
            this.ovenParamUC018 = new Anchitech.Baking.Controls.OvenParamUC();
            this.ovenParamUC019 = new Anchitech.Baking.Controls.OvenParamUC();
            this.ovenParamUC010 = new Anchitech.Baking.Controls.OvenParamUC();
            this.ovenParamUC020 = new Anchitech.Baking.Controls.OvenParamUC();
            this.ovenParamUC012 = new Anchitech.Baking.Controls.OvenParamUC();
            this.ovenParamUC021 = new Anchitech.Baking.Controls.OvenParamUC();
            this.ovenParamUC022 = new Anchitech.Baking.Controls.OvenParamUC();
            this.ovenParamUC023 = new Anchitech.Baking.Controls.OvenParamUC();
            this.tableLayoutPanel1.SuspendLayout();
            this.tlpParamList.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.Controls.Add(this.btnSetParam, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.tlpParamList, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnGetParam, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnSetDefaultValue, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.lbGetStatus, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.lbSetStatus, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.btnGetDefaultValue, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(760, 495);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // btnSetParam
            // 
            this.btnSetParam.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnSetParam.Location = new System.Drawing.Point(101, 458);
            this.btnSetParam.Name = "btnSetParam";
            this.btnSetParam.Size = new System.Drawing.Size(86, 23);
            this.btnSetParam.TabIndex = 6;
            this.btnSetParam.Text = "更新到设备";
            this.btnSetParam.UseVisualStyleBackColor = true;
            this.btnSetParam.Click += new System.EventHandler(this.BtnSetParam_Click);
            // 
            // tlpParamList
            // 
            this.tlpParamList.ColumnCount = 2;
            this.tableLayoutPanel1.SetColumnSpan(this.tlpParamList, 4);
            this.tlpParamList.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpParamList.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpParamList.Controls.Add(this.ovenParamUC009, 0, 8);
            this.tlpParamList.Controls.Add(this.ovenParamUC008, 0, 7);
            this.tlpParamList.Controls.Add(this.ovenParamUC007, 0, 6);
            this.tlpParamList.Controls.Add(this.ovenParamUC006, 0, 5);
            this.tlpParamList.Controls.Add(this.ovenParamUC005, 0, 4);
            this.tlpParamList.Controls.Add(this.ovenParamUC004, 0, 3);
            this.tlpParamList.Controls.Add(this.ovenParamUC003, 0, 2);
            this.tlpParamList.Controls.Add(this.ovenParamUC002, 0, 1);
            this.tlpParamList.Controls.Add(this.ovenParamUC001, 0, 0);
            this.tlpParamList.Controls.Add(this.ovenParamUC011, 0, 10);
            this.tlpParamList.Controls.Add(this.ovenParamUC013, 1, 0);
            this.tlpParamList.Controls.Add(this.ovenParamUC014, 1, 1);
            this.tlpParamList.Controls.Add(this.ovenParamUC015, 1, 2);
            this.tlpParamList.Controls.Add(this.ovenParamUC016, 1, 3);
            this.tlpParamList.Controls.Add(this.ovenParamUC017, 1, 4);
            this.tlpParamList.Controls.Add(this.ovenParamUC018, 1, 5);
            this.tlpParamList.Controls.Add(this.ovenParamUC019, 1, 6);
            this.tlpParamList.Controls.Add(this.ovenParamUC010, 0, 9);
            this.tlpParamList.Controls.Add(this.ovenParamUC020, 1, 7);
            this.tlpParamList.Controls.Add(this.ovenParamUC012, 0, 11);
            this.tlpParamList.Controls.Add(this.ovenParamUC021, 1, 8);
            this.tlpParamList.Controls.Add(this.ovenParamUC022, 1, 9);
            this.tlpParamList.Controls.Add(this.ovenParamUC023, 1, 10);
            this.tlpParamList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpParamList.Location = new System.Drawing.Point(3, 53);
            this.tlpParamList.Name = "tlpParamList";
            this.tlpParamList.RowCount = 12;
            this.tlpParamList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.333417F));
            this.tlpParamList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.333417F));
            this.tlpParamList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.333417F));
            this.tlpParamList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.333417F));
            this.tlpParamList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.333417F));
            this.tlpParamList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.333417F));
            this.tlpParamList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.333417F));
            this.tlpParamList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.333417F));
            this.tlpParamList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.333417F));
            this.tlpParamList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.332584F));
            this.tlpParamList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.333333F));
            this.tlpParamList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.333333F));
            this.tlpParamList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpParamList.Size = new System.Drawing.Size(754, 389);
            this.tlpParamList.TabIndex = 0;
            // 
            // btnGetParam
            // 
            this.btnGetParam.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnGetParam.Location = new System.Drawing.Point(101, 13);
            this.btnGetParam.Name = "btnGetParam";
            this.btnGetParam.Size = new System.Drawing.Size(86, 23);
            this.btnGetParam.TabIndex = 1;
            this.btnGetParam.Text = "获取设备值";
            this.btnGetParam.UseVisualStyleBackColor = true;
            this.btnGetParam.Click += new System.EventHandler(this.BtnGetParam_Click);
            // 
            // btnSetDefaultValue
            // 
            this.btnSetDefaultValue.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnSetDefaultValue.Location = new System.Drawing.Point(291, 458);
            this.btnSetDefaultValue.Name = "btnSetDefaultValue";
            this.btnSetDefaultValue.Size = new System.Drawing.Size(86, 23);
            this.btnSetDefaultValue.TabIndex = 2;
            this.btnSetDefaultValue.Text = "设置为默认值";
            this.btnSetDefaultValue.UseVisualStyleBackColor = true;
            this.btnSetDefaultValue.Click += new System.EventHandler(this.BtnSetDefaultValue_Click);
            // 
            // lbGetStatus
            // 
            this.lbGetStatus.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lbGetStatus.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.lbGetStatus, 2);
            this.lbGetStatus.ForeColor = System.Drawing.Color.LimeGreen;
            this.lbGetStatus.Location = new System.Drawing.Point(383, 19);
            this.lbGetStatus.Name = "lbGetStatus";
            this.lbGetStatus.Size = new System.Drawing.Size(89, 12);
            this.lbGetStatus.TabIndex = 3;
            this.lbGetStatus.Text = "获取设备值状态";
            // 
            // lbSetStatus
            // 
            this.lbSetStatus.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lbSetStatus.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.lbSetStatus, 2);
            this.lbSetStatus.ForeColor = System.Drawing.Color.LimeGreen;
            this.lbSetStatus.Location = new System.Drawing.Point(383, 464);
            this.lbSetStatus.Name = "lbSetStatus";
            this.lbSetStatus.Size = new System.Drawing.Size(89, 12);
            this.lbSetStatus.TabIndex = 4;
            this.lbSetStatus.Text = "设置设备值状态";
            // 
            // btnGetDefaultValue
            // 
            this.btnGetDefaultValue.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnGetDefaultValue.Location = new System.Drawing.Point(291, 13);
            this.btnGetDefaultValue.Name = "btnGetDefaultValue";
            this.btnGetDefaultValue.Size = new System.Drawing.Size(86, 23);
            this.btnGetDefaultValue.TabIndex = 5;
            this.btnGetDefaultValue.Text = "加载默认值";
            this.btnGetDefaultValue.UseVisualStyleBackColor = true;
            this.btnGetDefaultValue.Click += new System.EventHandler(this.BtnGetDefaultValue_Click);
            // 
            // ovenParamUC009
            // 
            this.ovenParamUC009.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ovenParamUC009.Location = new System.Drawing.Point(3, 259);
            this.ovenParamUC009.Name = "ovenParamUC009";
            this.ovenParamUC009.Size = new System.Drawing.Size(371, 26);
            this.ovenParamUC009.TabIndex = 16;
            // 
            // ovenParamUC008
            // 
            this.ovenParamUC008.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ovenParamUC008.Location = new System.Drawing.Point(3, 227);
            this.ovenParamUC008.Name = "ovenParamUC008";
            this.ovenParamUC008.Size = new System.Drawing.Size(371, 26);
            this.ovenParamUC008.TabIndex = 14;
            // 
            // ovenParamUC007
            // 
            this.ovenParamUC007.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ovenParamUC007.Location = new System.Drawing.Point(3, 195);
            this.ovenParamUC007.Name = "ovenParamUC007";
            this.ovenParamUC007.Size = new System.Drawing.Size(371, 26);
            this.ovenParamUC007.TabIndex = 12;
            // 
            // ovenParamUC006
            // 
            this.ovenParamUC006.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ovenParamUC006.Location = new System.Drawing.Point(3, 163);
            this.ovenParamUC006.Name = "ovenParamUC006";
            this.ovenParamUC006.Size = new System.Drawing.Size(371, 26);
            this.ovenParamUC006.TabIndex = 10;
            // 
            // ovenParamUC005
            // 
            this.ovenParamUC005.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ovenParamUC005.Location = new System.Drawing.Point(3, 131);
            this.ovenParamUC005.Name = "ovenParamUC005";
            this.ovenParamUC005.Size = new System.Drawing.Size(371, 26);
            this.ovenParamUC005.TabIndex = 8;
            // 
            // ovenParamUC004
            // 
            this.ovenParamUC004.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ovenParamUC004.Location = new System.Drawing.Point(3, 99);
            this.ovenParamUC004.Name = "ovenParamUC004";
            this.ovenParamUC004.Size = new System.Drawing.Size(371, 26);
            this.ovenParamUC004.TabIndex = 6;
            // 
            // ovenParamUC003
            // 
            this.ovenParamUC003.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ovenParamUC003.Location = new System.Drawing.Point(3, 67);
            this.ovenParamUC003.Name = "ovenParamUC003";
            this.ovenParamUC003.Size = new System.Drawing.Size(371, 26);
            this.ovenParamUC003.TabIndex = 4;
            // 
            // ovenParamUC002
            // 
            this.ovenParamUC002.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ovenParamUC002.Location = new System.Drawing.Point(3, 35);
            this.ovenParamUC002.Name = "ovenParamUC002";
            this.ovenParamUC002.Size = new System.Drawing.Size(371, 26);
            this.ovenParamUC002.TabIndex = 2;
            // 
            // ovenParamUC001
            // 
            this.ovenParamUC001.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ovenParamUC001.Location = new System.Drawing.Point(3, 3);
            this.ovenParamUC001.Name = "ovenParamUC001";
            this.ovenParamUC001.Size = new System.Drawing.Size(371, 26);
            this.ovenParamUC001.TabIndex = 0;
            // 
            // ovenParamUC011
            // 
            this.ovenParamUC011.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ovenParamUC011.Location = new System.Drawing.Point(3, 323);
            this.ovenParamUC011.Name = "ovenParamUC011";
            this.ovenParamUC011.Size = new System.Drawing.Size(371, 26);
            this.ovenParamUC011.TabIndex = 1;
            // 
            // ovenParamUC013
            // 
            this.ovenParamUC013.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ovenParamUC013.Location = new System.Drawing.Point(380, 3);
            this.ovenParamUC013.Name = "ovenParamUC013";
            this.ovenParamUC013.Size = new System.Drawing.Size(371, 26);
            this.ovenParamUC013.TabIndex = 5;
            // 
            // ovenParamUC014
            // 
            this.ovenParamUC014.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ovenParamUC014.Location = new System.Drawing.Point(380, 35);
            this.ovenParamUC014.Name = "ovenParamUC014";
            this.ovenParamUC014.Size = new System.Drawing.Size(371, 26);
            this.ovenParamUC014.TabIndex = 7;
            // 
            // ovenParamUC015
            // 
            this.ovenParamUC015.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ovenParamUC015.Location = new System.Drawing.Point(380, 67);
            this.ovenParamUC015.Name = "ovenParamUC015";
            this.ovenParamUC015.Size = new System.Drawing.Size(371, 26);
            this.ovenParamUC015.TabIndex = 9;
            // 
            // ovenParamUC016
            // 
            this.ovenParamUC016.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ovenParamUC016.Location = new System.Drawing.Point(380, 99);
            this.ovenParamUC016.Name = "ovenParamUC016";
            this.ovenParamUC016.Size = new System.Drawing.Size(371, 26);
            this.ovenParamUC016.TabIndex = 11;
            // 
            // ovenParamUC017
            // 
            this.ovenParamUC017.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ovenParamUC017.Location = new System.Drawing.Point(380, 131);
            this.ovenParamUC017.Name = "ovenParamUC017";
            this.ovenParamUC017.Size = new System.Drawing.Size(371, 26);
            this.ovenParamUC017.TabIndex = 13;
            // 
            // ovenParamUC018
            // 
            this.ovenParamUC018.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ovenParamUC018.Location = new System.Drawing.Point(380, 163);
            this.ovenParamUC018.Name = "ovenParamUC018";
            this.ovenParamUC018.Size = new System.Drawing.Size(371, 26);
            this.ovenParamUC018.TabIndex = 15;
            // 
            // ovenParamUC019
            // 
            this.ovenParamUC019.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ovenParamUC019.Location = new System.Drawing.Point(380, 195);
            this.ovenParamUC019.Name = "ovenParamUC019";
            this.ovenParamUC019.Size = new System.Drawing.Size(371, 26);
            this.ovenParamUC019.TabIndex = 17;
            // 
            // ovenParamUC010
            // 
            this.ovenParamUC010.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ovenParamUC010.Location = new System.Drawing.Point(3, 291);
            this.ovenParamUC010.Name = "ovenParamUC010";
            this.ovenParamUC010.Size = new System.Drawing.Size(371, 26);
            this.ovenParamUC010.TabIndex = 19;
            // 
            // ovenParamUC020
            // 
            this.ovenParamUC020.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ovenParamUC020.Location = new System.Drawing.Point(380, 227);
            this.ovenParamUC020.Name = "ovenParamUC020";
            this.ovenParamUC020.Size = new System.Drawing.Size(371, 26);
            this.ovenParamUC020.TabIndex = 18;
            // 
            // ovenParamUC012
            // 
            this.ovenParamUC012.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ovenParamUC012.Location = new System.Drawing.Point(3, 355);
            this.ovenParamUC012.Name = "ovenParamUC012";
            this.ovenParamUC012.Size = new System.Drawing.Size(371, 31);
            this.ovenParamUC012.TabIndex = 3;
            // 
            // ovenParamUC021
            // 
            this.ovenParamUC021.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ovenParamUC021.Location = new System.Drawing.Point(380, 259);
            this.ovenParamUC021.Name = "ovenParamUC021";
            this.ovenParamUC021.Size = new System.Drawing.Size(371, 26);
            this.ovenParamUC021.TabIndex = 20;
            // 
            // ovenParamUC022
            // 
            this.ovenParamUC022.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ovenParamUC022.Location = new System.Drawing.Point(380, 291);
            this.ovenParamUC022.Name = "ovenParamUC022";
            this.ovenParamUC022.Size = new System.Drawing.Size(371, 26);
            this.ovenParamUC022.TabIndex = 21;
            // 
            // ovenParamUC023
            // 
            this.ovenParamUC023.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ovenParamUC023.Location = new System.Drawing.Point(380, 323);
            this.ovenParamUC023.Name = "ovenParamUC023";
            this.ovenParamUC023.Size = new System.Drawing.Size(371, 26);
            this.ovenParamUC023.TabIndex = 22;
            // 
            // ParamSettingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(760, 495);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ParamSettingForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "烤箱参数设置";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tlpParamList.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tlpParamList;
        private OvenParamUC ovenParamUC001;
        private OvenParamUC ovenParamUC019;
        private OvenParamUC ovenParamUC009;
        private OvenParamUC ovenParamUC018;
        private OvenParamUC ovenParamUC008;
        private OvenParamUC ovenParamUC017;
        private OvenParamUC ovenParamUC007;
        private OvenParamUC ovenParamUC016;
        private OvenParamUC ovenParamUC006;
        private OvenParamUC ovenParamUC015;
        private OvenParamUC ovenParamUC005;
        private OvenParamUC ovenParamUC014;
        private OvenParamUC ovenParamUC004;
        private OvenParamUC ovenParamUC013;
        private OvenParamUC ovenParamUC003;
        private OvenParamUC ovenParamUC012;
        private OvenParamUC ovenParamUC002;
        private OvenParamUC ovenParamUC011;
        private OvenParamUC ovenParamUC010;
        private OvenParamUC ovenParamUC020;
        private System.Windows.Forms.Button btnGetParam;
        private System.Windows.Forms.Button btnSetDefaultValue;
        private System.Windows.Forms.Label lbGetStatus;
        private System.Windows.Forms.Label lbSetStatus;
        private System.Windows.Forms.Button btnGetDefaultValue;
        private System.Windows.Forms.Button btnSetParam;
        private OvenParamUC ovenParamUC023;
        private OvenParamUC ovenParamUC021;
        private OvenParamUC ovenParamUC022;
    }
}