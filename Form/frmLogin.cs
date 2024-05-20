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

namespace Alarmlines
{
    public partial class login : Form
    {
        //decalre properties 
        List<LocatonObj> L = new List<LocatonObj>();
        public TaskCallback Ta;
        public string Username { get; set; }
        public string Password { get; set; }
        private bool validvalue = false;
        public login()
        {
            InitializeComponent();
        }

        public login(List<LocatonObj> lst, TaskCallback f)
        {
            InitializeComponent();
            L = lst;
            Ta = f;
        }

        //validate string 
        private bool StringValidator(string input)
        {
            string pattern = "[^a-zA-Z]";
            if (Regex.IsMatch(input, pattern))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        //validate integer 
        private bool IntegerValidator(string input)
        {
            string pattern = "[^0-9]";
            if (Regex.IsMatch(input, pattern))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        //clear user inputs 
        private void ClearTexts(string user, string pass)
        {
            user = String.Empty;
            pass = String.Empty;
        }
        //method to check if eligible to be logged in 
        internal bool IsLoggedIn(string user, string pass)
        {
            //check user name empty 
            if (string.IsNullOrEmpty(user))
            {
                MessageBox.Show("Enter the user name!");
                return false;

            }
            //check user name is valid type 
            else if (StringValidator(user) == true)
            {
                MessageBox.Show("Enter only text here");
                ClearTexts(user, pass);
                return false;
            }
            //check user name is correct 
            else
            {
                if (Const.USERNAME != user)
                {
                    MessageBox.Show("User name is incorrect!");
                    ClearTexts(user, pass);
                    return false;
                }
                //check password is empty 
                else
                {
                    if (string.IsNullOrEmpty(pass))
                    {
                        MessageBox.Show("Enter the passowrd!");
                        return false;
                    }
                    //check password is correct 
                    else if (Const.PASSWORD != pass)
                    {
                        MessageBox.Show("Password is incorrect");
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            validvalue = false;
            if (IsLoggedIn(txbUser.Text, txbPass.Text) == true)
            {
                validvalue = true;
                this.Close();               
            }
        }

        private void ChDispass_CheckedChanged(object sender, EventArgs e)
        {
            txbPass.UseSystemPasswordChar = !chDispass.Checked;
        }

        private void login_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (validvalue == true)
            {
                Setting f_identify = new Setting(L, Ta);
                f_identify.Show();
            }
            validvalue = false;
        }
    }
}
