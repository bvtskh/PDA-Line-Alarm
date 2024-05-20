using Alarmlines.Class;
using Alarmlines.Class.WOVerify;
using Alarmlines.DB;
using CommonProject;
using CommonProject.Business;
using CommonProject.Loading.LoadingClass;
using CommonProject.MsgBox_AQ;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZedGraph;

namespace Alarmlines
{
    public delegate void TaskCallback();
    public delegate void TaskCallbackConfirm(string com);


    public partial class Main : Form
    {

        private DeviceSetting setting = new DeviceSetting();
        private string inputData = "";
        private SerialPort comControl = new SerialPort();
        private List<PDA_ErrorHistory_Confirm> lstTblError = new List<PDA_ErrorHistory_Confirm>();
        private List<PDA_ErrorHistory_Confirm> lstTblnonAlarmError = new List<PDA_ErrorHistory_Confirm>();
        private List<PDA_ErrorHistory_Confirm> lstDisplayView = new List<PDA_ErrorHistory_Confirm>();
        List<Tbl_PDADefineError> lstDefineErr = new List<Tbl_PDADefineError>();
        List<Tbl_PDADefineError> lstDefineMidErr = new List<Tbl_PDADefineError>();
        private List<ErrorInfo> _lstFilterError = new List<ErrorInfo>();
        private List<CustomerObj> _lstCustomer = new List<CustomerObj>();
        private List<LocatonObj> _lstLocation = new List<LocatonObj>();
        private bool isConnectDevice = false, isAlarmline = false;
        private int devTimeout = 0, QueryTimeCount = 0;
        public int IDErr = -1;
        private string SystemStateTemp = "";
        private bool initialComplete = false;
        private string currLine;
        private List<DB.Location> locationList;

        #region FormProcess
        public Main()
        {
            InitializeComponent();

            //Hiển thị thông tin giá trị trên biểu đồ:
            ZgDrawMonitor.IsShowPointValues = true;
            // Đăng ký sự kiện hiển thị lên kết quả ở các cột khi trỏ chuột vào vị trí đó
            this.ZgDrawMonitor.PointValueEvent += new ZedGraph.ZedGraphControl.PointValueHandler(this.zedGraph_GeneralReport_pcs_PointValueEvent);
        }

        private void Main_Load(object sender, EventArgs e)
        {

            currLine = Class.Ultils.GetValueRegistryKey("GroupLine");//Lấy khu vực line hiện tại
            locationList = LineHelper.GetLocations();
            string PathFileCustomer = Const.pathCustomer + "\\Customer.csv";
            string PathFileLocation = Const.pathLocation + "\\Location.csv";
            if (!Directory.Exists(Const.pathFolderConfig)) Directory.CreateDirectory(Const.pathFolderConfig);
            if (!File.Exists(Const.pathConfig)) SaveInitSystem();
            if (!Directory.Exists(Const.pathLog)) Directory.CreateDirectory(Const.pathLog);
            if (!Directory.Exists(Const.pathCustomer)) Directory.CreateDirectory(Const.pathCustomer);
            if (!Directory.Exists(Const.pathLocation)) Directory.CreateDirectory(Const.pathLocation);
            if (!Directory.Exists(Const.pathExport)) Directory.CreateDirectory(Const.pathExport);
            //tạo file config system 
            if (!File.Exists(PathFileCustomer)) SaveDefaultCustomer();
            if (!File.Exists(PathFileLocation)) SaveDefaultLocation();

            DeviceSetting.ReadXML<DeviceSetting>(out setting, Const.pathConfig);
            comControl.DataReceived += new SerialDataReceivedEventHandler(ComControl_DataReceived);
            _lstCustomer = CustomerObj.ReadCSV(PathFileCustomer);
            cboCusSearch.DataSource = _lstCustomer.Select(o => o.name).ToList();
            _lstLocation = LocatonObj.ReadCSV(PathFileLocation);
            cbbAreaMidErrMonitor.DataSource =
            cboAreaDataEarlyHour.DataSource =
            cboAreaQuery.DataSource =
            cbbAreaReplace.DataSource =
            _lstLocation.Select(o => o.name).ToList();

            //khu vực mặc định:
            cboAreaQuery.Text = Properties.Settings.Default.Area;
            cbbProcessSearch.DataSource = Const.lstProcessPDA.Select(o => o.name_process).ToList();
            cbbProcessSearch.Text = Const.lstProcessPDA[0].name_process;
            cbbSelectAnalysis.DataSource = _lstLocation.Select(o => o.name).ToList();
            cbbSelectAnalysis.Text = Properties.Settings.Default.Area;
            dtEndSearch.Value = DateTime.Now;
            dtStartSearch.Value = DateTime.Now.AddDays(-3);
            DateTime T = new DateTime();
            T = dtEndAnalysis.Value = DateTime.Now;
            dtStartAnalysis.Value = new DateTime(T.Year, T.Month, 1, 8, 0, 0);
            cbbObjectAnalysis.DataSource = Const.lstObjAnalysis;
            cbbObjectAnalysis.Text = Const.lstObjAnalysis[0];
            //check version phan mem, load define error
            var ver_soft = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            string verInfo = Const.VersionPath;
            string ver_db = string.Empty;
            this.Text = Const.SoftwareName + ver_soft;
            using (DBContext db = new DBContext())
            {
                lstDefineErr = db.Tbl_PDADefineError.Where(o => o.ErrorLevel.Contains("High")).ToList();
                lstDefineMidErr = db.Tbl_PDADefineError.Where(o => o.ErrorLevel.Contains("Mid")).ToList();
                var lst = db.Tbl_PDAErrorVersion.ToList();
                var item = lst.LastOrDefault();
                ver_db = item == null ? "" : item.VersionCode;// Sửa cái đoạn này k check null
            }

            string msg = string.Format("Đã lên version {0}. Hãy vào đường dẫn  {1} {0}", ver_db, verInfo + "SMT PDA Error Information Ver");
            if (ver_soft != ver_db)
            {
                MessageBox.Show(msg, "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Application.Exit();
                return;
            }

            CalculatorChart.SetDataList(lstDefineErr, _lstCustomer, _lstLocation, lstDefineMidErr);
            QueryTimer.Enabled = true;
            QueryTimeCount = 0;
            //khởi tạo hoàn thành
            initialComplete = true;
            //load dữ liệu
            QueryProcess(Const.LOADPART_PROC, Const.HIGH_LEVEL);
            QueryTimer.Enabled = true;
            QueryDevTimer.Enabled = true;
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (comControl.IsOpen)
            {
                SendCom("#OFF*");
            }
            KillProcess("LoadTitle");
        }

        private string zedGraph_GeneralReport_pcs_PointValueEvent(ZedGraph.ZedGraphControl sender, ZedGraph.GraphPane pane, ZedGraph.CurveItem curve, int iPt)
        {
            //CusNameQuery = curve.Label.Text;
            //DayQuery = Convert.ToInt16(pane.XAxis.Scale.TextLabels[iPt]);
            return curve.Label.Text + " = " + curve.Points[iPt].Y.ToString();
        }

        #endregion

        #region ComPort

        public bool CheckComport()
        {
            try
            {
                if (comControl.IsOpen) return true;
                comControl.PortName = "COM" + Properties.Settings.Default.ComNo.ToString();
                comControl.BaudRate = 9600;
                comControl.DataBits = 8;
                comControl.Parity = Parity.None;
                comControl.StopBits = StopBits.One;

                comControl.ReadTimeout = 500;
                comControl.WriteTimeout = 500;
                comControl.ReadBufferSize = 2000;
                comControl.WriteBufferSize = 1000;

                comControl.Open();
                comControl.DiscardInBuffer();
                comControl.DiscardOutBuffer();
                if (comControl.IsOpen)
                {
                    st_lbStateDevice.Text = "Connected";
                    st_lbStateDevice.BackColor = Color.Green;
                    st_lbStateDevice.ForeColor = System.Drawing.Color.Black;
                    SendCom("#RESET*");
                    st_lbInfoCom.Text = comControl.PortName + ", " + setting.baudRate.ToString() + " bps, " + setting.dataBits.ToString() + " bit, ";
                    st_lbInfoCom.ForeColor = Color.Blue;
                    return true;
                }
                else
                {
                    st_lbStateDevice.Text = "Disconnect";
                    st_lbStateDevice.ForeColor = System.Drawing.Color.Black;
                    return false;
                }

            }
            catch (Exception)
            {
                st_lbInfoCom.Text = comControl.PortName + ", " + setting.baudRate.ToString() + " bps, " + setting.dataBits.ToString() + " bit, ";
                st_lbStateDevice.Text = "Disconnect";
                st_lbStateDevice.ForeColor = System.Drawing.Color.Black;
                return false;
            }
        }

        public void SaveInitSystem()
        {
            setting.portName = "COM3";
            setting.baudRate = 9600;
            setting.dataBits = 8;
            setting.parity = "None";
            setting.stopBits = "1";
            setting.queryTime = 60;// 15 phut 1 lan
            setting.ConfirmTime = 120;// cho leader confirm loi trong 2 phut
            setting.QueryBeforeHours = 12;// can confirm
            setting.Location = "ALL";
            DeviceSetting.WriteXML<DeviceSetting>(this.setting, Const.pathConfig);
        }

        //2. xử lý nhận dữ liệu từ COM control
        private void ComControl_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            inputData = comControl.ReadExisting();
            if (inputData != String.Empty)
            {
                ReceicedTex_comControl(inputData);
            }
        }
        delegate void SetTextCallback_comControl(string text);
        public void ReceicedTex_comControl(string text)
        {
            if (txtRecei_comControl.InvokeRequired)
            {
                SetTextCallback_comControl d = new SetTextCallback_comControl(ReceicedTex_comControl);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                txtRecei_comControl.Text += text;
            }
        }

        private void txtRecei_comControl_TextChanged(object sender, EventArgs e)
        {
            if (txtRecei_comControl.Text.Contains("*") == true)
            {
                txtRecei_comControl.Text.Replace("\n", "");
                st_lbRecei_comControl.Text = "";
                st_lbRecei_comControl.Text = txtRecei_comControl.Text;
                txtRecei_comControl.Text = "";
            }
        }

        private void st_lbRecei_comControl_TextChanged(object sender, EventArgs e)
        {
            if (st_lbRecei_comControl.Text != "")
            {
                // khoi dong xong gui thong tin xac nhan model loai gi(do 1 dong ho/2 dong ho)
                if (st_lbRecei_comControl.Text.Contains("READY"))
                {
                    isConnectDevice = true;
                    devTimeout = 0;
                }
                // neu may gui OK thi tu dong link WIP
                else if (st_lbRecei_comControl.Text.Contains("OK"))
                {

                }
                // Neu may gui NG hien thong bao NG va Enable
                else if (st_lbRecei_comControl.Text.Contains("NG"))
                {

                }
            }
        }

        private void SendCom(string cmd)
        {
            if (comControl.IsOpen == false) return;
            st_lbSend.Text = "";
            st_lbSend.ForeColor = Color.Blue;
            comControl.WriteLine(cmd);
            st_lbSend.Text = cmd;
        }

        #endregion Comport

        #region Tab các lỗi nghiêm trọng 

        void DislayConnectServer()
        {
            tssErrorSystem.Text = "Có dữ liệu mới";
            tssErrorSystem.ForeColor = Color.Blue;
        }

        private void DisplayError(int count)
        {
            tssErrorSystem.Text = "Chưa lấy được dữ liệu";
            tssErrorSystem.ForeColor = Color.Red;
            //tssErrorSystem.BackColor = Color.Red;
            QueryTimer.Enabled = true;
            QueryTimer.Interval = setting.queryTime * 1000;
            QueryTimeCount = 0;
        }

        private void PushDatabase(List<PDA_ErrorHistory_Confirm> lstRemove, int index, string comment)
        {
            tssMove.Text = "";
            var curDate = DateTime.Now;
            DateTime beforeTime = new DateTime();
            //Tính toán dữ liệu 
            if (setting.QueryBeforeHours > 0)
                beforeTime = curDate.AddHours((-1) * setting.QueryBeforeHours);
            else
                beforeTime = DateTime.Now;
            try
            {
                using (var db = new DBContext())
                {
                    foreach (var p in lstRemove)
                    {
                        var itemBackUp = db.PDA_ErrorHistoryBk.FirstOrDefault(o => o.IdErr == p.id);
                        if (itemBackUp == null)
                        {
                            var newBU = new PDA_ErrorHistoryBk
                            {
                                ErrorTime = p.ErrorTime,
                                Line = p.Line,
                                Model = p.Model,
                                WO = p.WO,
                                PartCode = p.PartCode,
                                ErrorContent = p.ErrorContent,
                                OperatorCode = p.OperatorCode,
                                Customer = p.Customer,
                                Location = p.Location,
                                Comment = comment,
                                IdErr = p.id,
                                ProcessID = p.ProcessID,
                                Slot = p.Slot
                            };
                            db.PDA_ErrorHistoryBk.Add(newBU);
                        }
                        else
                        {
                            // update them thong tin o day
                        }
                    }
                    db.SaveChanges();
                    //foreach (var p in lstTblError)
                    //{
                    //    var item = lstRemove.FirstOrDefault(p);
                    //    if (item != null)
                    //        p.isConfirm = true;// da duoc xac nhan
                    //}
                    lstTblError = GetDatabase(cboAreaQuery.Text, setting.QueryBeforeHours, Const.HIGH_LEVEL);
                    tssMove.Text = "OK";
                }
                // Cap nhat vao dgv
                QueryTimer.Enabled = true;
                QueryTimeCount = 0;
                DislayConnectServer();
            }
            catch
            {
                DisplayError(1);
            }
        }

