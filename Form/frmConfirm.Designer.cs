namespace Alarmlines
{
    partial class Confirm
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnCancelConfirm = new System.Windows.Forms.Button();
            this.btnRealConfirm = new System.Windows.Forms.Button();
            this.grCustomerInfo = new System.Windows.Forms.GroupBox();
            this.lbCustomerInfoConfirm = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.lbLineInfoConfirm = new System.Windows.Forms.Label();
            this.grDevice = new System.Windows.Forms.GroupBox();
            this.lbInfoErrConfirm = new System.Windows.Forms.Label();
            this.lbContentConfirm = new System.Windows.Forms.Label();
            this.txbContentConfirm = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.grCustomerInfo.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.grDevice.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupBox1.Controls.Add(this.btnCancelConfirm);
            this.groupBox1.Controls.Add(this.btnRealConfirm);
            this.groupBox1.Controls.Add(this.grCustomerInfo);
            this.groupBox1.Controls.Add(this.groupBox3);
            this.groupBox1.Controls.Add(this.grDevice);
            this.groupBox1.Controls.Add(this.lbContentConfirm);
            this.groupBox1.Controls.Add(this.txbContentConfirm);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(520, 237);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // btnCancelConfirm
            // 
            this.btnCancelConfirm.BackColor = System.Drawing.SystemColors.Control;
            this.btnCancelConfirm.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancelConfirm.Location = new System.Drawing.Point(403, 186);
            this.btnCancelConfirm.Name = "btnCancelConfirm";
            this.btnCancelConfirm.Size = new System.Drawing.Size(111, 35);
            this.btnCancelConfirm.TabIndex = 49;
            this.btnCancelConfirm.Text = "Cancel";
            this.btnCancelConfirm.UseVisualStyleBackColor = false;
            this.btnCancelConfirm.Click += new System.EventHandler(this.btnCancelConfirm_Click);
            // 
            // btnRealConfirm
            // 
            this.btnRealConfirm.BackColor = System.Drawing.Color.Lime;
            this.btnRealConfirm.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRealConfirm.Location = new System.Drawing.Point(226, 186);
            this.btnRealConfirm.Name = "btnRealConfirm";
            this.btnRealConfirm.Size = new System.Drawing.Size(111, 35);
            this.btnRealConfirm.TabIndex = 48;
            this.btnRealConfirm.Text = "Xác nhận";
            this.btnRealConfirm.UseVisualStyleBackColor = false;
            this.btnRealConfirm.Click += new System.EventHandler(this.btnRealConfirm_Click);
            // 
            // grCustomerInfo
            // 
            this.grCustomerInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.grCustomerInfo.BackColor = System.Drawing.Color.White;
            this.grCustomerInfo.Controls.Add(this.lbCustomerInfoConfirm);
            this.grCustomerInfo.Location = new System.Drawing.Point(398, 19);
            this.grCustomerInfo.Name = "grCustomerInfo";
            this.grCustomerInfo.Size = new System.Drawing.Size(116, 56);
            this.grCustomerInfo.TabIndex = 47;
            this.grCustomerInfo.TabStop = false;
            this.grCustomerInfo.Text = "Customer";
            // 
            // lbCustomerInfoConfirm
            // 
            this.lbCustomerInfoConfirm.AutoSize = true;
            this.lbCustomerInfoConfirm.BackColor = System.Drawing.Color.White;
            this.lbCustomerInfoConfirm.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbCustomerInfoConfirm.ForeColor = System.Drawing.Color.Blue;
            this.lbCustomerInfoConfirm.Location = new System.Drawing.Point(6, 21);
            this.lbCustomerInfoConfirm.Name = "lbCustomerInfoConfirm";
            this.lbCustomerInfoConfirm.Size = new System.Drawing.Size(54, 25);
            this.lbCustomerInfoConfirm.TabIndex = 0;
            this.lbCustomerInfoConfirm.Text = "......";
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.BackColor = System.Drawing.Color.White;
            this.groupBox3.Controls.Add(this.lbLineInfoConfirm);
            this.groupBox3.Location = new System.Drawing.Point(330, 19);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(62, 56);
            this.groupBox3.TabIndex = 44;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Line";
            // 
            // lbLineInfoConfirm
            // 
            this.lbLineInfoConfirm.AutoSize = true;
            this.lbLineInfoConfirm.BackColor = System.Drawing.Color.White;
            this.lbLineInfoConfirm.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbLineInfoConfirm.ForeColor = System.Drawing.Color.Blue;
            this.lbLineInfoConfirm.Location = new System.Drawing.Point(2, 21);
            this.lbLineInfoConfirm.Name = "lbLineInfoConfirm";
            this.lbLineInfoConfirm.Size = new System.Drawing.Size(54, 25);
            this.lbLineInfoConfirm.TabIndex = 0;
            this.lbLineInfoConfirm.Text = "......";
            // 
            // grDevice
            // 
            this.grDevice.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grDevice.BackColor = System.Drawing.Color.White;
            this.grDevice.Controls.Add(this.lbInfoErrConfirm);
            this.grDevice.ForeColor = System.Drawing.Color.Red;
            this.grDevice.Location = new System.Drawing.Point(6, 19);
            this.grDevice.Name = "grDevice";
            this.grDevice.Size = new System.Drawing.Size(319, 56);
            this.grDevice.TabIndex = 43;
            this.grDevice.TabStop = false;
            this.grDevice.Text = "Error Content";
            // 
            // lbInfoErrConfirm
            // 
            this.lbInfoErrConfirm.AutoSize = true;
            this.lbInfoErrConfirm.BackColor = System.Drawing.Color.White;
            this.lbInfoErrConfirm.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbInfoErrConfirm.Location = new System.Drawing.Point(6, 21);
            this.lbInfoErrConfirm.Name = "lbInfoErrConfirm";
            this.lbInfoErrConfirm.Size = new System.Drawing.Size(54, 25);
            this.lbInfoErrConfirm.TabIndex = 0;
            this.lbInfoErrConfirm.Text = "......";
            // 
            // lbContentConfirm
            // 
            this.lbContentConfirm.AutoSize = true;
            this.lbContentConfirm.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbContentConfirm.Location = new System.Drawing.Point(6, 85);
            this.lbContentConfirm.Name = "lbContentConfirm";
            this.lbContentConfirm.Size = new System.Drawing.Size(436, 20);
            this.lbContentConfirm.TabIndex = 1;
            this.lbContentConfirm.Text = "Nhập nội dung xác nhận Lỗi (Nội dung dưới 100 ký tự)";
            // 
            // txbContentConfirm
            // 
            this.txbContentConfirm.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txbContentConfirm.Location = new System.Drawing.Point(6, 108);
            this.txbContentConfirm.MaxLength = 100;
            this.txbContentConfirm.Multiline = true;
            this.txbContentConfirm.Name = "txbContentConfirm";
            this.txbContentConfirm.Size = new System.Drawing.Size(508, 62);
            this.txbContentConfirm.TabIndex = 0;
            // 
            // Confirm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(544, 261);
            this.Controls.Add(this.groupBox1);
            this.Name = "Confirm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Confirm";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Confirm_FormClosed);
            this.Load += new System.EventHandler(this.Confirm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.grCustomerInfo.ResumeLayout(false);
            this.grCustomerInfo.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.grDevice.ResumeLayout(false);
            this.grDevice.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lbContentConfirm;
        private System.Windows.Forms.TextBox txbContentConfirm;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label lbLineInfoConfirm;
        private System.Windows.Forms.GroupBox grDevice;
        private System.Windows.Forms.Label lbInfoErrConfirm;
        private System.Windows.Forms.GroupBox grCustomerInfo;
        private System.Windows.Forms.Label lbCustomerInfoConfirm;
        private System.Windows.Forms.Button btnCancelConfirm;
        private System.Windows.Forms.Button btnRealConfirm;
    }
}