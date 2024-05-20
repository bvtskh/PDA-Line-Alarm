namespace Alarmlines
{
    partial class login
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
            this.btnLogin = new System.Windows.Forms.Button();
            this.chDispass = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txbPass = new System.Windows.Forms.TextBox();
            this.txbUser = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnLogin
            // 
            this.btnLogin.Location = new System.Drawing.Point(18, 84);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(75, 23);
            this.btnLogin.TabIndex = 11;
            this.btnLogin.Text = "Login";
            this.btnLogin.UseVisualStyleBackColor = true;
            this.btnLogin.Click += new System.EventHandler(this.BtnLogin_Click);
            // 
            // chDispass
            // 
            this.chDispass.AutoSize = true;
            this.chDispass.Location = new System.Drawing.Point(116, 90);
            this.chDispass.Name = "chDispass";
            this.chDispass.Size = new System.Drawing.Size(85, 17);
            this.chDispass.TabIndex = 10;
            this.chDispass.Text = "Display pass";
            this.chDispass.UseVisualStyleBackColor = true;
            this.chDispass.CheckedChanged += new System.EventHandler(this.ChDispass_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(35, 56);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Password";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(35, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "User name";
            // 
            // txbPass
            // 
            this.txbPass.Location = new System.Drawing.Point(116, 53);
            this.txbPass.Name = "txbPass";
            this.txbPass.Size = new System.Drawing.Size(122, 20);
            this.txbPass.TabIndex = 7;
            this.txbPass.UseSystemPasswordChar = true;
            // 
            // txbUser
            // 
            this.txbUser.Location = new System.Drawing.Point(116, 17);
            this.txbUser.Name = "txbUser";
            this.txbUser.Size = new System.Drawing.Size(122, 20);
            this.txbUser.TabIndex = 6;
            // 
            // login
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(256, 125);
            this.Controls.Add(this.btnLogin);
            this.Controls.Add(this.chDispass);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txbPass);
            this.Controls.Add(this.txbUser);
            this.Name = "login";
            this.Text = "login";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.login_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.CheckBox chDispass;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txbPass;
        private System.Windows.Forms.TextBox txbUser;
    }
}