        //GetDatabase
        //Input: 1. AreaFilter: Lọc theo khu vực  2.TimeFilter: Lọc theo thời gian
        //Output: List các lỗi chưa được xác nhận

        private List<PDA_ErrorHistory_Confirm> GetDatabase(string AreaFilter, int TimeFilter, string level)
        {
            if (initialComplete == false) return null;
            DisplayRowSelect(0, Const.CLEAR_LEVEL);
            //if (tabControl.SelectedTabIndex != Const.MONITOR_TAB) return;
            var curDate = DateTime.Now;
            DateTime beforeTime = new DateTime();

            //Tính toán dữ liệu 
            if (TimeFilter > 0)
                beforeTime = curDate.AddHours((-1) * TimeFilter);
            else
                beforeTime = DateTime.Now;
            //RunProcess("LoadTitle.exe");
            try
            {

                List<PDA_ErrorHistory> lstErrorCurrent = new List<PDA_ErrorHistory>();
                List<PDA_ErrorHistoryBk> lstErroBackupCurrent = new List<PDA_ErrorHistoryBk>();
                List<PDA_ErrorHistory_Confirm> lstResultTime = new List<PDA_ErrorHistory_Confirm>();
                List<PDA_ErrorHistory_Confirm> lstResultError = new List<PDA_ErrorHistory_Confirm>();
                using (var db = new DBContext())
                {
                    lstErrorCurrent = db.PDA_ErrorHistory.Where(o => o.ErrorTime >= beforeTime && o.ErrorTime <= curDate).ToList();
                    lstErroBackupCurrent = db.PDA_ErrorHistoryBk.Where(o => o.ErrorTime >= beforeTime && o.ErrorTime <= curDate).ToList();
                }
                if (lstErrorCurrent == null) return null;
                //if (lstErroBackupCurrent == null || lstErroBackupCurrent.Count == 0) return null;
                foreach (var item in lstErrorCurrent)
                {
                    PDA_ErrorHistoryBk itemBackUp = new PDA_ErrorHistoryBk();
                    PDA_ErrorHistory_Confirm p = new PDA_ErrorHistory_Confirm();
                    p.Customer = item.Customer; p.ErrorContent = item.ErrorContent; p.ErrorTime = item.ErrorTime;
                    p.id = item.id; p.Line = item.Line; p.Location = item.Location; p.Model = item.Model; p.OperatorCode = item.OperatorCode;
                    p.PartCode = item.PartCode; p.ProcessID = item.ProcessID; p.Slot = item.Slot; p.SystemId = item.SystemId; p.WO = item.WO;
                    // so sanh item da ton tai trong bang Backup chua?
                    if (lstErroBackupCurrent != null)
                        itemBackUp = lstErroBackupCurrent.FirstOrDefault(o => o.IdErr == item.id);
                    // neu chua thi cap nhat vao list hien thi khi bản ghi không có trong bảng backup và bảng errors
                    //if (itemBackUp == null || lstErroBackupCurrent == null)
                    //{
                    //    lstResultTime.Add(item);
                    //}
                    if (itemBackUp == null || lstErroBackupCurrent == null) p.isConfirm = false;
                    else if (itemBackUp != null) p.isConfirm = true;
                    lstResultTime.Add(p);
                }
                // Loc theo cac loi quan trong, loc theo khu vuc của line
                if ((lstDefineErr == null || lstDefineErr.Count == 0) && level == Const.HIGH_LEVEL) return null;
                if ((lstDefineMidErr == null || lstDefineMidErr.Count == 0) && level == Const.MID_LEVEL) return null;
                if ((lstDefineErr.Count > 0 && level == Const.HIGH_LEVEL) || (lstDefineMidErr.Count > 0 && level == Const.MID_LEVEL))
                {
                    foreach (var p in lstResultTime)
                    {
                        var IfindArea = _lstLocation.FirstOrDefault(o => o.lstline.Contains(p.Line));
                        var groupId = Class.Ultils.GetValueRegistryKey("GroupLine");
                        if (IfindArea != null) p.Location = IfindArea.name;
                        Tbl_PDADefineError IfindError = new Tbl_PDADefineError();
                        if (level == Const.HIGH_LEVEL)
                            IfindError = lstDefineErr.FirstOrDefault(o => o.PDAErrorContent.Contains(p.ErrorContent));
                        else if (level == Const.MID_LEVEL)
                            IfindError = lstDefineMidErr.FirstOrDefault(o => o.PDAErrorContent.Contains(p.ErrorContent));

                        if (IfindError != null)
                        {
                            if (AreaFilter == "ALL")
                            {
                                if ((Properties.Settings.Default.Verify == Const.UNVISIBLE && p.ProcessID != 2) || (Properties.Settings.Default.Verify == Const.VISIBLE))
                                {
                                    if (cbbLangMonitor.Text == "VN") p.ErrorContent = IfindError.VNContent;
                                    p.Customer = CustomerObj.getname(p.Customer, _lstCustomer);
                                    lstResultError.Add(p);
                                }
                            }
                            else if (p.Location == AreaFilter)
                            {
                                var countgroupbyarea = LocationHelper.GetLocationByArea(AreaFilter);
                                if ((Properties.Settings.Default.Verify == Const.UNVISIBLE && p.ProcessID != 2) || (Properties.Settings.Default.Verify == Const.VISIBLE))
                                {
                                    if (cbbLangMonitor.Text == "VN") p.ErrorContent = IfindError.VNContent;
                                    if (countgroupbyarea.Count() == 0)
                                    {
                                        p.Customer = CustomerObj.getname(p.Customer, _lstCustomer);
                                        lstResultError.Add(p);
                                    }
                                    else
                                    {
                                        var checkgroupId = locationList.FirstOrDefault(c => c.LineId == p.Line && c.GroupID == groupId);
                                        if (checkgroupId != null)
                                        {
                                            p.Customer = CustomerObj.getname(p.Customer, _lstCustomer);
                                            lstResultError.Add(p);
                                        }
                                    }

                                    //else
                                    //{
                                    //    p.Customer = CustomerObj.getname(p.Customer, _lstCustomer);
                                    //}
                                }
                            }

                        }
                    }
                }
                KillProcess("LoadTitle");
                DislayConnectServer();
                //var  lst = lstResultError.Where(o=>o.isConfirm == false).ToList();
                //if (lst != null) {
                //    lbErrorAlarm.Text = lst.Count.ToString();
                //    lbErrorAlarm.ForeColor = Color.Red;
                //    if (lst.Count == 0)
                //        lbErrorAlarm.ForeColor = Color.Blue;
                //}
                return lstResultError;
            }
            catch
            {
                DisplayError(2);
                KillProcess("LoadTitle");
                return null;
            }
        }

        //Lọc dữ liệu theo công đoạn bắn PDA(1.LoadPart, 2.VerifyPart, 3.ReloadPart), theo mức độ lỗi
        private void QueryProcess(int process, string level)
        {
            tstrQueryState.Text = "Query Running";
            tstrQueryState.ForeColor = Color.Blue;

            //Xóa list cũ cập nhật list mới 
            if (level == Const.HIGH_LEVEL && process != Const.NON_ERR_PROC)
            {
                if (lstTblError != null) lstTblError.Clear();
            }
            else if (level == Const.MID_LEVEL)
            {
                if (lstTblnonAlarmError != null) lstTblnonAlarmError.Clear();
            }

            //list lỗi theo mức lỗi (Level) chưa được xử lý
            if (level == Const.HIGH_LEVEL && process != Const.NON_ERR_PROC)
            {
                lstTblError = GetDatabase(cboAreaQuery.Text, setting.QueryBeforeHours, level);
                if (lstTblError == null)
                {
                    tssErrorSystem.Text = "Không có dữ liệu";
                    tssErrorSystem.ForeColor = Color.Red;
                    return;
                }
            }
            else if (level == Const.MID_LEVEL)
            {
                lstTblnonAlarmError = GetDatabase(cbbAreaMidErrMonitor.Text, setting.QueryBeforeHours, level);
                if (lstTblnonAlarmError == null)
                {
                    MessageBox.Show("Phần mềm đang không lấy được dữ liệu lỗi không nghiêm trọng từ Server");
                    return;
                }
            }
            // Hiển thị thông tin lên Datagridview            
            RefreshDatagridView(process, level);
            // Hiển thị lên đồ thị
            if (level == Const.HIGH_LEVEL/* && (process == Const.RELOAD_PROC || process == Const.LOADPART_PROC)*/)
                GeneralReportMonitor(ZgDrawMonitor, level, Const.DRAW_DAY);
            //else if (level == Const.HIGH_LEVEL && (process == Const.VERIFY_PROC))
            //    GeneralReportMonitor(ZgVerify, level, Const.DRAW_DAY);
            else
                GeneralReportMonitor(ZgMidErr, level, Const.DRAW_DAY);
            tstrQueryState.Text = "Query Waiting";
            tstrQueryState.ForeColor = Color.Black;

            //Tính toán line phát sinh lỗi nhiều nhất
            DateTime curTime = DateTime.Now;
            var sophuttrongngay = Common.Get_SoPhut_onDay(curTime);
            var QueryTime = new DateTime(curTime.Year, curTime.Month, curTime.Day, 8, 0, 0);
            if (sophuttrongngay >= 960)
                QueryTime = QueryTime.AddDays(-1);
            //RunProcess("LoadTitle.exe");
            using (var db = new DBContext())
            {
                var lstDayCurrent = db.PDA_ErrorHistory.Where(o => o.ErrorTime >= QueryTime && o.ErrorTime < curTime).ToList();

                List<PDA_ErrorHistory> lstResult = new List<Alarmlines.PDA_ErrorHistory>();
                foreach (var p in lstDayCurrent)
                {
                    var IfindArea = _lstLocation.FirstOrDefault(o => o.lstline.Contains(p.Line));
                    if (IfindArea != null)
                    {
                        p.Location = IfindArea.name;
                        if (((level == Const.HIGH_LEVEL) && (IfindArea.name == cboAreaQuery.Text || cboAreaQuery.Text == "ALL") && lstDefineErr.FirstOrDefault(o => o.PDAErrorContent.Contains(p.ErrorContent)) != null)
                            || ((level == Const.MID_LEVEL) && (IfindArea.name == cbbAreaMidErrMonitor.Text || cbbAreaMidErrMonitor.Text == "ALL") && lstDefineMidErr.FirstOrDefault(o => o.PDAErrorContent.Contains(p.ErrorContent)) != null))
                        {
                            if ((Properties.Settings.Default.Verify == Const.UNVISIBLE && p.ProcessID != 2) || (Properties.Settings.Default.Verify == Const.VISIBLE))
                                lstResult.Add(p);
                        }
                    }
                }
                //bo trung lap loi
                var lsttrunglap = lstResult.GroupBy(o => new { o.PartCode, o.WO, o.Model, o.Slot, o.Line }).Select(o => new PDA_ErrorHistory()
                {
                    id = o.OrderBy(r => r.ErrorTime).LastOrDefault().id,
                    Customer = o.OrderBy(r => r.ErrorTime).LastOrDefault().Customer,
                    ErrorTime = o.OrderBy(r => r.ErrorTime).LastOrDefault().ErrorTime,
                    ErrorContent = o.OrderBy(r => r.ErrorTime).LastOrDefault().ErrorContent,
                    Line = o.OrderBy(r => r.ErrorTime).LastOrDefault().Line,
                    Location = o.OrderBy(r => r.ErrorTime).LastOrDefault().Location,
                    Model = o.OrderBy(r => r.ErrorTime).LastOrDefault().Model,
                    OperatorCode = o.OrderBy(r => r.ErrorTime).LastOrDefault().OperatorCode,
                    PartCode = o.OrderBy(r => r.ErrorTime).LastOrDefault().PartCode,
                    ProcessID = o.OrderBy(r => r.ErrorTime).LastOrDefault().ProcessID,
                    Slot = o.OrderBy(r => r.ErrorTime).LastOrDefault().Slot,
                    SystemId = o.OrderBy(r => r.ErrorTime).LastOrDefault().SystemId,
                    WO = o.OrderBy(r => r.ErrorTime).LastOrDefault().WO
                }).ToList();
                // List line sap xep theo thang name va count
                var lstDay = lsttrunglap.GroupBy(o => o.Line).Select(o => new { o.Key, Count = o.Count() }).ToList();
                //var lstDay = lstResult.GroupBy(o => o.Line).Select(o => new { o.Key, Count = o.Count() }).ToList();
                // Sap xep theo thu tu giam dan
                lstDay = lstDay.OrderByDescending(o => o.Count).ToList();
                List<ObjectAnalysis> lstDispDay = new List<ObjectAnalysis>();

                for (int i = 0; i < lstDay.Count; i++)
                {
                    ObjectAnalysis p = new ObjectAnalysis();
                    p.Name = lstDay[i].Key;
                    p.Quantity = lstDay[i].Count;
                    lstDispDay.Add(p);
                }
                if (lstDispDay == null) return;

                // Hiển thị kết quả
                if (level == Const.HIGH_LEVEL)
                {
                    dgvViewMonitor.DataSource = lstDispDay;
                    dgvViewMonitor.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    dgvViewMonitor.Columns["Name"].HeaderText = "Line";
                    dgvViewMonitor.Columns["Quantity"].HeaderText = "Số lần lỗi";
                }
                else if (level == Const.MID_LEVEL)
                    dgvSortMidErr.DataSource = lstDispDay;
                //KillProcess("LoadTitle");

            }
        }

