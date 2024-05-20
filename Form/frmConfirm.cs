using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Alarmlines
{
    public partial class Confirm : Form
    {
        private PDA_ErrorHistory_Confirm data = new PDA_ErrorHistory_Confirm();
        bool isConfirm = false;
        public Confirm(PDA_ErrorHistory_Confirm db)
        {
            InitializeComponent();
            data = db;
        }

        private void Confirm_Load(object sender, EventArgs e)
        {
            txbContentConfirm.Text = "";
            if (data == null) return;
            lbInfoErrConfirm.Text = data.ErrorContent;
            lbLineInfoConfirm.Text = data.Line;
            lbCustomerInfoConfirm.Text = data.Customer;
            isConfirm = false;
        }

        private void btnRealConfirm_Click(object sender, EventArgs e)
        {
            if (txbContentConfirm.Text != "")
            {
                DialogResult dialogResult = MessageBox.Show("Đã đúng lỗi cần xác nhận?", "Warring!!", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (dialogResult != DialogResult.OK)
                {
                    return;
                }
               
                this.Close();
            }
            else
            {
                MessageBox.Show("Bạn phải nhập nội dung xác nhận!");
                txbContentConfirm.Text = "";
            }
            isConfirm = true;
            this.Text = txbContentConfirm.Text;
        }

        private void btnCancelConfirm_Click(object sender, EventArgs e)
        {
            this.Text = "";
            isConfirm = false;
            this.Close();
        }

        private void Confirm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (isConfirm == false)
                this.Text = "";
            isConfirm = false;
        }
    }
}
