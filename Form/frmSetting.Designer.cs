namespace Alarmlines
{
    partial class Setting
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
            this.btnCancel = new System.Windows.Forms.Button();
            this.cmbBaudrate = new System.Windows.Forms.ComboBox();
            this.cmbComport = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cmbDataBits = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbParity = new System.Windows.Forms.ComboBox();
            this.cmbStopBits = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label11 = new System.Windows.Forms.Label();
            this.cmbQueryBeforeHours = new System.Windows.Forms.ComboBox();
            this.CheckboxAdvance = new System.Windows.Forms.CheckBox();
            this.lbState = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.txbConfirmTime = new System.Windows.Forms.TextBox();
            this.txbQueryTime = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.Normal = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.Normal.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(282, 156);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(86, 40);
            this.btnCancel.TabIndex = 26;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // cmbBaudrate
            // 
            this.cmbBaudrate.Enabled = false;
            this.cmbBaudrate.FormattingEnabled = true;
            this.cmbBaudrate.Items.AddRange(new object[] {
            "9600",
            "19200",
            "38400",
            "56000",
            "115200"});
            this.cmbBaudrate.Location = new System.Drawing.Point(61, 86);
            this.cmbBaudrate.Name = "cmbBaudrate";
            this.cmbBaudrate.Size = new System.Drawing.Size(99, 21);
            this.cmbBaudrate.TabIndex = 3;
            this.cmbBaudrate.Visible = false;
            // 
            // cmbComport
            // 
            this.cmbComport.FormattingEnabled = true;
            this.cmbComport.Location = new System.Drawing.Point(61, 33);
            this.cmbComport.Name = "cmbComport";
            this.cmbComport.Size = new System.Drawing.Size(99, 21);
            this.cmbComport.TabIndex = 3;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 89);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(50, 13);
            this.label5.TabIndex = 3;
            this.label5.Text = "Baudrate";
            // 
            // cmbDataBits
            // 
            this.cmbDataBits.Enabled = false;
            this.cmbDataBits.FormattingEnabled = true;
            this.cmbDataBits.Items.AddRange(new object[] {
            "8",
            "7"});
            this.cmbDataBits.Location = new System.Drawing.Point(71, 3);
            this.cmbDataBits.Name = "cmbDataBits";
            this.cmbDataBits.Size = new System.Drawing.Size(109, 21);
            this.cmbDataBits.TabIndex = 6;
            this.cmbDataBits.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "DataBits";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmbParity
            // 
            this.cmbParity.Enabled = false;
            this.cmbParity.FormattingEnabled = true;
            this.cmbParity.Location = new System.Drawing.Point(71, 35);
            this.cmbParity.Name = "cmbParity";
            this.cmbParity.Size = new System.Drawing.Size(109, 21);
            this.cmbParity.TabIndex = 7;
            this.cmbParity.Visible = false;
            // 
            // cmbStopBits
            // 
            this.cmbStopBits.FormattingEnabled = true;
            this.cmbStopBits.Location = new System.Drawing.Point(71, 64);
            this.cmbStopBits.Name = "cmbStopBits";
            this.cmbStopBits.Size = new System.Drawing.Size(109, 21);
            this.cmbStopBits.TabIndex = 8;
            this.cmbStopBits.Visible = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 36);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(50, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "Com Port";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.cmbQueryBeforeHours);
            this.groupBox1.Controls.Add(this.CheckboxAdvance);
            this.groupBox1.Controls.Add(this.lbState);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.txbConfirmTime);
            this.groupBox1.Controls.Add(this.txbQueryTime);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Location = new System.Drawing.Point(6, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(365, 138);
            this.groupBox1.TabIndex = 25;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Param System";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(163, 19);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(94, 13);
            this.label11.TabIndex = 12;
            this.label11.Text = "QueryBeforeHours";
            // 
            // cmbQueryBeforeHours
            // 
            this.cmbQueryBeforeHours.FormattingEnabled = true;
            this.cmbQueryBeforeHours.Items.AddRange(new object[] {
            "2",
            "4",
            "8",
            "12",
            "24",
            "36",
            "48",
            "60",
            "72"});
            this.cmbQueryBeforeHours.Location = new System.Drawing.Point(166, 37);
            this.cmbQueryBeforeHours.Name = "cmbQueryBeforeHours";
            this.cmbQueryBeforeHours.Size = new System.Drawing.Size(83, 21);
            this.cmbQueryBeforeHours.TabIndex = 11;
            this.cmbQueryBeforeHours.Visible = false;
            // 
            // CheckboxAdvance
            // 
            this.CheckboxAdvance.AutoSize = true;
            this.CheckboxAdvance.Location = new System.Drawing.Point(166, 82);
            this.CheckboxAdvance.Name = "CheckboxAdvance";
            this.CheckboxAdvance.Size = new System.Drawing.Size(69, 17);
            this.CheckboxAdvance.TabIndex = 24;
            this.CheckboxAdvance.Text = "Advance";
            this.CheckboxAdvance.UseVisualStyleBackColor = true;
            this.CheckboxAdvance.CheckedChanged += new System.EventHandler(this.CheckboxAdvance_CheckedChanged);
            // 
            // lbState
            // 
            this.lbState.AutoSize = true;
            this.lbState.Location = new System.Drawing.Point(16, 113);
            this.lbState.Name = "lbState";
            this.lbState.Size = new System.Drawing.Size(32, 13);
            this.lbState.TabIndex = 8;
            this.lbState.Text = "State";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(125, 83);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(12, 13);
            this.label9.TabIndex = 7;
            this.label9.Text = "s";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(125, 41);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(12, 13);
            this.label8.TabIndex = 6;
            this.label8.Text = "s";
            // 
            // txbConfirmTime
            // 
            this.txbConfirmTime.Location = new System.Drawing.Point(19, 80);
            this.txbConfirmTime.Name = "txbConfirmTime";
            this.txbConfirmTime.ReadOnly = true;
            this.txbConfirmTime.Size = new System.Drawing.Size(100, 20);
            this.txbConfirmTime.TabIndex = 5;
            // 
            // txbQueryTime
            // 
            this.txbQueryTime.Location = new System.Drawing.Point(19, 38);
            this.txbQueryTime.Name = "txbQueryTime";
            this.txbQueryTime.ReadOnly = true;
            this.txbQueryTime.Size = new System.Drawing.Size(100, 20);
            this.txbQueryTime.TabIndex = 4;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(16, 61);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(68, 13);
            this.label6.TabIndex = 3;
            this.label6.Text = "Confirm Time";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(16, 19);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(61, 13);
            this.label7.TabIndex = 2;
            this.label7.Text = "Query Time";
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(188, 156);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(86, 40);
            this.btnSave.TabIndex = 22;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 32);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(33, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Parity";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 61);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(46, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "StopBits";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 37.69231F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 62.30769F));
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.cmbDataBits, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.cmbParity, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.cmbStopBits, 1, 2);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(176, 19);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 51.72414F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 48.27586F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 34F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(183, 96);
            this.tableLayoutPanel1.TabIndex = 23;
            // 
            // Normal
            // 
            this.Normal.Controls.Add(this.cmbBaudrate);
            this.Normal.Controls.Add(this.cmbComport);
            this.Normal.Controls.Add(this.label5);
            this.Normal.Controls.Add(this.tableLayoutPanel1);
            this.Normal.Controls.Add(this.label4);
            this.Normal.Location = new System.Drawing.Point(12, 237);
            this.Normal.Name = "Normal";
            this.Normal.Size = new System.Drawing.Size(365, 129);
            this.Normal.TabIndex = 21;
            this.Normal.TabStop = false;
            this.Normal.Text = "Normal";
            // 
            // Setting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(373, 197);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.Normal);
            this.Name = "Setting";
            this.Text = "Setting";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Setting_FormClosed);
            this.Load += new System.EventHandler(this.Setting_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.Normal.ResumeLayout(false);
            this.Normal.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ComboBox cmbBaudrate;
        private System.Windows.Forms.ComboBox cmbComport;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cmbDataBits;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbParity;
        private System.Windows.Forms.ComboBox cmbStopBits;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lbState;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txbConfirmTime;
        private System.Windows.Forms.TextBox txbQueryTime;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox CheckboxAdvance;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.GroupBox Normal;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ComboBox cmbQueryBeforeHours;
    }
}