        void displaySystemState()
        {
            if (lstTblError == null) return;
            var item_alarm = lstTblError.Where(o => o.isConfirm == false).ToList();
            // var item_alarm = lstTblError.FirstOrDefault(o => o.isConfirm == false);
            //if (lstTblError.Count == 0)
            if (item_alarm.Count() == 0)
            {
                cbbLangVerify.Text = "Tình trạng tốt, không có lỗi nghiêm trọng";
                cbbLangVerify.ForeColor = Color.White;
                cbbLangVerify.BackColor = Color.Blue;
                if (comControl.IsOpen)
                {
                    SendCom("#OFF*");
                }
                isAlarmline = false;
            }
            else
            {
                if (cboAreaQuery.Text != "ALL")
                {
                    cbbLangVerify.BackColor = Color.Red;
                    SystemStateTemp = cbbLangVerify.Text = "LINE ĐANG GẶP VẤN ĐỀ. XỬ LÝ LỖI, XONG THÌ ẤN XÁC NHẬN.";
                    foreach (var items in item_alarm)
                    {

                        Console.WriteLine("Line " + items.Line);
                        var isGroupId = locationList.FirstOrDefault(r => r.LineId == items.Line);
                        if (isGroupId is null)
                        {
                            if (comControl.IsOpen)
                            {
                                SendCom("#ON*");
                            }
                        }
                        else
                        {
                            var checkgroupId = locationList.Where(r => r.LineId == items.Line && r.GroupID == Class.Ultils.GetValueRegistryKey("GroupLine")).ToList();
                            if (checkgroupId.Count() > 0)
                            {
                                if (comControl.IsOpen)
                                {
                                    SendCom("#ON*");
                                }
                            }
                        }
                    }
                    isAlarmline = true;
                }
            }
            //writelog();
        }

