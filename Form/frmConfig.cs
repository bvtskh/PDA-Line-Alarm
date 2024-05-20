using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.IO.Ports;
using CommonProject.Business;
using Alarmlines.Class;

namespace Alarmlines
{
    public partial class frmConfig : Form
    {
        public frmConfig()
        {
            InitializeComponent();
        }

        public void LoadDeviceNames()
        {
            cbbSerialPort.DataSource = SerialPort.GetPortNames();
            cbbBaudrate.DataSource = new List<int>() { 9600, 14400, 19200, 38400, 57600, 115200, 128000, 134400, 161280 };
        }

        private void FormConfig_Load(object sender, EventArgs e)
        {
            LoadDeviceNames();
        }

        private void btnSaveChanged_Click(object sender, EventArgs e)
        {
            var flag = string.IsNullOrEmpty(cbbSerialPort.Text);
            if (flag)
            {
                MessageBox.Show("Chưa chọn ComPort!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
            }
            flag = string.IsNullOrEmpty(txtRun.Text);
            if (flag)
            {
                MessageBox.Show("Chưa cài đặt tín hiệu OK!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
            }
            flag = string.IsNullOrEmpty(txtStop.Text);
            if (flag)
            {
                MessageBox.Show("Chưa cài đặt tín hiệu NG!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
            }

            flag = txtRun.Text == txtStop.Text;
            if (flag)
            {
                MessageBox.Show("Tín hiệu không hợp lệ!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
            }
            var value = cbbBaudrate.SelectedValue.ToString();
            int baudrate = 9600;
            if (!int.TryParse(value, out baudrate))
            {
                MessageBox.Show("BaudRate không hợp lệ!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
            }
            Ultils.WriteRegistry(Const.COM, cbbSerialPort.Text);
            Ultils.WriteRegistry(Const.SignalOK, txtRun.Text);
            Ultils.WriteRegistry(Const.SignalNG, txtStop.Text);
            MessageBox.Show("Save success!", "THÔNG BÁO", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }


        private void lblRefreshCOMPorts_Click(object sender, EventArgs e)
        {
        }


        private void btnTestSend_Click(object sender, EventArgs e)
        {
            CommPort comm = CommPort.Instance;
            comm.StatusChanged += OnStatusChanged;
            comm.Open(cbbSerialPort.Text);
            try
            {
                comm.Send(txtRun.Text);
                MessageBox.Show("Gửi thành công", "THÔNG BÁO", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception comException)
            {
                MessageBox.Show(comException.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void OnStatusChanged(string param)
        {
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            CommPort comm = CommPort.Instance;
            comm.StatusChanged += OnStatusChanged;
            comm.Open(cbbSerialPort.Text);
            try
            {
                comm.Send(txtStop.Text);
                MessageBox.Show("Gửi thành công", "THÔNG BÁO", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception comException)
            {
                MessageBox.Show(comException.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

    }
}
