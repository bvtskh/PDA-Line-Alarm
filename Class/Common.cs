using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Drawing;

namespace Alarmlines
{    
    public class Common
    {
        public static void RunProcess(string runFileName)
        {
            string ExePath = Path.Combine(Application.StartupPath, runFileName);
            System.Diagnostics.Process.Start(ExePath);
        }

        public static void KillProcess(string processName)
        {
            System.Diagnostics.Process[] pros = System.Diagnostics.Process.GetProcessesByName((processName));
            foreach (System.Diagnostics.Process pro in pros)
            {
                try
                {
                    pro.Kill();
                }
                catch (Exception ex)
                {

                    continue;
                }
            }
        }

        private static string ShowMessage_Password(string title, string promptText)
        {
            Form form = new Form();
            Label label = new Label();
            TextBox textBox = new TextBox();
            Button buttonOk = new Button();
            Button buttonCancel = new Button();

            form.Text = title;
            label.Text = promptText;

            buttonOk.Text = "OK";
            buttonCancel.Text = "Cancel";
            buttonOk.DialogResult = DialogResult.OK;
            buttonCancel.DialogResult = DialogResult.Cancel;

            label.SetBounds(9, 20, 372, 13);
            textBox.SetBounds(12, 36, 372, 20);
            buttonOk.SetBounds(228, 72, 75, 23);
            buttonCancel.SetBounds(309, 72, 75, 23);

            label.AutoSize = true;
            textBox.Anchor = textBox.Anchor | AnchorStyles.Right;
            textBox.PasswordChar = '*';
            buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            form.ClientSize = new Size(396, 107);
            form.Controls.AddRange(new Control[] { label, textBox, buttonOk, buttonCancel });
            form.ClientSize = new Size(Math.Max(300, label.Right + 10), form.ClientSize.Height);
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MinimizeBox = false;
            form.MaximizeBox = false;
            form.AcceptButton = buttonOk;
            form.CancelButton = buttonCancel;

            DialogResult dialogResult = form.ShowDialog();

            if (dialogResult == DialogResult.OK) return textBox.Text;
            else return "";
        }
        public static bool CheckPassword(string Password)
        {
            bool result = false;

            if (Common.ShowMessage_Password("Login", "Please input Password in here") == Password) result = true;
            else
            {
                MessageBox.Show("Password is Wrong! Pls input again", "Error");
                result = false;
            }
            return result;
        }
        public static string ShowMessage(string title, string promptText)
        {
            Form form = new Form();
            Label label = new Label();
            TextBox textBox = new TextBox();
            Button buttonOk = new Button();
            Button buttonCancel = new Button();

            form.Text = title;
            label.Text = promptText;

            buttonOk.Text = "OK";
            buttonCancel.Text = "Cancel";
            buttonOk.DialogResult = DialogResult.OK;
            buttonCancel.DialogResult = DialogResult.Cancel;

            label.SetBounds(9, 20, 372, 13);
            textBox.SetBounds(12, 36, 372, 20);
            buttonOk.SetBounds(228, 72, 75, 23);
            buttonCancel.SetBounds(309, 72, 75, 23);

            label.AutoSize = true;
            textBox.Anchor = textBox.Anchor | AnchorStyles.Right;
            buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            form.ClientSize = new Size(396, 107);
            form.Controls.AddRange(new Control[] { label, textBox, buttonOk, buttonCancel });
            form.ClientSize = new Size(Math.Max(300, label.Right + 10), form.ClientSize.Height);
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MinimizeBox = false;
            form.MaximizeBox = false;
            form.AcceptButton = buttonOk;
            form.CancelButton = buttonCancel;

            DialogResult dialogResult = form.ShowDialog();

            if (dialogResult == DialogResult.OK) return textBox.Text;
            else if (dialogResult == DialogResult.Cancel) return "Cancle";
            else return "";
        }
        public static int Get_SoPhut_onDay(DateTime CurrentTime)
        {
            int Result = 0;
            if (CurrentTime.Hour >= 8 && CurrentTime.Hour < 24)
            {
                DateTime today8h = new DateTime(CurrentTime.Year, CurrentTime.Month, CurrentTime.Day, 8, 0, 0);
                Result = (int)(CurrentTime - today8h).TotalMinutes;
            }
            else if (CurrentTime.Hour < 8)
            {
                DateTime today0h = new DateTime(CurrentTime.Year, CurrentTime.Month, CurrentTime.Day, 0, 0, 0);
                Result = 16 * 60 + (int)(CurrentTime - today0h).TotalMinutes;
            }

            return Result;
        }

    public static void ExportExcelFromDatagridview(DataGridView dgvData)
        {
            try
            {
                using (SaveFileDialog sfd = new SaveFileDialog() { Filter = "Excel 97-2003 Workbook|*.xls|Excel Workbook|*.xlsx" })
                {
                    //sfd.InitialDirectory = "C:\\";
                    sfd.Title = "Save Excel Files";
                    //sfd.CheckFileExists = true;
                    sfd.CheckPathExists = true;
                    sfd.DefaultExt = "xls";
                    //sfd.Filter = "Text files (.txt)|*.txt|All files (.*)|*.*";
                    sfd.FilterIndex = 2;
                    sfd.RestoreDirectory = true;
                    sfd.FileName = DateTime.Now.ToString("yyyyMMdd");
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        XSSFWorkbook wb = new XSSFWorkbook();
                        ISheet sheet = wb.CreateSheet();

                        // tạo header file (trong file excel)
                        var row0 = sheet.CreateRow(0);
                        int cellIndex = 0;
                        foreach (DataGridViewColumn item in dgvData.Columns)
                        {
                            row0.CreateCell(cellIndex).SetCellValue(item.HeaderText);
                            cellIndex++;
                        }

                        // nội dung:
                        int rowIndex = 1;
                        foreach (DataGridViewRow row in dgvData.Rows)
                        {
                            var newRow = sheet.CreateRow(rowIndex);

                            int _cellIndex = 0;
                            foreach (DataGridViewColumn item in dgvData.Columns)
                            {
                                newRow.CreateCell(_cellIndex).SetCellValue(row.Cells[_cellIndex].Value != null ? row.Cells[_cellIndex].Value.ToString() : "");
                                _cellIndex++;
                            }
                            // tăng index
                            rowIndex++;
                        }

                        FileStream fs = new FileStream(sfd.FileName, FileMode.OpenOrCreate);
                        wb.Write(fs);

                        MessageBox.Show("Đã xuất xong");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("File nay dang mo, hay dong file truoc khi xu ly" + ex.ToString());
            }
        }
    }
}
