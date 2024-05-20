using Alarmlines.Class;
using CommonProject.MsgBox_AQ;
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
    public partial class frmReplaceConfirm : Form
    {
        WODetailInfo info;
        public frmReplaceConfirm(WODetailInfo info)
        {
            InitializeComponent();
            this.info = info;
            lblTitle.Text = $"Xác nhận linh kiện {info.UPN}";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            foreach (RadioButton rBtn in this.Controls.OfType<RadioButton>())
            {
                if (rBtn.Checked)
                {
                    info.Remark = rBtn.Text;
                    break;
                }
            }
            var result = SMTHelper.UpdatePDAByConfirm(info);
            var dialog = RJMessageBox.Show(result,"Thông báo",MessageBoxButtons.OK,MessageBoxIcon.Information);
            Close();
        }
    }
}