        private void cmbAreaQuery_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControlMain.SelectedTab.Text != Const.TAB_NGHIEMTRONG) return;
            if (initialComplete == false) return;
            QueryProcess(Const.LOADPART_PROC, Const.HIGH_LEVEL);
        }

        private void cbbLangMonitor_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControlMain.SelectedTab.Text != Const.TAB_NGHIEMTRONG) return;
            if (initialComplete == false) return;
            QueryProcess(Const.LOADPART_PROC, Const.HIGH_LEVEL);
        }
        #endregion

        #region Report       

        private void GeneralReportMonitor(ZedGraphControl zg, string level, string Type)
        {
            //ALL: toàn bộ khách hàng, Day: lấy dữ liệu theo ngày
            DateTime cur = DateTime.Now;
            List<Xaxis_YLocation> lstReportLocation_Days = CalculatorChart.GetListReportLocation(cboAreaQuery.Text, Type, cur.AddDays(-6), cur.AddDays(+1), level);
            // Show
            this.Invoke((MethodInvoker)delegate
            {
                ChartProperty p = new ChartProperty();
                if (level == Const.HIGH_LEVEL)
                    p = new ChartProperty()
                    {
                        title = "BIỂU ĐỒ LỖI CÁC KHU VỰC " + cboAreaQuery.Text + " theo Ngày(" + "8"/*cur.Hour.ToString()*/+ "h " +
                    "Hôm trước tới " + "8"/*cur.Hour.ToString()*/+ "h Hôm nay)"
                    ,
                        titleX = "",
                        titleY = "SỐ LỖI NGHIÊM TRỌNG",
                        FontSize = 24,
                        selectObj = cboAreaQuery.Text
                    };
                else if (level == Const.MID_LEVEL)
                    p = new ChartProperty()
                    {
                        title = "BIỂU ĐỒ LỖI CÁC KHU VỰC " + cbbAreaMidErrMonitor.Text + " theo Ngày(" + "8"/*cur.Hour.ToString()*/+ "h " +
                    "Hôm trước tới " + "8"/*cur.Hour.ToString()*/+ "h Hôm nay)"
                    ,
                        titleX = "",
                        titleY = "SỐ LỖI KHÔNG PHẢI CẢNH BÁO",
                        FontSize = 24,
                        selectObj = cbbAreaMidErrMonitor.Text
                    };

                zg.GraphPane.CurveList.Clear();
                zg.GraphPane.GraphObjList.Clear();
                ComChart.Report_BarGraphic(zg, lstReportLocation_Days, p);
                zg.Refresh();
            });
        }

        private void GeneralReportAnalysis(ZedGraphControl zg, Xaxis_YAnalysis data, string Type, DateTime TimeStart, DateTime TimeEnd)
        {
            //ALL: toàn bộ khách hàng, Day: lấy dữ liệu theo ngày
            DateTime cur = DateTime.Now;
            //var lstReportCustomer_Days = GetListReportCustomer("ALL", "Day", cur.AddDays(-7), cur);


            this.Invoke((MethodInvoker)delegate
            {
                ChartProperty p = new ChartProperty();
                p = new ChartProperty()
                {
                    title = "BIỂU ĐỒ LỖI theo " + Type + " tính từ ngày " + TimeStart + " đến ngày " + TimeEnd,
                    titleX = "",
                    titleY = "SỐ LỖI NGHIÊM TRỌNG",
                    FontSize = 12,
                    selectObj = Type
                };

                zg.GraphPane.CurveList.Clear();
                zg.GraphPane.GraphObjList.Clear();
                ComChart.Report_BarGraphicAnalysis(zg, data, p);
                zg.Refresh();
            });
        }
        #endregion Report

        #region TimerAll
        private void QueryTimer_Tick(object sender, EventArgs e)
        {
            QueryTimeCount = 0;
            if (tabControlMain.SelectedTab.Text == Const.TAB_NGHIEMTRONG)
                QueryProcess(Const.LOADPART_PROC, Const.HIGH_LEVEL);
            QueryTimer.Interval = setting.queryTime * 1000;
            if (tabControlMain.SelectedTab.Text == Const.TAB_DULIEUDAUGIO)
                QueryTimer.Interval = setting.queryTime * 1000;
            LoadDataEarlyHour();
        }

        private void DiplayClock()
        {
            //float fCpu = pCpu.NextValue();
            //float fRam = pRam.NextValue();
            //mpgrCpu.Value = (int)fCpu;
            //mpgrRam.Value = (int)fRam;
            //lblCpu.Text = string.Format("{0:0.00}%", fCpu);
            //lblRam.Text = string.Format("{0:0.00}%", fRam);
            //chart1.Series["CPU"].Points.AddY(fCpu);
            //chart1.Series["RAM"].Points.AddY(fRam); 
            tssMove.Text = "";
            tsslbBeforeHourQuery.Text = setting.QueryBeforeHours.ToString() + "h";
            tstrClockValue.Text = DateTime.Now.ToString("HH:mm:ss  dd:MM:yyyy");
        }

        void DisplayStateBlink()
        {
            if (isAlarmline == true)
            {
                cbbLangVerify.Text = ((QueryTimeCount % 2 == 0) ? SystemStateTemp : "");
            }
        }

        private void QueryDevTimer_Tick(object sender, EventArgs e)
        {
            DiplayClock();
            QueryTimeCount++;
            st_DisQueryTime.Text = QueryTimeCount.ToString();

            if (comControl.IsOpen == false)
            {
                CheckComport();
            }
            else
            {
                devTimeout++;
                if (((devTimeout % 5) == 0) && (devTimeout > 0))
                {
                    SendCom("#GETDEV*");
                }
                if (devTimeout >= 10) isConnectDevice = false;
            }
            if (comControl.IsOpen == false) isConnectDevice = false;
            if (isConnectDevice == false)
            {
                st_lbDevice.Text = "THIẾT BỊ CẢNH BÁO ĐANG KHÔNG ĐƯỢC KẾT NỐI. KIỂM TRA LẠI";
                //grStateSystem.BackColor = Color.Red;
                st_lbDevice.ForeColor = Color.Red;
                //writelog();
            }
            else
            {
                st_lbDevice.Text = "THIẾT BỊ ĐANG KẾT NỐI VỚI PHẦN MỀM.";
                //grStateSystem.BackColor = Color.Red;
                st_lbDevice.ForeColor = Color.Blue;
                displaySystemState();
                //LoadDataEarlyHour();
            }
            //DisplayStateBlink();
        }
        #endregion

        #region Datagridview

        private void RefreshDatagridView(int proc, string level)
        {
            // hien thi ra datagridview   
            if (level == Const.HIGH_LEVEL && (tabControlMain.SelectedTab.Text == Const.TAB_NGHIEMTRONG) /*&& proc == Const.LOADPART_PROC*/)
            {
                if (lstTblError == null) return;

                var lstv = lstTblError.GroupBy(o => new { o.PartCode, o.WO, o.Model, o.Slot, o.Line }).Select(o => new PDA_ErrorHistory_Confirm()
                {
                    id = o.OrderBy(r => r.ErrorTime).LastOrDefault().id,
                    Customer = o.OrderBy(r => r.ErrorTime).LastOrDefault().Customer,
                    ErrorTime = o.OrderBy(r => r.ErrorTime).LastOrDefault().ErrorTime,
                    ErrorContent = o.OrderBy(r => r.ErrorTime).LastOrDefault().ErrorContent,
                    Line = o.OrderBy(r => r.ErrorTime).LastOrDefault().Line,
                    Location = o.OrderBy(r => r.ErrorTime).LastOrDefault().Location,
                    Model = o.OrderBy(r => r.ErrorTime).LastOrDefault().Model,
                    OperatorCode = o.OrderBy(r => r.ErrorTime).LastOrDefault().OperatorCode,
                    PartCode = o.OrderBy(r => r.ErrorTime).LastOrDefault().PartCode,
                    ProcessID = o.OrderBy(r => r.ErrorTime).LastOrDefault().ProcessID,
                    Slot = o.OrderBy(r => r.ErrorTime).LastOrDefault().Slot,
                    SystemId = o.OrderBy(r => r.ErrorTime).LastOrDefault().SystemId,
                    WO = o.OrderBy(r => r.ErrorTime).LastOrDefault().WO,
                    isConfirm = o.OrderBy(r => r.ErrorTime).LastOrDefault().isConfirm
                }).ToList();

                if (Properties.Settings.Default.ViewError == "Unvisible")
                    lstDisplayView = lstv.OrderBy(o => o.isConfirm).ThenByDescending(o => o.ErrorTime).ToList();
                else
                    lstDisplayView = lstv.OrderBy(o => o.isConfirm).ThenByDescending(o => (o.ErrorContent.Contains("UpnPartNotRequired") == true || o.ErrorContent.Contains("OldUpnNotMatch")) && o.ProcessID == 3).ToList();


                var lst = lstv.Where(o => o.isConfirm == false).ToList();
                if (lst != null)
                {
                    lbErrorAlarm.Text = lst.Count.ToString();
                    lbErrorAlarm.ForeColor = Color.Red;
                    if (lst.Count == 0)
                        lbErrorAlarm.ForeColor = Color.Blue;
                }
                dgvInformation.Rows.Clear();
                dgvInformation.DataSource = null;
                lbTotalErrorsMonitor.Text = lstDisplayView.Count.ToString();
                foreach (var item in lstDisplayView)
                {
                    var IfindProcess = Const.lstProcessPDA.FirstOrDefault(o => o.id == item.ProcessID);
                    if (IfindProcess == null) break;
                    int rowid = dgvInformation.Rows.Add();
                    DataGridViewRow row = dgvInformation.Rows[rowid];
                    row.Cells["ID"].Value = item.id;
                    row.Cells["ErrorTime"].Value = item.ErrorTime;
                    row.Cells["Line"].Value = item.Line;
                    row.Cells["Slot"].Value = item.Slot;
                    row.Cells["Model"].Value = item.Model;
                    row.Cells["WO"].Value = item.WO;
                    row.Cells["PartCode"].Value = item.PartCode;
                    row.Cells["Error"].Value = item.ErrorContent;
                    row.Cells["ProcessID"].Value = IfindProcess.name_process;
                    if (cbbLangMonitor.Text == "VN")
                        row.Cells["ProcessID"].Value = IfindProcess.name_processVN;
                    row.Cells["Operator"].Value = item.OperatorCode;
                    row.Cells["Customer"].Value = item.Customer;
                    row.Cells["Location"].Value = item.Location;

                    if ((item.ErrorContent.Contains("UpnPartNotRequired") == true || item.ErrorContent.Contains("OldUpnNotMatch")) && item.ProcessID == 3 && item.isConfirm == false)
                    {
                        row.Cells["Confirm"].Value = "Confirm";
                        row.Cells["Confirm"].Style.BackColor = Color.Red;
                        row.Cells["Confirm"].Style.ForeColor = Color.Black;
                        row.DefaultCellStyle.BackColor = Color.Red;
                        row.Cells["Error"].Style.ForeColor = Color.Black;
                    }
                    else if (item.isConfirm == false)
                    {
                        row.Cells["Confirm"].Value = "Confirm";
                        row.Cells["Confirm"].Style.BackColor = Color.Red;
                        row.Cells["Confirm"].Style.ForeColor = Color.Red;
                    }
                    else if (item.isConfirm == true)
                    {
                        row.Cells["Confirm"].Value = "Done";
                        row.Cells["Confirm"].Style.BackColor = Color.Lime;
                        row.Cells["Confirm"].Style.ForeColor = Color.Black;
                        row.DefaultCellStyle.BackColor = Color.Lime;
                        row.Cells["Error"].Style.ForeColor = Color.Black;
                    }
                }
                dgvInformation.EnableHeadersVisualStyles = false;
                dgvInformation.Refresh();
                displaySystemState();

            }
            else if (level == Const.MID_LEVEL && (tabControlMain.SelectedTab.Text == Const.TAB_NO_NGHIEMTRONG))
            {
                lstDisplayView = lstTblnonAlarmError.OrderByDescending(o => o.ErrorTime).ToList();
                if (lstTblnonAlarmError != null)
                    dgvMidErrMonitor.Rows.Clear();
                dgvMidErrMonitor.DataSource = null;
                lbTotalErrMidErr.Text = lstDisplayView.Count.ToString();
                foreach (var item in lstDisplayView)
                {
                    var IfindProcess = Const.lstProcessPDA.FirstOrDefault(o => o.id == item.ProcessID);
                    int rowid = dgvMidErrMonitor.Rows.Add();
                    DataGridViewRow row = dgvMidErrMonitor.Rows[rowid];
                    row.Cells["IDMid"].Value = item.id;
                    row.Cells["ErrorTimeMid"].Value = item.ErrorTime;
                    row.Cells["LineMid"].Value = item.Line;
                    row.Cells["SlotMid"].Value = item.Slot;
                    row.Cells["ModelMid"].Value = item.Model;
                    row.Cells["WOMid"].Value = item.WO;
                    row.Cells["PartCodeMid"].Value = item.PartCode;
                    row.Cells["ErrorContentMid"].Value = item.ErrorContent;
                    if (IfindProcess != null)
                        row.Cells["ProcessIDMid"].Value = IfindProcess.name_process;
                    else if (cbbLangMidErrMonitor.Text == "VN" && IfindProcess != null)
                        row.Cells["ProcessIDMid"].Value = IfindProcess.name_processVN;
                    else if (IfindProcess != null)
                        row.Cells["ProcessIDMid"].Value = "";
                    row.Cells["OperatorMid"].Value = item.OperatorCode;
                    row.Cells["CustomerMid"].Value = item.Customer;
                    row.Cells["LocationMid"].Value = item.Location;
                }
                dgvMidErrMonitor.EnableHeadersVisualStyles = false;
                dgvMidErrMonitor.Refresh();
            }
        }

        void DisplayRowSelect(int index, string level)
        {
            if (level == Const.HIGH_LEVEL)
            {
                lbInfoErr.Text = dgvInformation.Rows[index].Cells["Error"].Value.ToString();
                lbLineInfo.Text = dgvInformation.Rows[index].Cells["Line"].Value.ToString();
                lbOperatorInfo.Text = dgvInformation.Rows[index].Cells["Operator"].Value.ToString();
                lbWOInfo.Text = dgvInformation.Rows[index].Cells["WO"].Value.ToString();
                lbModelInfo.Text = dgvInformation.Rows[index].Cells["Model"].Value.ToString();
                lbCustomerInfo.Text = dgvInformation.Rows[index].Cells["Customer"].Value.ToString();
            }
            else if (level == Const.MID_LEVEL)
            {
                lbContentMidErr.Text = dgvMidErrMonitor.Rows[index].Cells["ErrorContentMid"].Value.ToString();
                lbLineErrMid.Text = dgvMidErrMonitor.Rows[index].Cells["LineMid"].Value.ToString();
                lbOperMidErr.Text = dgvMidErrMonitor.Rows[index].Cells["OperatorMid"].Value.ToString();
                lbWOMidErr.Text = dgvMidErrMonitor.Rows[index].Cells["WOMid"].Value.ToString();
                lbModelMidErr.Text = dgvMidErrMonitor.Rows[index].Cells["ModelMid"].Value.ToString();
                lbCusMidErr.Text = dgvMidErrMonitor.Rows[index].Cells["CustomerMid"].Value.ToString();
            }
            else if (level == Const.CLEAR_LEVEL)
            {
                lbInfoErr.Text = "";
                lbLineInfo.Text = "";
                lbOperatorInfo.Text = "";
                lbWOInfo.Text = "";
                lbModelInfo.Text = "";
                lbCustomerInfo.Text = "";
                txbComment.Text = "";
            }
        }
        void DisplayNullRowSelect()
        {
            lbInfoErr.Text = "";
            lbLineInfo.Text = "";
            lbOperatorInfo.Text = "";
            lbWOInfo.Text = "";
            lbModelInfo.Text = "";
            lbCustomerInfo.Text = "";
        }

        private void dgvInformation_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0 || e.RowIndex == -1) return;
                DisplayRowSelect(0, Const.CLEAR_LEVEL);
                DisplayRowSelect(e.RowIndex, Const.HIGH_LEVEL);
                IDErr = Convert.ToInt32(dgvInformation.Rows[e.RowIndex].Cells[0].Value);
                using (var db = new DBContext())
                {
                    var item = db.PDA_ErrorHistoryBk.FirstOrDefault(o => o.IdErr == IDErr);
                    if (item != null)
                    {
                        grCommentHigErr.Text = "Đã xác nhận";
                        grCommentHigErr.ForeColor = Color.Blue;
                        txbComment.Text = item.Comment;
                        txbComment.ForeColor = Color.Black;
                    }
                    else
                    {
                        grCommentHigErr.Text = "Chưa xác nhận";
                        grCommentHigErr.ForeColor = Color.Red;
                    }
                }
            }
            catch
            {

            }
        }

        private void dgvInformation_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                var senderGrid = (DataGridView)sender;

                if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0)
                {
                    DataGridViewRow row = dgvInformation.Rows[e.RowIndex];
                    if (row.Cells["Confirm"].Value == "Done") return;
                    //không cho phép update dữ liệu từ Quyết (dữ liệu gốc)
                    QueryTimer.Enabled = false;
                    //
                    if (lstTblError == null) return;
                    if (lstTblError.Count == 0 || IDErr <= 0 || IDErr == -1) return;
                    PDA_ErrorHistory_Confirm item = lstTblError.FirstOrDefault(o => o.id == IDErr);
                    Confirm fx = new Confirm(item);
                    fx.ShowDialog();
                    if (fx.Text != "")
                    {
                        List<PDA_ErrorHistory_Confirm> lstSame = new List<PDA_ErrorHistory_Confirm>();
                        PDA_ErrorHistory_Confirm obj = lstTblError.FirstOrDefault(o => o.id == IDErr);
                        if (obj == null) return;
                        lstSame = lstTblError.Where(o => o.Line == obj.Line && o.Slot == obj.Slot && o.Model == obj.Model && o.WO == obj.WO && o.PartCode == obj.PartCode).ToList();
                        PushDatabase(lstSame, IDErr, fx.Text);
                        RefreshDatagridView(Const.LOADPART_PROC, Const.HIGH_LEVEL);
                        DisplayNullRowSelect();
                    }
                    //cho phép update lại dữ liệu gốc
                    QueryTimer.Enabled = true;
                    QueryTimeCount = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }
        #endregion

        #region Tab Phân tích dữ liệu
        List<ObjectAnalysis> SearchAnalysisProcess(string Select)
        {
            if (initialComplete == false) return null;
            List<PDA_ErrorHistory> lstResult = new List<PDA_ErrorHistory>();

            DateTime Tstart = dtStartAnalysis.Value;
            Tstart = new DateTime(Tstart.Year, Tstart.Month, Tstart.Day, 8, 0, 0);

            DateTime Cur = DateTime.Now;
            DateTime Tend = dtEndAnalysis.Value;
            //Tend = new DateTime(Tend.Year, Tend.Month, Tend.Day, 8, 0, 0);            
            DateTime TstartDay = Tend;
            TstartDay = new DateTime(TstartDay.Year, TstartDay.Month, TstartDay.Day, 8, 0, 0);
            var sophuttrongngay = Common.Get_SoPhut_onDay(Tend);
            if (sophuttrongngay >= 960)
                TstartDay = TstartDay.AddDays(-1);
            TstartDay = new DateTime(TstartDay.Year, TstartDay.Month, TstartDay.Day, 8, 0, 0);
            DateTime TstartWeek = Tend.AddDays(-6);
            TstartWeek = new DateTime(TstartWeek.Year, TstartWeek.Month, TstartWeek.Day, 8, 0, 0);

            DateTime TstartMonth = new DateTime(Tend.Year, Tend.Month, 1, 8, 0, 0);
            TstartMonth = new DateTime(TstartMonth.Year, TstartMonth.Month, TstartMonth.Day, 8, 0, 0);

            //RunProcess("LoadTitle.exe");
            try
            {

                using (var db = new DBContext())
                {
                    var lstView = db.PDA_ErrorHistory.Where(o => o.ErrorTime >= Tstart && o.ErrorTime < Tend).ToList();

                    //loc loi nghiem trong va khach hang
                    if (lstDefineErr == null) return null;
                    if (lstDefineErr.Count > 0)
                    {
                        foreach (var p in lstView)
                        {
                            //Lọc theo khu vực
                            var IfindArea = _lstLocation.FirstOrDefault(o => o.lstline.Contains(p.Line));
                            if (IfindArea != null) p.Location = IfindArea.name;
                            //Lọc theo khách hàng
                            //CustomerObj IfindCusAll = new CustomerObj();
                            //IfindCusAll = _lstCustomer.FirstOrDefault(o => o.code.Contains(p.Customer));                            
                            //if ((IfindCusAll != null) && (cbbSelectAnalysis.Text == "ALL")) p.Customer = IfindCusAll.name;

                            // Lọc theo lỗi nghiêm trọng
                            var IfindError = lstDefineErr.FirstOrDefault(o => o.PDAErrorContent.Contains(p.ErrorContent));

                            if (IfindError != null && ((IfindArea != null && IfindArea.name == cbbSelectAnalysis.Text) || cbbSelectAnalysis.Text == "ALL"))
                            {
                                //if (IfindError != null) p.ErrorContent = IfindError.VNContent;
                                p.Customer = CustomerObj.getname(p.Customer, _lstCustomer);
                                //p.Location = LocatonObj.getname(p.Line, _lstLocation);
                                lstResult.Add(p);
                            }
                        }
                    }
                    // loc list theo ngay, theo tuan, theo thang
                    //var lstMonthCurrent = lstResult.Where(o => o.ErrorTime >= TstartMonth && o.ErrorTime < Tend).ToList();
                    var lstMonthCurrent = lstResult.Where(o => o.ErrorTime >= Tstart && o.ErrorTime < Tend).ToList();
                    if (lstMonthCurrent == null) return null;
                    // List line sap xep theo thang name va count
                    List<ObjectAnalysis> lstMonth = new List<ObjectAnalysis>();
                    if (Select == Const.OBJ_LINE)
                        lstMonth = lstMonthCurrent.GroupBy(o => o.Line).Select(o => new ObjectAnalysis() { Name = o.Key, Quantity = o.Count() }).ToList();
                    else if (Select == Const.OBJ_OPERATOR)
                        lstMonth = lstMonthCurrent.GroupBy(o => o.OperatorCode).Select(o => new ObjectAnalysis() { Name = o.Key, Quantity = o.Count() }).ToList();
                    else if (Select == Const.OBJ_MODEL)
                        lstMonth = lstMonthCurrent.GroupBy(o => o.Model).Select(o => new ObjectAnalysis() { Name = o.Key, Quantity = o.Count() }).ToList();
                    else if (Select == Const.OBJ_PARTCODE)
                        lstMonth = lstMonthCurrent.GroupBy(o => o.PartCode).Select(o => new ObjectAnalysis() { Name = o.Key, Quantity = o.Count() }).ToList();
                    else if (Select == Const.OBJ_WO)
                        lstMonth = lstMonthCurrent.GroupBy(o => o.WO).Select(o => new ObjectAnalysis() { Name = o.Key, Quantity = o.Count() }).ToList();
                    else if (Select == Const.OBJ_PROCESS)
                        lstMonth = lstMonthCurrent.GroupBy(o => o.ProcessID).Select(o => new ObjectAnalysis() { Name = o.Key == 1 ? "Load Part" : (o.Key == 2 ? "Verify Part" : (o.Key == 3 ? "Reload Part" : "")), Quantity = o.Count() }).ToList();
                    else if (Select == Const.OBJ_CUSTOMER)
                        lstMonth = lstMonthCurrent.GroupBy(o => o.Customer).Select(o => new ObjectAnalysis() { Name = o.Key.ToString(), Quantity = o.Count() }).ToList();
                    //var lstMonth = lstMonthCurrent.GroupBy(o => o.Line).Select(o => new { o.Key, Count = o.Count() }).ToList();                   
                    // Sap xep theo thu tu giam dan
                    lstMonth = lstMonth.OrderByDescending(o => o.Quantity).ToList();
                    var lstDayCurrent = lstMonthCurrent.Where(o => o.ErrorTime >= TstartDay && o.ErrorTime < Tend).ToList();
                    if (lstDayCurrent == null) return null;
                    var lstWeekCurrent = lstMonthCurrent.Where(o => o.ErrorTime >= TstartWeek && o.ErrorTime < Tend).ToList();
                    if (lstWeekCurrent == null) return null;
                    List<ObjectAnalysis> lstWeek = new List<ObjectAnalysis>();
                    if (Select == Const.OBJ_LINE)
                        lstWeek = lstWeekCurrent.GroupBy(o => o.Line).Select(o => new ObjectAnalysis() { Name = o.Key, Quantity = o.Count() }).ToList();
                    else if (Select == Const.OBJ_OPERATOR)
                        lstWeek = lstWeekCurrent.GroupBy(o => o.OperatorCode).Select(o => new ObjectAnalysis() { Name = o.Key, Quantity = o.Count() }).ToList();
                    else if (Select == Const.OBJ_MODEL)
                        lstWeek = lstWeekCurrent.GroupBy(o => o.Model).Select(o => new ObjectAnalysis() { Name = o.Key, Quantity = o.Count() }).ToList();
                    else if (Select == Const.OBJ_PARTCODE)
                        lstWeek = lstWeekCurrent.GroupBy(o => o.PartCode).Select(o => new ObjectAnalysis() { Name = o.Key, Quantity = o.Count() }).ToList();
                    else if (Select == Const.OBJ_WO)
                        lstWeek = lstWeekCurrent.GroupBy(o => o.WO).Select(o => new ObjectAnalysis() { Name = o.Key, Quantity = o.Count() }).ToList();
                    else if (Select == Const.OBJ_PROCESS)
                        lstWeek = lstWeekCurrent.GroupBy(o => o.ProcessID).Select(o => new ObjectAnalysis() { Name = o.Key == 1 ? "Load Part" : (o.Key == 2 ? "Verify Part" : (o.Key == 3 ? "Reload Part" : "")), Quantity = o.Count() }).ToList();
                    else if (Select == Const.OBJ_CUSTOMER)
                        lstWeek = lstWeekCurrent.GroupBy(o => o.Customer).Select(o => new ObjectAnalysis() { Name = o.Key.ToString(), Quantity = o.Count() }).ToList();
                    List<ObjectAnalysis> lstDay = new List<ObjectAnalysis>();
                    if (Select == Const.OBJ_LINE)
                        lstDay = lstDayCurrent.GroupBy(o => o.Line).Select(o => new ObjectAnalysis() { Name = o.Key, Quantity = o.Count() }).ToList();
                    else if (Select == Const.OBJ_OPERATOR)
                        lstDay = lstDayCurrent.GroupBy(o => o.OperatorCode).Select(o => new ObjectAnalysis() { Name = o.Key, Quantity = o.Count() }).ToList();
                    else if (Select == Const.OBJ_MODEL)
                        lstDay = lstDayCurrent.GroupBy(o => o.Model).Select(o => new ObjectAnalysis() { Name = o.Key, Quantity = o.Count() }).ToList();
                    else if (Select == Const.OBJ_PARTCODE)
                        lstDay = lstDayCurrent.GroupBy(o => o.PartCode).Select(o => new ObjectAnalysis() { Name = o.Key, Quantity = o.Count() }).ToList();
                    else if (Select == Const.OBJ_WO)
                        lstDay = lstDayCurrent.GroupBy(o => o.WO).Select(o => new ObjectAnalysis() { Name = o.Key, Quantity = o.Count() }).ToList();
                    else if (Select == Const.OBJ_PROCESS)
                        lstDay = lstDayCurrent.GroupBy(o => o.ProcessID).Select(o => new ObjectAnalysis() { Name = o.Key == 1 ? "Load Part" : (o.Key == 2 ? "Verify Part" : (o.Key == 3 ? "Reload Part" : "")), Quantity = o.Count() }).ToList();
                    else if (Select == Const.OBJ_CUSTOMER)
                        lstDay = lstDayCurrent.GroupBy(o => o.Customer).Select(o => new ObjectAnalysis() { Name = o.Key.ToString(), Quantity = o.Count() }).ToList();
                    lstWeek = lstWeek.OrderByDescending(o => o.Quantity).ToList();
                    lstDay = lstDay.OrderByDescending(o => o.Quantity).ToList();

                    List<ObjectAnalysis> lstDispMonth = new List<ObjectAnalysis>();
                    List<ObjectAnalysis> lstDispWeek = new List<ObjectAnalysis>();
                    List<ObjectAnalysis> lstDispDay = new List<ObjectAnalysis>();

                    lbDailyAnalysis.Text = "Thống kê lỗi " /*+ Select*/ + " trong Ngày";
                    lbWeekAnalysis.Text = "Thống kê lỗi " /*+ Select*/ + " theo Tuần";
                    lbMonthAnalysis.Text = "Thống kê lỗi " /*+ Select */+ " theo khoảng thời gian chọn";
                    for (int i = 0; i < lstMonth.Count; i++)
                    {
                        ObjectAnalysis p = new ObjectAnalysis();
                        p.Name = lstMonth[i].Name;
                        p.Quantity = lstMonth[i].Quantity;
                        lstDispMonth.Add(p);
                    }
                    for (int i = 0; i < lstWeek.Count; i++)
                    {
                        ObjectAnalysis p = new ObjectAnalysis();
                        p.Name = lstWeek[i].Name;
                        p.Quantity = lstWeek[i].Quantity;
                        lstDispWeek.Add(p);
                    }
                    for (int i = 0; i < lstDay.Count; i++)
                    {
                        ObjectAnalysis p = new ObjectAnalysis();
                        p.Name = lstDay[i].Name;
                        p.Quantity = lstDay[i].Quantity;
                        lstDispDay.Add(p);
                    }
                    dgvDayViewAnalysis.DataSource = lstDispDay;
                    dgvWeekViewAnalysis.DataSource = lstDispWeek;
                    dgvMonthViewAnalysis.DataSource = lstDispMonth;

                    dgvDayViewAnalysis.Columns["Name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                    dgvDayViewAnalysis.Columns["Name"].Width = 180;
                    dgvDayViewAnalysis.Columns["Quantity"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                    dgvDayViewAnalysis.Columns["Quantity"].Width = 80;

                    dgvWeekViewAnalysis.Columns["Name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                    dgvWeekViewAnalysis.Columns["Name"].Width = 180;
                    dgvWeekViewAnalysis.Columns["Quantity"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                    dgvWeekViewAnalysis.Columns["Quantity"].Width = 80;

                    dgvMonthViewAnalysis.Columns["Name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                    dgvMonthViewAnalysis.Columns["Name"].Width = 180;
                    dgvMonthViewAnalysis.Columns["Quantity"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                    dgvMonthViewAnalysis.Columns["Quantity"].Width = 80;

                    KillProcess("LoadTitle");
                    DislayConnectServer();
                    return lstDispMonth;
                }
            }
            catch
            {
                DisplayError(7);
                KillProcess("LoadTitle");
                return null;
            }
        }

        // Hàm này lấy dữ liệu cần phân tích và vẽ đồ thị
        private void getdataAnalysis()
        {
            // trả lai dư liệu vẽ đồ thị
            if (dtStartAnalysis.Value > dtEndAnalysis.Value)
            {
                MessageBox.Show("Thời gian chọn để phân tích đang không hợp lý. Thời điểm bắt đầu phải trước thời điểm kết thúc");
                return;
            }
            List<ObjectAnalysis> lstResult = SearchAnalysisProcess(cbbObjectAnalysis.Text);
            if (lstResult == null || lstResult.Count == 0) return;
            string[] label = new string[lstResult.Count];
            double[] y = new double[lstResult.Count];
            for (int i = 0; i < lstResult.Count; i++)
            {
                label[i] = lstResult[i].Name;
                y[i] = lstResult[i].Quantity;
            }
            Xaxis_YAnalysis p = new Xaxis_YAnalysis() { X_axis = label, Yaxis = y };
            txbMaxErrAnalysis.Text = label[0];
            // vẽ đồ thị thông kê theo tháng
            GeneralReportAnalysis(ZgDrawAnalysis, p, cbbObjectAnalysis.Text, dtStartAnalysis.Value, dtEndSearch.Value);
        }

        private void cmbSelectAnalysis_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (initialComplete == false) return;
            getdataAnalysis();
        }

        private void cbbSelectAnalysis_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (initialComplete == false) return;
            getdataAnalysis();
        }


        private void Tab1_btnShow_Click(object sender, EventArgs e)
        {
            if (initialComplete == false) return;
            getdataAnalysis();
        }

        private void dtStartAnalysis_ValueChanged(object sender, EventArgs e)
        {
            if (initialComplete == false) return;
            getdataAnalysis();
        }

        private void dtEndAnalysis_ValueChanged(object sender, EventArgs e)
        {
            if (initialComplete == false) return;
            getdataAnalysis();
        }

        private void tabControlMain_Selected(object sender, TabControlEventArgs e)
        {
            if (initialComplete == false) return;
            if (e.TabPage.Text == Const.TAB_NO_NGHIEMTRONG)
            {
                cbbAreaMidErrMonitor.Text = Properties.Settings.Default.Area;
                cbbAreaMidErrMonitor_SelectedIndexChanged(null, null);
            }
            else if (e.TabPage.Text == Const.TAB_NGHIEMTRONG)
            {
                cboAreaQuery.Text = Properties.Settings.Default.Area;
                QueryProcess(Const.LOADPART_PROC, Const.HIGH_LEVEL);
            }
            else if (e.TabPage.Text == Const.TAB_DULIEUDAUGIO)
            {
                cboAreaDataEarlyHour.Text = Properties.Settings.Default.Area;
                cbbAreaMidErrMonitor_SelectedIndexChanged(null, null);
                LoadDataEarlyHour();
            }
            else if (e.TabPage.Text == Const.TAB_CANHBAOTHAYNOI)
            {
                cbbAreaReplace.Text = Properties.Settings.Default.Area;
                btnFilterArea_Click(null, null);
            }
        }

        #endregion

        #region Giá trị mặc định của hệ thống
        public void SaveDefaultError()
        {
            string Path = Const.pathError + "\\Error.csv";
            List<ErrorInfo> lstfirst = new List<ErrorInfo>() {
                new ErrorInfo { ErrorCODE = 1,ErrorContent = "PartBarcodeNotFound", ErrorContentVN = "Mã linh kiện không tồn tại", Alarm = 1},
                new ErrorInfo { ErrorCODE = 2,ErrorContent = "UpnNotMatch", ErrorContentVN = "Nhầm linh kiện", Alarm = 1},
                new ErrorInfo { ErrorCODE = 3,ErrorContent = "UpnPartNotRequired", ErrorContentVN = "Nhầm linh kiện", Alarm = 1},
                new ErrorInfo { ErrorCODE = 4,ErrorContent = "InvalidBarcode", ErrorContentVN = "Mã linh kiện không tồn tại", Alarm = 1}
            };
            ErrorInfo.WriteCSV(Path, lstfirst);
        }

        public void SaveDefaultCustomer()
        {
            string Path = Const.pathCustomer + "\\Customer.csv";
            List<CustomerObj> lstfirst = new List<CustomerObj>() {
                new CustomerObj { name = "ALL", code =  new List<string> { "ALL" } },
                new CustomerObj { name = "BIVN", code =  new List<string> { "CS001"} },
                new CustomerObj { name = "TOYODENSO", code =  new List<string> {"CS002" } },
                new CustomerObj { name = "CVN", code = new List<string> {"CS005","CS006", "CS014","CS021","CS025","P12", "P13" } },
                new CustomerObj { code = new List<string> {"CS013", "CS020" } , name = "YOKOWO" },
                new CustomerObj { code = new List<string> {"CS016","CS032","P41" }, name = "KYOCERA" },
                new CustomerObj { code = new List<string> {"CS018" },name = "NIHON" },
                new CustomerObj { code = new List<string> {"CS022", "P28" },name = "FUJI" },
                new CustomerObj { code = new List<string> {"CS023", "P35" },name = "HLV" },
                new CustomerObj { code = new List<string> {"CS030", "P55" },name = "NICHICON HK" },
                new CustomerObj { code = new List<string> {"CS038", "CS048","CS049","P24" },name = "VALEO" },
                 new CustomerObj { name = "STANLE ELECTRONICS", code =  new List<string> {"CS034" } },
                new CustomerObj { code = new List<string> {"CS040", "P48", "CS029"},name = "NICHICON JP" },
                new CustomerObj { code = new List<string> {"CS045", "P02" },name = "YASUKAWA" },
                new CustomerObj { code = new List<string> {"CS048", "P04"},name = "ICHIKOH" },
                new CustomerObj { code = new List<string> { "P23"}, name = "STANLEY" },
                new CustomerObj { code = new List<string> { "P51"}, name = "TOSOK" },
                new CustomerObj { code = new List<string> { "P85"}, name = "TYD" },
                new CustomerObj { code = new List<string> { "P05"}, name = "FormLabs" }
            };
            CustomerObj.WriteCSV(Path, lstfirst);
        }

        public void SaveDefaultLocation()
        {
            string Path = Const.pathLocation + "\\Location.csv";
            List<LocatonObj> lstfirst = new List<LocatonObj>() {
                new LocatonObj { name = "ALL", lstline = new List<string> { "AllLine"} },
                new LocatonObj { name = "CANON",lstline = new List<string> { "S01","S02","S03","S04","S05","S06","S07","S09","S12","S33","S34","S38","S39","S40","S46","S48","S49","S50","A01","A02","A03","A04","A101","A102","A103","A104", "JV03"} },
                new LocatonObj { name = "AUTO1",lstline = new List<string> { "S10", "S32","S37","S42","S43","S45","S47"} },
                new LocatonObj { name = "AUTO2",lstline = new List<string> { "S08", "S11"} },
                new LocatonObj { name = "BROTHER",lstline = new List<string> { "S13", "S14", "S15", "S17", "S18", "S19","S20","S21","S22","S23","S24","S25","S26","S27","S28","S29","S30","S31","S35","S36","S44","A05","A06","A07","A08","A09","A202","A203","A204","JV05","JV09"} },
                new LocatonObj { name = "YASKAWA",lstline = new List<string> { "S41", "S16"} }
            };
            LocatonObj.WriteCSV(Path, lstfirst);
        }
        #endregion

        #region Menu
        private void confirmAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Common.CheckPassword("lca@123") == true)
            {
                if (lstTblError == null) return;
                if (lstTblError.Count == 0) return;
                QueryTimer.Enabled = false;
                DialogResult dialogResult = MessageBox.Show("Bạn thực sự muốn Xác nhận hết lỗi của khu vực?", "Warring!!", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (dialogResult != DialogResult.OK)
                {
                    return;
                }
                try
                {
                    //RunProcess("LoadTitle.exe");
                    var curDate = DateTime.Now;
                    DateTime beforeTime = new DateTime();
                    //Tính toán dữ liệu 
                    if (setting.QueryBeforeHours > 0)
                        beforeTime = curDate.AddHours((-1) * setting.QueryBeforeHours);
                    else
                        beforeTime = DateTime.Now;
                    List<PDA_ErrorHistoryBk> lstErroBackupCurrent = new List<PDA_ErrorHistoryBk>();
                    List<PDA_ErrorHistoryBk> lstNewBK = new List<PDA_ErrorHistoryBk>();
                    List<PDA_ErrorHistory> lstFilterLocation = new List<PDA_ErrorHistory>();

                    using (var db = new DBContext())
                    {
                        lstErroBackupCurrent = db.PDA_ErrorHistoryBk.Where(o => o.ErrorTime >= beforeTime && o.ErrorTime <= curDate).ToList();
                        // xu ly list
                        foreach (var item in lstTblError)
                        {
                            PDA_ErrorHistoryBk itemBackUp = new PDA_ErrorHistoryBk();
                            if (lstErroBackupCurrent != null)
                                itemBackUp = lstErroBackupCurrent.FirstOrDefault(o => o.IdErr == item.id);
                            if (itemBackUp == null || lstErroBackupCurrent == null)
                            {
                                var newBU = new PDA_ErrorHistoryBk
                                {
                                    ErrorTime = item.ErrorTime,
                                    Line = item.Line,
                                    Model = item.Model,
                                    WO = item.WO,
                                    PartCode = item.PartCode,
                                    ErrorContent = item.ErrorContent,
                                    OperatorCode = item.OperatorCode,
                                    Customer = item.Customer,
                                    Location = item.Location,
                                    Comment = "Xác nhận All",
                                    IdErr = item.id,
                                    ProcessID = item.ProcessID,
                                    Slot = item.Slot
                                };
                                lstNewBK.Add(newBU);
                            }
                        }

                        //luu vao bang bakup               
                        db.PDA_ErrorHistoryBk.AddRange(lstNewBK);
                        db.SaveChanges();
                        lstTblError.Clear();
                        tssMove.Text = "OK";
                    }
                    // Cap nhat vao dgv
                    QueryTimer.Enabled = true;
                    QueryTimeCount = 0;
                    RefreshDatagridView(Const.LOADPART_PROC, Const.HIGH_LEVEL);
                    DisplayNullRowSelect();
                    displaySystemState();
                    //LoadDataEarlyHour();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }
        // Hàm này được gọi khi Form Setting bị đóng lại
        public void SetDeviceSetting()
        {
            CheckComport();
        }

        private void setupSystemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //login f = new login(_lstLocation, SetDeviceSetting);
            //f.Show();
            Setting f = new Setting(_lstLocation, SetDeviceSetting);
            f.Show();
        }

        #endregion

        #region Chạy ap load form báo dữ liệu đang tải từ Server
        public static void RunProcess(string runFileName)
        {
            string ExePath = Path.Combine(Application.StartupPath, runFileName);
            //System.Diagnostics.Process[] pros = System.Diagnostics.Process.GetProcessesByName((runFileName));
            //if (pros != null) return;
            //bool istate = true;
            //foreach (var item in pros) {
            //    istate = item.HasExited;
            //    if (istate == true) return;
            //}
            System.Diagnostics.Process.Start(ExePath);
        }

        /* Tắt 1 chương trình đang chạy*/
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
        #endregion

        #region Tab Tìm kiếm dữ liệu

        void SearchProcess()
        {
            //using (var db = new DBContext())
            //{
            //    var lstView = db.PDA_ErrorHistory.Where(o => (o.Line.Contains(txtItemSearch.Text) ||
            //                                                 o.WO.Contains(txtItemSearch.Text) ||
            //                                                 o.PartCode.Contains(txtItemSearch.Text) ||
            //                                                 o.OperatorCode.Contains(txtItemSearch.Text) ||
            //                                                 o.Model.Contains(txtItemSearch.Text))).ToList();

            //    dgvView.DataSource = lstView;
            //}


            Tbl_PDADefineError ItemErrConvert = new Tbl_PDADefineError();
            string FilterErr = "";
            if (initialComplete == false) return;
            List<PDA_ErrorHistoryEdit> lstResult = new List<PDA_ErrorHistoryEdit>();
            DateTime Tend = new DateTime();
            DateTime CurTime = DateTime.Now;
            DateTime Tstart = new DateTime();
            Tend = dtEndSearch.Value;
            if (Tend.Date != CurTime.Date)
                Tend = new DateTime(Tend.Year, Tend.Month, Tend.Day, 8, 0, 0);
            Tstart = dtStartSearch.Value;
            Tstart = new DateTime(Tstart.Year, Tstart.Month, Tstart.Day, 8, 0, 0);
            if (Tstart > Tend)
            {
                MessageBox.Show("Phải chọn thời gian bắt đầu xảy ra trước thời gian kết thúc");
                return;
            }
            if (Tstart == Tend && Tend.Date != CurTime.Date)
            { //nếu thời gian mà bằng nhau thì chọn dữ liệu ngày cần hiển thị                   
                Tstart = new DateTime(Tend.Year, Tend.Month, Tend.Day, 8, 0, 0);
                Tend = Tend.AddDays(+1);
            }
            if (lstDefineErr == null || lstDefineErr.Count == 0) return;
            if (lstDefineErr.Count > 0)
            {
                ItemErrConvert = lstDefineErr.FirstOrDefault(o => o.PDAErrorContent.Contains(txtItemSearch.Text) || o.VNContent.Contains(txtItemSearch.Text));
                if (ItemErrConvert != null) FilterErr = ItemErrConvert.PDAErrorContent;
                else FilterErr = txtItemSearch.Text;
            }
            //RunProcess("LoadTitle.exe");
            try
            {
                using (var db = new DBContext())
                {
                    var lstView = db.PDA_ErrorHistory.Where(o => (o.Line.Contains(txtItemSearch.Text) ||
                                                                 o.WO.Contains(txtItemSearch.Text) ||
                                                                 o.PartCode.Contains(txtItemSearch.Text) ||
                                                                 o.ErrorContent.Contains(FilterErr) ||
                                                                 o.OperatorCode.Contains(txtItemSearch.Text) ||
                                                                 o.Model.Contains(txtItemSearch.Text)) &&
                                                                 (o.ErrorTime >= Tstart) && (o.ErrorTime < Tend)).ToList();

                    //loc loi nghiem trong va khach hang
                    if (lstDefineErr == null || lstDefineErr.Count == 0) return;
                    if (lstDefineErr.Count > 0)
                    {
                        foreach (var p in lstView)
                        {
                            try
                            {
                                string name_cus = "";
                                //Lọc theo công đoạn                            
                                var ifindProcess = Const.lstProcessPDA.FirstOrDefault(o => o.id == p.ProcessID);
                                //Lọc theo khu vực
                                var IfindArea = _lstLocation.FirstOrDefault(o => o.lstline.Contains(p.Line));
                                if (IfindArea != null) p.Location = IfindArea.name;
                                //Lọc theo khách hàng
                                CustomerObj IfindCus = _lstCustomer.FirstOrDefault(o => o.code.Contains(p.Customer));
                                if (IfindCus != null)
                                {
                                    name_cus = p.Customer = IfindCus.name;
                                }
                                else name_cus = p.Customer;// truong hop Ifindcus = null
                                                           //else break;
                                                           //Lọc theo lỗi
                                var IfindError = lstDefineErr.FirstOrDefault(o => o.PDAErrorContent.Contains(p.ErrorContent));
                                // Lọc theo lỗi theo khách hàng                            
                                if ((name_cus == cboCusSearch.Text || cboCusSearch.Text == "ALL")
                                    && ((ifindProcess != null && ifindProcess.name_process == cbbProcessSearch.Text) || cbbProcessSearch.Text == "ALL"))
                                {
                                    if ((IfindError != null) && cbbLanguageAnalysis.Text != "EN")
                                        p.ErrorContent = IfindError.VNContent;
                                    PDA_ErrorHistoryEdit item = new PDA_ErrorHistoryEdit();
                                    item.id = p.id; item.Line = p.Line; item.Location = p.Location; item.ErrorTime = p.ErrorTime; item.Customer = p.Customer;
                                    item.ErrorContent = p.ErrorContent; item.Model = p.Model; item.WO = p.WO; item.SystemId = p.SystemId;
                                    item.Slot = p.Slot; item.Location = p.Location; item.PartCode = p.PartCode; item.OperatorCode = p.OperatorCode;
                                    item.ProcessID = p.ProcessID.ToString();
                                    if (ifindProcess != null) item.ProcessID = ifindProcess.name_process;
                                    lstResult.Add(item);
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.ToString());
                            }
                        }
                    }

                    // Hiển thị kết quả
                    if (lstResult == null || lstResult.Count == 0)
                    {
                        dgvView.DataSource = null;
                        dgvView.Refresh();
                        txbTotalErrorSearch.Text = "0";
                        KillProcess("LoadTitle");
                        return;
                    }
                    dgvView.DataSource = lstResult.OrderByDescending(o => o.ErrorTime).ToList();
                    txbTotalErrorSearch.Text = lstResult.Count.ToString();
                    dgvView.AutoResizeColumns();
                    dgvView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                    dgvView.Columns["SystemId"].Visible = false;
                    dgvView.Columns["Line"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                    dgvView.Columns["Line"].Width = 50;
                    dgvView.Columns["Slot"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                    dgvView.Columns["Slot"].Width = 50;
                    dgvView.Columns["Model"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                    dgvView.Columns["Model"].Width = 120;
                    dgvView.Columns["ErrorContent"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                    dgvView.Columns["ErrorContent"].Width = 180;
                    dgvView.Columns["PartCode"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                    dgvView.Columns["PartCode"].Width = 120;
                    dgvView.Columns["OperatorCode"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                    dgvView.Columns["OperatorCode"].Width = 150;
                    dgvView.Columns["Customer"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                    dgvView.Columns["Customer"].Width = 110;
                    KillProcess("LoadTitle");
                    DislayConnectServer();
                }
            }
            catch
            {
                DisplayError(6);
                KillProcess("LoadTitle");
            }

        }

        private void txtItemSearch_KeyUp(object sender, KeyEventArgs e)
        {
            if (initialComplete == false) return;
            if (e.KeyCode == Keys.Enter)
                SearchProcess();
        }

        private void cboCusSearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (initialComplete == false) return;
            SearchProcess();
        }

        private void cboTypeErrSearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (initialComplete == false) return;
            SearchProcess();
        }

        private void dtStartTimeQuery_ValueChanged(object sender, EventArgs e)
        {
            if (initialComplete == false) return;
            SearchProcess();
        }

        private void cbbProcessSearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (initialComplete == false) return;
            SearchProcess();
        }

        DataGridView dgvCurrentSelect = new DataGridView();
        private void exportToExcelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Common.ExportExcelFromDatagridview(dgvCurrentSelect);
        }

        private void dgvView_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                dgvCurrentSelect = (DataGridView)sender;
                MenuStripMain.Show(dgvCurrentSelect, new Point(e.X, e.Y));
            }
        }

        private void dgvInformation_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                dgvCurrentSelect = (DataGridView)sender;
                MenuStripMain.Show(dgvCurrentSelect, new Point(e.X, e.Y));
            }
        }

        private void cbbLanguageAnalysis_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (initialComplete == false) return;
            SearchProcess();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (initialComplete == false) return;
            SearchProcess();
        }

        private void txbTimeErrAnalysis_KeyUp(object sender, KeyEventArgs e)
        {
            if (initialComplete == false) return;
            if (e.KeyCode == Keys.Enter)
                SearchProcess();
        }

        //private void dbBackupToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    if (initialComplete == false) return;
        //    SearchProcessBackup();
        //}
        // Để hiển thị các lỗi nghiêm trọng sẽ bôi đỏ còn không nghiêm trọng sẽ màu đen nhưng như thế truy vấn sẽ chậm
        private void dgvView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            var row = dgvView.Rows[e.RowIndex];
            var item = lstDefineErr.FirstOrDefault(o => o.PDAErrorContent.Contains(row.Cells["ErrorContent"].Value.ToString())
                                                     || o.VNContent.Contains(row.Cells["ErrorContent"].Value.ToString()));
            if (item == null) return;
            //row.DefaultCellStyle.ForeColor = Color.Red;
            row.Cells["ErrorContent"].Style.ForeColor = Color.Red;
        }

        private void dtStartSearch_ValueChanged(object sender, EventArgs e)
        {
            SearchProcess();
        }

        #region định nghĩa khu vực mặc định
        private void bROTHERToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Area = bROTHERToolStripMenuItem.Text;
            Properties.Settings.Default.Save();
            cboAreaQuery.SelectedItem = Properties.Settings.Default.Area;
        }

        private void cANONToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Area = cANONToolStripMenuItem.Text;
            Properties.Settings.Default.Save();
            cboAreaQuery.SelectedItem = Properties.Settings.Default.Area;
        }

        private void aUTOToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Area = aUTOToolStripMenuItem.Text;
            Properties.Settings.Default.Save();
            cboAreaQuery.SelectedItem = Properties.Settings.Default.Area;
        }

        private void yASKAWAToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Area = yASKAWAToolStripMenuItem.Text;
            Properties.Settings.Default.Save();
            cboAreaQuery.SelectedItem = Properties.Settings.Default.Area;
        }

        private void tOÀNBỘToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Area = tOÀNBỘToolStripMenuItem.Text;
            Properties.Settings.Default.Save();
            cboAreaQuery.SelectedItem = Properties.Settings.Default.Area;
        }

        private void aUTO2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Area = aUTO2ToolStripMenuItem.Text;
            Properties.Settings.Default.Save();
            cboAreaQuery.SelectedItem = Properties.Settings.Default.Area;
        }

        private void cOMNoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var COMNo = Common.ShowMessage("Nhập Cổng COM (COM3: nhập giá trị số 3)", "Nhập mã COM");
            try
            {
                Properties.Settings.Default.ComNo = Convert.ToInt16(COMNo);
                Properties.Settings.Default.Save();
            }
            catch
            {
                MessageBox.Show("Bạn phải nhập số");
            }
        }

        private void cbbAreaMidErrMonitor_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControlMain.SelectedTab.Text != Const.TAB_NO_NGHIEMTRONG) return;
            if (initialComplete == false) return;
            QueryProcess(Const.NON_ERR_PROC, Const.MID_LEVEL);
        }

        private void dgvMidErrMonitor_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (initialComplete == false) return;
            if (e.RowIndex < 0 || e.RowIndex == -1) return;
            DisplayRowSelect(e.RowIndex, Const.MID_LEVEL);
        }

        private void khuToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void dgvView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                Int32 id;
                if (e.RowIndex < 0 || e.RowIndex == -1) return;
                id = Convert.ToInt32(dgvView.Rows[e.RowIndex].Cells[0].Value);
                if (id <= 0) return;
                using (var db = new DBContext())
                {
                    var item = db.PDA_ErrorHistoryBk.FirstOrDefault(o => o.IdErr == id);
                    var IfindError = lstDefineErr.FirstOrDefault(o => o.PDAErrorContent.Contains(item.ErrorContent) || o.VNContent.Contains(item.ErrorContent));
                    if (item != null && IfindError != null)
                    {
                        MessageBox.Show(item.Comment, "Nội dung comment", MessageBoxButtons.OKCancel);
                    }
                    else if (item == null && IfindError != null)
                        MessageBox.Show("Bản tin chưa được xác nhận xử lý", "Nội dung comment", MessageBoxButtons.OKCancel);
                }
            }
            catch
            {

            }
        }

        private void timerTest_Tick(object sender, EventArgs e)
        {

        }

        private void visibleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Verify = visibleToolStripMenuItem.Text;
            Properties.Settings.Default.Save();
            QueryProcess(Const.LOADPART_PROC, Const.HIGH_LEVEL);
        }

        private void unvisibleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Verify = unvisibleToolStripMenuItem.Text;
            Properties.Settings.Default.Save();
            QueryProcess(Const.LOADPART_PROC, Const.HIGH_LEVEL);
        }

        private void visibleToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.ViewError = visibleToolStripMenuItem1.Text;
            Properties.Settings.Default.Save();
        }

        private void unvisibleToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.ViewError = unvisibleToolStripMenuItem1.Text;
            Properties.Settings.Default.Save();
        }

        private void groupLineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new fGroupLine().ShowDialog();
        }

        private void cbbLangMidErrMonitor_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControlMain.SelectedTab.Text != Const.TAB_NO_NGHIEMTRONG) return;
            if (initialComplete == false) return;
            QueryProcess(Const.NON_ERR_PROC, Const.MID_LEVEL);
        }

        private void cboAreaDataEarlyHour_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControlMain.SelectedTab.Text != Const.TAB_DULIEUDAUGIO) return;
            if (initialComplete == false) return;
            LoadDataEarlyHour();
        }

        private void dgvMidErrView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0 || e.RowIndex == -1) return;
                DisplayRowSelect(e.RowIndex, Const.MID_LEVEL);
            }
            catch
            {

            }
        }

        #endregion

        #endregion
        #region Tab Dữ liệu đầu giờ
        DataTable table;
        void LoadDataEarlyHour()
        {
            using (var dxcontext = new DXContext())
            {
                List<LoadedOrderItem> loadedOrderItems = new List<LoadedOrderItem>();

                var result = dxcontext.LoadedOrderItems.Where(c => c.IS_VERIFIED == false).ToList();
                if (cboAreaDataEarlyHour.Text == "ALL")
                {
                    lblCount.Text = result.Count().ToString();
                    dtgv_view.DataSource = SetDataTable(result);
                }
                else
                {
                    var listLinebyCustomer = dxcontext.Locations.Where(x => x.LocationId == cboAreaDataEarlyHour.Text).ToList();
                    foreach (var item in listLinebyCustomer)
                    {
                        var resultcheckIsLine = result.Where(d => d.LINE_ID == item.LineId).ToList();

                        foreach (var items in resultcheckIsLine)
                        {
                            var list = new LoadedOrderItem()
                            {
                                LINE_ID = items.LINE_ID,
                                PRODUCTION_ORDER_ID = items.PRODUCTION_ORDER_ID,
                                PART_ID = items.PART_ID,
                                MACHINE_ID = items.MACHINE_ID,
                                MACHINE_SLOT = items.MACHINE_SLOT,
                                UPD_TIME = items.UPD_TIME,
                                PRODUCT_ID = items.PRODUCT_ID,
                                IS_VERIFIED = items.IS_VERIFIED
                            };
                            loadedOrderItems.Add(list);
                        }
                    }
                    lblCount.Text = loadedOrderItems.Count().ToString();
                    dtgv_view.DataSource = SetDataTable(loadedOrderItems);
                }
            }
        }

        private void dgrvListReplacError_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
        }

        DataTable SetDataTable(List<LoadedOrderItem> loadedOrderItems)
        {
            table = new DataTable();
            table.Columns.Add("Line");
            table.Columns.Add("Product Order");
            table.Columns.Add("Part No");
            table.Columns.Add("Machine Id");
            table.Columns.Add("Machine Slot");
            table.Columns.Add("Product ID");
            foreach (var item in loadedOrderItems)
            {
                table.Rows.Add(item.LINE_ID, item.PRODUCTION_ORDER_ID, item.PART_ID, item.MACHINE_ID, item.MACHINE_SLOT, item.PRODUCT_ID);
            }
            return table;
        }
        #endregion

        #region Tab Cảnh báo thay nối

        private void RequestCompleted(DataTable dt)
        {
            pbLoading.Visible = false;
            dgrvListReplacError.AutoGenerateColumns = true;
            dgrvListReplacError.DataSource = dt;
            dgrvListReplacError.Columns["ConfirmReplace"].DisplayIndex = dgrvListReplacError.ColumnCount - 1;
            dgrvListReplacError.AutoGenerateColumns = false;
            if (string.IsNullOrEmpty(cbbAreaReplace.Text) || cbbAreaReplace.Text == "ALL")
            {
                (dgrvListReplacError.DataSource as DataTable).DefaultView.RowFilter = "";
            }
            else
            {
                var listLine = _lstLocation.FirstOrDefault(o => o.name == cbbAreaReplace.Text);
                var sql = new List<string>();
                foreach (var line in listLine.lstline)
                {
                    sql.Add($"'{line}'");
                }
                (dgrvListReplacError.DataSource as DataTable).DefaultView.RowFilter = $"[Line] IN ({string.Join(",", sql)})";
            }
            lblTotalErrorReplace.Text = dgrvListReplacError.Rows.GetRowCount(DataGridViewElementStates.Visible).ToString();
        }

        private void cbbAreaReplace_SelectedValueChanged(object sender, EventArgs e)
        {
            _areaReplace = cbbAreaReplace.Text;
        }

        private void btnFilterArea_Click(object sender, EventArgs e)
        {
            pbLoading.Visible = true;
            BackgroundLoading BL = new BackgroundLoading(GetData, RequestWOCompleted);
            BL.Start(false);
            DrawGraph();
        }
        private void DrawGraph()
        {
            zedGraphControl1.GraphPane.CurveList.Clear();
            var firstDate = DateTime.Now.AddDays(-6);
            var lastDate = DateTime.Now;
            List<WODetailInfo> list = SMTHelper.GetUPNNotReplace(GetListLine());
            List<WODetailInfo> listWrong = SMTHelper.GetUPNWrong(GetListLine());
            zedGraphControl1.IsShowPointValues = true;
            string[] xs = new string[7];
            double[] ys1 = new double[7];
            double[] ys2 = new double[7];
            var index = 0;
            for (DateTime i = firstDate; i <= lastDate; i = i.AddDays(1))
            {
                xs[index] = i.ToString("dd");
                var error = list.Where(m => m.Upd_Time.ToString("yyyy-MM-dd") == i.ToString("yyyy-MM-dd")).ToList();
                if (error == null || error.Count == 0) ys1[index] = 0;
                else ys1[index] = error.Select(m => m.UPN).Distinct().Count();

                var error1 = listWrong.Where(m => m.Upd_Time.ToString("yyyy-MM-dd") == i.ToString("yyyy-MM-dd")).ToList();
                if (error1 == null || error1.Count == 0) ys2[index] = 0;
                else ys2[index] = error1.Select(m => m.UPN).Distinct().Count();

                index++;
            }

            // style the plot
            zedGraphControl1.GraphPane.Title.Text = $"BIỂU ĐỒ LỖI CÁC KHU VỰC " + cbbAreaReplace.Text;
            zedGraphControl1.GraphPane.XAxis.Title.Text = "7 ngày gần nhất";
            zedGraphControl1.GraphPane.YAxis.Title.Text = "Tổng số cuộn không thay nối";

            //generate pane
            var pane = zedGraphControl1.GraphPane;

            pane.XAxis.Scale.IsVisible = true;
            pane.YAxis.Scale.IsVisible = true;

            pane.XAxis.MajorGrid.IsVisible = true;
            pane.YAxis.MajorGrid.IsVisible = true;

            pane.XAxis.Scale.TextLabels = xs;
            pane.XAxis.Type = AxisType.Text;

            //var pointsCurve;

            LineItem pointsCurve = pane.AddCurve("Không thay nối", null, ys1, Color.Black);
            pointsCurve.Line.IsVisible = true;
            pointsCurve.Line.Width = 3.0F;
            //Create your own scale of colors.

            pointsCurve.Symbol.Fill = new Fill(new Color[] { Color.Blue, Color.Green, Color.Red });
            pointsCurve.Symbol.Fill.Type = FillType.Solid;
            pointsCurve.Symbol.Type = SymbolType.Circle;
            pointsCurve.Symbol.Border.IsVisible = true;

            LineItem pointsCurve1 = pane.AddCurve("Sử dụng sai linh kiện", null, ys2, Color.Blue);
            pointsCurve1.Line.IsVisible = true;
            pointsCurve1.Line.Width = 3.0F;
            //Create your own scale of colors.

            pointsCurve1.Symbol.Fill = new Fill(new Color[] { Color.Blue, Color.Green, Color.Red });
            pointsCurve1.Symbol.Fill.Type = FillType.Solid;
            pointsCurve1.Symbol.Type = SymbolType.Circle;
            pointsCurve1.Symbol.Border.IsVisible = true;

            pane.AxisChange();
            zedGraphControl1.Refresh();
        }
        private void btnFilterWO_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txbWO.Text.Trim()))
                {
                    (dgrvWOVerify.DataSource as DataTable).DefaultView.RowFilter = "";
                }
                else
                {
                    (dgrvWOVerify.DataSource as DataTable).DefaultView.RowFilter = $"[WO] = '{txbWO.Text.Trim()}'";
                }
                AddButtonStartVerify();
                lblTotalErrorReplace.Text = dgrvListReplacError.Rows.GetRowCount(DataGridViewElementStates.Visible).ToString();
            }
            catch (Exception ex)
            {

            }
        }

        private string _areaReplace = "";

        private DataTable GetData()
        {
            IWOVerify woVerify = new WOVerify2();
            return woVerify.GetWONeedVerify(GetListLine());
        }
        private void RequestWOCompleted(DataTable dt)
        {
            pbLoading.Visible = false;
            dgrvWOVerify.DataSource = dt;
            AddButtonStartVerify();
            dgrvLineReplaceError.DataSource = SMTHelper.GetLineReplaceError(GetListLine());
        }


        private void dgrvWOVerify_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dgrvListReplacError.DataSource = null;
            int hangchon = dgrvWOVerify.CurrentRow.Index;
            string WO = dgrvWOVerify["WOVerify", hangchon].Value.ToString();
            string line = dgrvWOVerify["LineVerify", hangchon].Value.ToString();
            int IsVerify = int.Parse(dgrvWOVerify["IsVerify", hangchon].Value.ToString());
   
            dgrvListReplacError.DataSource = SMTHelper.GetListUPNMinusErrorByWO(WO, line);

            AddConfirm();
            // nếu click button
            var senderGrid = (DataGridView)sender;
            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn
                && e.RowIndex >= 0)
            {
                saveToVerifyToolStripMenuItem_Click();
            }

        }


        private void AddConfirm()
        {
            foreach (DataGridViewRow row in dgrvListReplacError.Rows)
            {
                row.Cells["ConfirmReplace"].Value = "Confirm";
            }
            lblNumberUPN.Text = dgrvListReplacError.RowCount.ToString();
        }

        private void AddButtonStartVerify()
        {
            foreach (DataGridViewRow row in dgrvWOVerify.Rows)
            {
                int IsVerify = int.Parse(row.Cells["IsVerify"].Value.ToString());
                if (IsVerify == Const.DOING)
                {
                    row.Cells["btnVerify"].Value = "Verifying";
                }
                else
                {
                    row.Cells["btnVerify"].Value = "Start";
                }
                DateTime TimeStart = DateTime.Parse(row.Cells["TimeVerify"].Value.ToString());
                if (TimeStart.Year == 9999)
                {
                    row.Cells["TimeVerify"].Value = "";
                }
                string state = row.Cells["StateVerify"].Value.ToString();
                if (IsVerify != Const.DONE && state == Const.FINISH)
                {
                    row.DefaultCellStyle.BackColor = Color.Red;
                }
            }
        }

        private void btnReload_Click(object sender, EventArgs e)
        {
            int hangchon = dgrvWOVerify.CurrentRow.Index;
            string WO = dgrvWOVerify["WOVerify", hangchon].Value.ToString();
            string line = dgrvWOVerify["LineVerify", hangchon].Value.ToString();
            dgrvListReplacError.DataSource = SMTHelper.GetListUPNMinusErrorByWO(WO, line);
            AddConfirm();
        }
        private void WOUpdateTimer_Tick(object sender, EventArgs e)
        {
            SMTHelper.Replace_WO_Update();
            InsertNewWoToDataGridView();
            if (SMTHelper.IsNeedVerifyNow(GetListLine()))
            {
                SendCom("#TON*");
            }
            else
            {
                SendCom("#TOFF*");
            }
        }
        private void InsertNewWoToDataGridView()
        {
            var woVerify = new WOVerify2();
            var list = woVerify.GetWONeedVerify(GetListLine());
            var current = (DataTable)dgrvWOVerify.DataSource;
            if (current == null) return;
            if (list == null) return;
            var listNew = new List<DataRow>();
            foreach (DataRow item in list.Rows)
            {
                var exist = current.AsEnumerable().Where(m => m.Field<string>("WO") == item.Field<string>("WO")).FirstOrDefault();
                if (exist == null)
                {
                    listNew.Add(item);

                }
            }
            this.BeginInvoke(new Action(() =>
            {
                foreach (var item in listNew)
                {
                    int index = dgrvWOVerify.CurrentRow.Index;
                    DataTable dataTable = (DataTable)dgrvWOVerify.DataSource;
                    DataRow drToAdd = dataTable.NewRow();
                    drToAdd["WO"] = item.Field<string>("WO");
                    drToAdd["Line"] = item.Field<string>("Line");
                    drToAdd["State"] = item.Field<string>("State");
                    drToAdd["TimeVerify"] = DateTime.MaxValue;
                    drToAdd["IsVerify"] = false;
                    dataTable.Rows.Add(drToAdd);
                    dataTable.AcceptChanges();
                    AddButtonStartVerify();
                    dgrvWOVerify.FirstDisplayedScrollingRowIndex = index;
                }

            }));

        }
        private void dgrvListReplacError_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // nếu click button
            var senderGrid = (DataGridView)sender;
            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn
                && e.RowIndex >= 0)
            {
                int hangchon = dgrvListReplacError.CurrentRow.Index;
                string WO = dgrvListReplacError["WO", hangchon].Value.ToString();
                string line = dgrvListReplacError["Line", hangchon].Value.ToString();
                string machine = dgrvListReplacError["Machine", hangchon].Value.ToString();
                string slot = dgrvListReplacError["Slot", hangchon].Value.ToString();
                string upn = dgrvListReplacError["UPN", hangchon].Value.ToString();
                var dt = (DataTable)dgrvWOVerify.DataSource;
                var woInfo = dt.AsEnumerable().Where(m => m.Field<string>("WO") == WO && m.Field<string>("Line") == line).FirstOrDefault();
                if (woInfo == null) return;
                new frmReplaceConfirm(new WODetailInfo()
                {
                    WO = WO,
                    Machine = machine,
                    Slot = slot,
                    UPN = upn,
                    ProductId = dgrvListReplacError["Product", hangchon].Value.ToString(),
                    Part = dgrvListReplacError["Part", hangchon].Value.ToString(),
                    Line = line,
                    MaterialOrderId = dgrvListReplacError["Material Order", hangchon].Value.ToString()
                }).ShowDialog();
                btnReload_Click(null, null);
                if (lblNumberUPN.Text == "0")
                {
                    dgrvWOVerify.Rows[dgrvWOVerify.CurrentRow.Index].Cells["btnVerify"].Value = Const.START;
                    dgrvWOVerify.Rows[dgrvWOVerify.CurrentRow.Index].Cells["TimeVerify"].Value = DateTime.Now.ToShortDateString();
                    dgrvWOVerify.Rows[dgrvWOVerify.CurrentRow.Index].Cells["IsVerify"].Value = Const.DONE;
                }

            }
        }

        private void tsConfig_Click(object sender, EventArgs e)
        {
            new frmConfig().ShowDialog();
        }

        private void cmsWOVerify_Opening(object sender, CancelEventArgs e)
        {

        }

        private void saveToVerifyToolStripMenuItem_Click()
        {
            try
            {
                int hangchon = dgrvWOVerify.CurrentRow.Index;
                string WO = dgrvWOVerify["WOVerify", hangchon].Value.ToString();
                string line = dgrvWOVerify["LineVerify", hangchon].Value.ToString();
                int verifyState = int.Parse(dgrvWOVerify["IsVerify", hangchon].Value.ToString());
                IWOVerify wOVerify = new WOVerify2();
                if (verifyState == Const.DOING && wOVerify.IsExistUPNVerified(WO, line))
                {
                    RJMessageBox.Show($"WO {WO} vẫn còn đang verify trên line {line}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (verifyState == Const.DONE || verifyState == Const.DOING)
                {
                    var dialog = RJMessageBox.Show($"Nhấn OK để verify lại WO {WO}!", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dialog != DialogResult.Yes) return;
                    SMTHelper.RemoveWoInLine(WO, line);
                }
                var r = RJMessageBox.Show($"Bạn có muốn verify WO {WO} ở line {line} này không?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (r != DialogResult.Yes) return;
                DataTable dt = new DataTable();
                if (WIPHelper.CheckWOFinish(WO, line))
                {
                    dt = WIPHelper.GetListUPNMinusErrorByWOFinish(WO, line);
                    if (dt.Rows.Count == 0)
                    {
                        RJMessageBox.Show("WO đã finish và không có dữ liệu âm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        var r1 = SMTHelper.UpdateWhenWOFinishedNoError(WO, line);
                        if (string.IsNullOrEmpty(r1))
                        {
                            btnFilterArea_Click(null, null);
                            return;
                        }
                        RJMessageBox.Show(r1, "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                else
                {
                    string woQty = "";
                    var form = new frmWoQty($"Nhập số lượng Wo {WO} trên line {line}");
                    form.message = (x) =>
                    {
                        woQty = x;
                    };
                    r = form.ShowDialog();
                    if (r != DialogResult.Yes) return;
                    dt = WIPHelper.GetListUPNMinusErrorByWO(WO, line, int.Parse(woQty));
                    if (dt.Rows.Count == 0)
                    {
                        RJMessageBox.Show("WO không có dữ liệu âm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                }

                var result = SMTHelper.SaveToVerify(dt.AsEnumerable().ToList(),WO,line);
                if (string.IsNullOrEmpty(result))
                {
                    RJMessageBox.Show("Lưu thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    dgrvWOVerify.Rows[dgrvWOVerify.CurrentRow.Index].Cells["btnVerify"].Value = Const.VERIFYING;
                    dgrvWOVerify.Rows[dgrvWOVerify.CurrentRow.Index].Cells["TimeVerify"].Value = DateTime.Now.ToShortDateString();
                    dgrvWOVerify.Rows[dgrvWOVerify.CurrentRow.Index].Cells["IsVerify"].Value = Const.DOING;
                    dgrvListReplacError.DataSource = dt;
                    AddConfirm();
                }
                else
                {
                    RJMessageBox.Show(result, "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            catch (Exception ex)
            {
                RJMessageBox.Show(ex.Message.ToString(), "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion
        private List<string> GetListLine()
        {
            if (_areaReplace == "ALL") return new List<string>();
            var listLine = _lstLocation.FirstOrDefault(o => o.name == _areaReplace);
            var list = listLine.lstline.Where(m => !string.IsNullOrEmpty(m)).ToList();
            var groupId = Class.Ultils.GetValueRegistryKey("GroupLine");
            if (string.IsNullOrEmpty(groupId)) return list;
            var lineByGroupId = locationList.Where(m => m.GroupID == groupId).Select(m => m.LineId).ToList();
            return lineByGroupId;
        }
    }
}
