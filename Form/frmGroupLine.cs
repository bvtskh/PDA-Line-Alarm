using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Ports;
using System.Threading.Tasks;
using System.Windows.Forms;
using Alarmlines.Class;

namespace Alarmlines
{
    public partial class fGroupLine : Form
    {
        public TaskCallback func;
        private List<LocatonObj> lstLocation = new List<LocatonObj>();
        public fGroupLine()
        {
            InitializeComponent();
        }
        public fGroupLine(List<LocatonObj> lst, TaskCallback f)
        {
            InitializeComponent();
            lstLocation = lst;
            func = f;
        }

        private void Setting_Load(object sender, EventArgs e)
        {
            cbbGroupLine.DataSource = LocationHelper.GetLocation();
            var groupLine = Ultils.GetValueRegistryKey("GroupLine");
            if (!string.IsNullOrEmpty(groupLine))
            {
                cbbGroupLine.Text = groupLine;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    Ultils.WriteRegistry("GroupLine", cbbGroupLine.Text);
                    this.Close();
                }
                catch
                {
                    lbState.Text = "Error.";
                    lbState.BackColor = Color.Red;
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
           // func();
        }

        private void Setting_FormClosed(object sender, FormClosedEventArgs e)
        {
           // func();
        }
    }
}
