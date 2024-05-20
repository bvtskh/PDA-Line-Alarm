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

namespace Alarmlines
{
    public partial class Setting : Form
    {
        public TaskCallback func;
        private DeviceSetting setting;
        private List<LocatonObj> lstLocation = new List<LocatonObj>();
        public Setting(List<LocatonObj> lst, TaskCallback f)
        {
            InitializeComponent();
            lstLocation = lst;
            func = f;
        }

        private void Setting_Load(object sender, EventArgs e)
        {
            cmbComport.DataSource = SerialPort.GetPortNames().ToList();
            
            cmbBaudrate.DataSource = new List<string>() { "4800", "9600", "19200", "38400", "56000", "115200" };
            cmbDataBits.DataSource = new List<string>() { "8", "9" };
            cmbParity.DataSource = new List<string>() { "None", "Odd", "Even", "Mark", "Space" };
            cmbStopBits.DataSource = new List<string>() { "0", "1", "1.5", "2" };
            DeviceSetting.ReadXML<DeviceSetting>(out setting, Const.pathConfig);
            // set value
            cmbComport.Text = setting.portName;
            cmbBaudrate.Text = setting.baudRate.ToString();
            cmbDataBits.Text = setting.dataBits.ToString();
            cmbParity.Text = setting.parity;
            cmbStopBits.Text = setting.stopBits;
            txbConfirmTime.Text = setting.ConfirmTime.ToString();
            txbQueryTime.Text = setting.queryTime.ToString();
            cmbQueryBeforeHours.Text = setting.QueryBeforeHours.ToString();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    setting.baudRate = Convert.ToInt32(cmbBaudrate.Text);
                    setting.portName = cmbComport.Text;
                    setting.dataBits = Convert.ToInt32(cmbDataBits.Text);
                    setting.parity = cmbParity.Text;
                    setting.stopBits = cmbStopBits.Text;
                    setting.queryTime = Convert.ToInt32(txbQueryTime.Text);
                    setting.ConfirmTime = Convert.ToInt32(txbConfirmTime.Text);
                    setting.QueryBeforeHours = Convert.ToInt32(cmbQueryBeforeHours.Text);
                    if (setting.QueryBeforeHours >2) 
                    DeviceSetting.WriteXML<DeviceSetting>(setting, Const.pathConfig);
                    lbState.Text = "Save Successful!";
                    lbState.BackColor = Color.Green;
                }
                catch
                {
                    lbState.Text = "Du lieu khong hop le! Kiem tra lai.";
                    lbState.BackColor = Color.Red;
                }                
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
            func();
        }

        private void Setting_FormClosed(object sender, FormClosedEventArgs e)
        {
            func();
        }

        private void CheckboxAdvance_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckboxAdvance.Checked)
            {
                txbConfirmTime.ReadOnly = false;
                txbQueryTime.ReadOnly = false;
                cmbQueryBeforeHours.Visible = true;
            }
            else {
                txbConfirmTime.ReadOnly = true;
                txbQueryTime.ReadOnly = true;
                cmbQueryBeforeHours.Visible = false;
            }
        }
    }
}
