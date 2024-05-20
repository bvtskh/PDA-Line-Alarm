using Alarmlines.Class;
using CommonProject.MsgBox_AQ;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Alarmlines
{
    public partial class frmWoQty : Form
    {
        public Action<string> message;
        public frmWoQty(string caption)
        {
            InitializeComponent();
            this.labelCaption.Text = caption;
            this.FormBorderStyle = FormBorderStyle.None;
            this.btnClose.DialogResult = DialogResult.Cancel;
            this.button2.DialogResult = DialogResult.Yes;
        }

        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        private void panelTitleBar_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txbWoQty_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txbWoQty_TextChanged(object sender, EventArgs e)
        {
            message(txbMessage.Text);
        }
    }
}
