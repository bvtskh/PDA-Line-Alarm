namespace Alarmlines
{
    partial class frmConfig
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmConfig));
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnSaveChanged = new System.Windows.Forms.Button();
            this.txtStop = new System.Windows.Forms.TextBox();
            this.lblRefreshCOMPorts = new System.Windows.Forms.Label();
            this.txtRun = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.btnTestConnect = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.cbbBaudrate = new System.Windows.Forms.ComboBox();
            this.btnTestSend = new System.Windows.Forms.Button();
            this.lblRefresh = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.cbbSerialPort = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnSaveChanged);
            this.groupBox2.Controls.Add(this.txtStop);
            this.groupBox2.Controls.Add(this.lblRefreshCOMPorts);
            this.groupBox2.Controls.Add(this.txtRun);
            this.groupBox2.Controls.Add(this.label15);
            this.groupBox2.Controls.Add(this.btnTestConnect);
            this.groupBox2.Controls.Add(this.btnStop);
            this.groupBox2.Controls.Add(this.cbbBaudrate);
            this.groupBox2.Controls.Add(this.btnTestSend);
            this.groupBox2.Controls.Add(this.lblRefresh);
            this.groupBox2.Controls.Add(this.label13);
            this.groupBox2.Controls.Add(this.cbbSerialPort);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(407, 303);
            this.groupBox2.TabIndex = 26;
            this.groupBox2.TabStop = false;
            // 
            // btnSaveChanged
            // 
            this.btnSaveChanged.BackColor = System.Drawing.Color.Green;
            this.btnSaveChanged.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSaveChanged.ForeColor = System.Drawing.Color.White;
            this.btnSaveChanged.Image = global::Alarmlines.Properties.Resources.Save;
            this.btnSaveChanged.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSaveChanged.Location = new System.Drawing.Point(275, 235);
            this.btnSaveChanged.Name = "btnSaveChanged";
            this.btnSaveChanged.Padding = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.btnSaveChanged.Size = new System.Drawing.Size(79, 30);
            this.btnSaveChanged.TabIndex = 23;
            this.btnSaveChanged.Text = "&Saves";
            this.btnSaveChanged.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnSaveChanged.UseVisualStyleBackColor = false;
            this.btnSaveChanged.Visible = false;
            this.btnSaveChanged.Click += new System.EventHandler(this.btnSaveChanged_Click);
            // 
            // txtStop
            // 
            this.txtStop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtStop.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtStop.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtStop.ForeColor = System.Drawing.Color.White;
            this.txtStop.Location = new System.Drawing.Point(101, 188);
            this.txtStop.Name = "txtStop";
            this.txtStop.Size = new System.Drawing.Size(209, 22);
            this.txtStop.TabIndex = 0;
            this.txtStop.Text = "#TOFF*";
            this.txtStop.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblRefreshCOMPorts
            // 
            this.lblRefreshCOMPorts.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblRefreshCOMPorts.Image = global::Alarmlines.Properties.Resources.refesh_16;
            this.lblRefreshCOMPorts.Location = new System.Drawing.Point(316, 70);
            this.lblRefreshCOMPorts.Name = "lblRefreshCOMPorts";
            this.lblRefreshCOMPorts.Size = new System.Drawing.Size(17, 21);
            this.lblRefreshCOMPorts.TabIndex = 27;
            this.lblRefreshCOMPorts.Click += new System.EventHandler(this.lblRefreshCOMPorts_Click);
            // 
            // txtRun
            // 
            this.txtRun.BackColor = System.Drawing.Color.Green;
            this.txtRun.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtRun.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtRun.ForeColor = System.Drawing.Color.White;
            this.txtRun.Location = new System.Drawing.Point(101, 147);
            this.txtRun.Name = "txtRun";
            this.txtRun.Size = new System.Drawing.Size(209, 22);
            this.txtRun.TabIndex = 0;
            this.txtRun.Text = "#TON*";
            this.txtRun.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(44, 191);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(32, 13);
            this.label15.TabIndex = 1;
            this.label15.Text = "Stop:";
            // 
            // btnTestConnect
            // 
            this.btnTestConnect.BackColor = System.Drawing.Color.Transparent;
            this.btnTestConnect.Enabled = false;
            this.btnTestConnect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTestConnect.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTestConnect.ForeColor = System.Drawing.Color.White;
            this.btnTestConnect.Location = new System.Drawing.Point(335, 68);
            this.btnTestConnect.Name = "btnTestConnect";
            this.btnTestConnect.Padding = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.btnTestConnect.Size = new System.Drawing.Size(19, 22);
            this.btnTestConnect.TabIndex = 23;
            this.btnTestConnect.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnTestConnect.UseVisualStyleBackColor = false;
            // 
            // btnStop
            // 
            this.btnStop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.btnStop.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnStop.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStop.ForeColor = System.Drawing.Color.White;
            this.btnStop.Image = global::Alarmlines.Properties.Resources.Forward_Arrow_16px;
            this.btnStop.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnStop.Location = new System.Drawing.Point(319, 188);
            this.btnStop.Name = "btnStop";
            this.btnStop.Padding = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.btnStop.Size = new System.Drawing.Size(31, 24);
            this.btnStop.TabIndex = 23;
            this.btnStop.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnStop.UseVisualStyleBackColor = false;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // cbbBaudrate
            // 
            this.cbbBaudrate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbbBaudrate.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbbBaudrate.FormattingEnabled = true;
            this.cbbBaudrate.Location = new System.Drawing.Point(101, 107);
            this.cbbBaudrate.Name = "cbbBaudrate";
            this.cbbBaudrate.Size = new System.Drawing.Size(209, 21);
            this.cbbBaudrate.TabIndex = 31;
            // 
            // btnTestSend
            // 
            this.btnTestSend.BackColor = System.Drawing.Color.Green;
            this.btnTestSend.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnTestSend.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTestSend.ForeColor = System.Drawing.Color.White;
            this.btnTestSend.Image = global::Alarmlines.Properties.Resources.Forward_Arrow_16px;
            this.btnTestSend.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnTestSend.Location = new System.Drawing.Point(319, 146);
            this.btnTestSend.Name = "btnTestSend";
            this.btnTestSend.Padding = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.btnTestSend.Size = new System.Drawing.Size(31, 25);
            this.btnTestSend.TabIndex = 23;
            this.btnTestSend.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnTestSend.UseVisualStyleBackColor = false;
            this.btnTestSend.Click += new System.EventHandler(this.btnTestSend_Click);
            // 
            // lblRefresh
            // 
            this.lblRefresh.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblRefresh.Location = new System.Drawing.Point(330, 68);
            this.lblRefresh.Name = "lblRefresh";
            this.lblRefresh.Size = new System.Drawing.Size(17, 21);
            this.lblRefresh.TabIndex = 27;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(44, 152);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(30, 13);
            this.label13.TabIndex = 1;
            this.label13.Text = "Run:";
            // 
            // cbbSerialPort
            // 
            this.cbbSerialPort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbbSerialPort.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbbSerialPort.FormattingEnabled = true;
            this.cbbSerialPort.Location = new System.Drawing.Point(102, 68);
            this.cbbSerialPort.Name = "cbbSerialPort";
            this.cbbSerialPort.Size = new System.Drawing.Size(209, 21);
            this.cbbSerialPort.TabIndex = 22;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(44, 113);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 13);
            this.label1.TabIndex = 21;
            this.label1.Text = "Baudrate:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(44, 74);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(57, 13);
            this.label4.TabIndex = 21;
            this.label4.Text = "Port Name";
            // 
            // frmConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(407, 303);
            this.Controls.Add(this.groupBox2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmConfig";
            this.Load += new System.EventHandler(this.FormConfig_Load);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }


        #endregion

        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnSaveChanged;
        private System.Windows.Forms.TextBox txtStop;
        private System.Windows.Forms.Label lblRefreshCOMPorts;
        private System.Windows.Forms.TextBox txtRun;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Button btnTestConnect;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.ComboBox cbbBaudrate;
        private System.Windows.Forms.Button btnTestSend;
        private System.Windows.Forms.Label lblRefresh;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.ComboBox cbbSerialPort;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
    }
}