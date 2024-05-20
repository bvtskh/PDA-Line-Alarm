using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Alarmlines
{
    public class Const
    {
        public readonly static string SoftwareName = "Alarmlines Ver";
        public readonly static string pathFolderConfig = Application.StartupPath + "\\Setting";
        public readonly static string pathConfig = Application.StartupPath + "\\Setting\\Setting.xml";// lay trong thu muc project "config.xml"
        //public readonly static string pathDeviceConfig = Path.Combine(Application.StartupPath, "device_params.xml");// lay trong thu muc project "config.xml"
        //public readonly static string pathDevice = Path.Combine(Application.StartupPath, "device");
        //public readonly static string pathSystemSetting = Application.StartupPath + "\\Setting\\" + "System";
        public readonly static string VersionPath = $@"\\172.28.10.12\DX Center\16. SMT PDA Alarm System\";
        public readonly static string pathError = Application.StartupPath + "\\Setting\\" + "Error";
        public readonly static string pathCustomer = Application.StartupPath + "\\Setting\\" + "Customer";
        public readonly static string pathLocation = Application.StartupPath + "\\Setting\\" + "Location";
        public readonly static string pathExport = Application.StartupPath + "\\Setting\\" + "Export";
        public readonly static string pathLog = Application.StartupPath + "\\Log";
        public readonly static string PASSWORD = "lca@123";
        public readonly static string USERNAME = "admin";
        public readonly static string NON_TIME_DATABASE = "NoneTime";
        public readonly static string TIME_DATABASE = "FilterTime";
        public readonly static List<string> lstObjAnalysis = new List<string>() { "Khách hàng", "Công nhân", "Mã linh kiện", "Model", "WO", "Line", "Công đoạn" };
        public readonly static string OBJ_AREA = "Khu vực";
        public readonly static string OBJ_CUSTOMER = "Khách hàng";
        public readonly static string OBJ_OPERATOR = "Công nhân";
        public readonly static string OBJ_PARTCODE = "Mã linh kiện";
        public readonly static string OBJ_MODEL = "Model";
        public readonly static string OBJ_WO = "WO";
        public readonly static string OBJ_LINE = "Line";
        public readonly static string OBJ_PROCESS = "Công đoạn";

        public readonly static string HIGH_LEVEL = "High";
        public readonly static string MID_LEVEL = "Mid";
        public readonly static string CLEAR_LEVEL = "ClearAll";

        public readonly static string DRAW_DAY = "Day";
        public readonly static string DRAW_MONTH = "Month";

        public readonly static string VISIBLE = "Visible";
        public readonly static string UNVISIBLE = "Unvisible";

        public const int MONITOR_TAB = 0;
        public const int ANALYSIS_TAB = 1;
        public const int SEARCH_TAB = 2;

        public const int NON_ERR_PROC = 0;
        public const int LOADPART_PROC = 1;
        public const int VERIFY_PROC = 2;
        public const int RELOAD_PROC = 3;

        public readonly static string TAB_NGHIEMTRONG = "Lỗi nghiêm trọng";
        public readonly static string TAB_NO_NGHIEMTRONG = "Lỗi không cảnh báo";
        public readonly static string TAB_PHANTICH = "Phân tích dữ liệu";
        public readonly static string TAB_TIMKIEM = "Tìm kiếm";
        public readonly static string TAB_DULIEUDAUGIO = "Dữ liệu check đầu giờ";
        public readonly static string TAB_CANHBAOTHAYNOI = "Cảnh báo thay nối";

        public readonly static List<ProcessObject> lstProcessPDA = new List<ProcessObject>() {
                                                                   new ProcessObject() { name_process = "ALL", name_processVN = "Tất cả công đoạn", id = 0},
                                                                   new ProcessObject() { name_process = "Load Part", name_processVN = "Chuẩn bị ban đầu", id = 1},
                                                                   new ProcessObject() { name_process = "Verify Part", name_processVN = "Xác nhận đầu giờ",id = 2},
                                                                   new ProcessObject() { name_process = "Reload Part", name_processVN = "Thay nối linh kiện", id = 3} 
                                                                   };
        public static string RUNNING = "Running";
        public static string FINISH = "Finish";
        public static string START = "Start";
        public static string VERIFYING = "Verifying";

        // state verify
        public static int NONE = 0;
        public static int DOING = 1;
        public static int DONE = 2;

        //com
        public static string COM = "COM";
        public static string SignalOK = "SignalOK";
        public static string SignalNG = "SignalNG";
    }

    public class ProcessObject{
        public string name_process { get; set; }
        public string name_processVN { get; set; }
        public int id { get; set; }
    }

    public class PDA_ErrorHistoryEdit
    {
        public long id { get; set; }

        public string SystemId { get; set; }

        public DateTime ErrorTime { get; set; }

        public string Line { get; set; }

        public string Model { get; set; }

        public string WO { get; set; }

        public string PartCode { get; set; }
        
        public string ErrorContent { get; set; }

        public string OperatorCode { get; set; }

        public string Customer { get; set; }

        public string Location { get; set; }

        public int? Slot { get; set; }

        public string ProcessID { get; set; }
    }


    public class PDA_ErrorHistory_Confirm
    {
        public long id { get; set; }

        public string SystemId { get; set; }

        public DateTime ErrorTime { get; set; }
        
        public string Line { get; set; }
        
        public string Model { get; set; }
        
        public string WO { get; set; }
        
        public string PartCode { get; set; }
        
        public string ErrorContent { get; set; }
        
        public string OperatorCode { get; set; }
        
        public string Customer { get; set; }
        
        public string Location { get; set; }

        public int? Slot { get; set; }

        public int? ProcessID { get; set; }

        public bool isConfirm { get; set; }
    }
}
