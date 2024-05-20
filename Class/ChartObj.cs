using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alarmlines
{
    public class ObjectAnalysis
    {
        public string Name { get; set; }
        public int Quantity { get; set; }
    }
    public class DefineMonth
    {
        public static string _month(int i)
        {
            string result = "";
            if (i == 1) result = "Jan";
            if (i == 2) result = "Feb";
            if (i == 3) result = "Mar";
            if (i == 4) result = "Apr";
            if (i == 5) result = "May";
            if (i == 6) result = "Jun";
            if (i == 7) result = "Jul";
            if (i == 8) result = "Aug";
            if (i == 9) result = "Sep";
            if (i == 10) result = "Oct";
            if (i == 11) result = "Nov";
            if (i == 12) result = "Dec";
            return result;
        }
    }
    public class ChartProperty
    {
        public string title { get; set; }
        public string titleX { get; set; }
        public string titleY { get; set; }
        public int FontSize { get; set; }
        public string selectObj { get; set; }
        public string unit { get; set; }

    }

    public class DataTest
    {
        public DateTime ErrorTime { get; set; }
        public string Error { get; set; }
        public string Location { get; set; }
    }

    public class InputCustomerData
    {
        public string[] LableX;
        public double[] BIVN_data;
        public double[] CVN_data;
        public double[] YASKAWA_data;
        public double[] KYO_data;
        public double[] NichiconJP_data;
        public double[] NichiconHK_data;
        public double[] TOYO_data;
        public double[] HLV_data;
        public double[] FUJI_data;
        public double[] YOKOWO_data;
        public double[] VALEO_data;
        public double[] ICHIKOH_data;
        public double[] STANLAY_data;
        public double[] NIDEC_data;
        public double[] FORBALS_data;
        public double[] Total_data;

    }
    public class Report_Summary
    {
        public string X_axis { get; set; }
        public double ProQty { get; set; }
        public double NGQty { get; set; }
        public double Target { get; set; }
    }

    public class Xaxis_YSMTFATRate
    {
        public string X_axis { get; set; }

        public double ProQty { get; set; }
        public double NGQty { get; set; }
        public double SMTProQty { get; set; }
        public double SMTNGQty { get; set; }

        public double Target { get; set; }
    }

    public class NGinfo
    {
        public double ProQty { get; set; }
        public double NGQty { get; set; }
    }
    public class Report_EachProcess
    {
        public DateTime Date { get; set; }
        public string X_axis { get; set; }

        public double ProQtyFAT { get; set; }
        public double ProQtySMT { get; set; }

        public double FAT_NGQty { get; set; }
        public double SMT_NGQty { get; set; }

        public double FLOW_NGQty { get; set; }
        public double ICT_NGQty { get; set; }
        public double FCT_NGQty { get; set; }
        public double KTM1_NGQty { get; set; }
        public double KTM2_NGQty { get; set; }
        public double OQC_NGQty { get; set; }
    }
    public class Xaxis_YCustomer
    {
        public DateTime Date { get; set; }
        public string X_axis { get; set; }
        public NGinfo BIVN_Customer { get; set; }
        public NGinfo CVN_Customer { get; set; }
        public NGinfo YASKAWA_Customer { get; set; }
        public NGinfo KYO_Customer { get; set; }
        public NGinfo NichiconJP_Customer { get; set; }
        public NGinfo NichiconHK_Customer { get; set; }
        public NGinfo TOYO_Customer { get; set; }
        public NGinfo HONDA_Customer { get; set; }
        public NGinfo FUJI_Customer { get; set; }
        public NGinfo YOKOWO_Customer { get; set; }
        public NGinfo VALEO_Customer { get; set; }
        public NGinfo ICHIKOH_Customer { get; set; }
        public NGinfo STANLEY_Customer { get; set; }
        public NGinfo NIHON_Customer { get; set; }
        public NGinfo FORBALS_Customer { get; set; }
        public int TotalNGofRepairType { get; set; }
    }

    public class Xaxis_YLocation
    {
        public DateTime Date { get; set; }
        public string X_axis { get; set; }
        public NGinfo BIVN_AM_Location { get; set; }
        public NGinfo BIVN_PM_Location { get; set; }
        public NGinfo CVN_AM_Location { get; set; }
        public NGinfo CVN_PM_Location { get; set; }
        public NGinfo YASKAWA_AM_Location { get; set; }
        public NGinfo YASKAWA_PM_Location { get; set; }
        public NGinfo AUTO_AM_Location { get; set; }
        public NGinfo AUTO_PM_Location { get; set; }
        public NGinfo AUTO1_AM_Location { get; set; }
        public NGinfo AUTO1_PM_Location { get; set; }
        public NGinfo AUTO2_AM_Location { get; set; }
        public NGinfo AUTO2_PM_Location { get; set; }
        public int TotalNG { get; set; }

        public NGinfo BIVN_Location { get; set; }
        public NGinfo CVN_Location { get; set; }
        public NGinfo YASKAWA_Location { get; set; }
        public NGinfo AUTO_Location { get; set; }
        public NGinfo AUTO1_Location { get; set; }
        public NGinfo AUTO2_Location { get; set; }
    }

    public class Xaxis_YAnalysis
    {
        public DateTime Date { get; set; }
        public string[] X_axis { get; set; }
        public double[] Yaxis { get; set; }
        public int TotalNG { get; set; }
    }

    public class CalLocation
    {
        public string[] Xaxis;
        public double[] BIVN_AM;
        public double[] BIVN_PM;
        public double[] CVN_AM;
        public double[] CVN_PM;
        public double[] YASKAWA_AM;
        public double[] YASKAWA_PM;
        public double[] AUTO_AM;
        public double[] AUTO_PM;
        public double[] AUTO1_AM;
        public double[] AUTO1_PM;
        public double[] AUTO2_AM;
        public double[] AUTO2_PM;
        public double[] Total;

        public double[] BIVN;
        public double[] CVN;
        public double[] YASKAWA;
        public double[] AUTO;
        public double[] AUTO1;
        public double[] AUTO2;
    }
    public class showDgv_DataErrorALLCustomer
    {
        public DateTime Date { get; set; }
        public string X_axis { get; set; }
        public string BIVN_Customer { get; set; }
        public string CVN_Customer { get; set; }
        public string YASKAWA_Customer { get; set; }
        public string KYO_Customer { get; set; }
        public string NichiconJP_Customer { get; set; }
        public string NichiconHK_Customer { get; set; }
        public string TOYO_Customer { get; set; }
        public string HONDA_Customer { get; set; }
        public string FUJI_Customer { get; set; }
        public string YOKOWO_Customer { get; set; }
        public string VALEO_Customer { get; set; }
        public string ICHIKOH_Customer { get; set; }
        public string STANLEY_Customer { get; set; }
        public string NIHON_Customer { get; set; }
        public string FORBALS_Customer { get; set; }
    }
    public class showDgv_DayALLcustomer
    {
        public string Day { get; set; }
        public string BIVN_Customer { get; set; }
        public string CVN_Customer { get; set; }
        public string YASKAWA_Customer { get; set; }
        public string KYO_Customer { get; set; }
        public string NICHICON_Customer { get; set; }
        public string TOYO_Customer { get; set; }
        public string HONDA_Customer { get; set; }
        public string FUJI_Customer { get; set; }
        public string YOKOWO_Customer { get; set; }
        public string VALEO_Customer { get; set; }
        public string ICHIKOH_Customer { get; set; }
        public string STANLEY_Customer { get; set; }
        public string NIHON_Customer { get; set; }
        public string FORBALS_Customer { get; set; }
        public string ALL_Customer_pcs { get; set; }
    }
    public class Report_Model
    {
        public string CustomerCODE { get; set; }
        public string ModelSeri { get; set; }
        public string ModelName { get; set; }

        public double SMTProQty { get; set; }
        public double FATProQty { get; set; }

        public double _ProQty { get; set; }
        public double _NGQty { get; set; }

        public double Total_NGQty { get; set; }
        public double SMT_NGQty { get; set; }
        public double Flow_NGQty { get; set; }
        public double ICT_NGQty { get; set; }
        public double FCT_NGQty { get; set; }
        public double KTM1_NGQty { get; set; }
        public double KTM2_NGQty { get; set; }
        public double OQC_NGQty { get; set; }

        public double Noichan_NGQty { get; set; }
        public double Thieuthiec_NGQty { get; set; }
        public double Thuathiec_NGQty { get; set; }
        public double Kenhchan_NGQty { get; set; }
        public double Tunglk_NGQty { get; set; }
        public double Volk_NGQty { get; set; }
        public double Nguoclk_NGQty { get; set; }
        public double Nhamlk_NGQty { get; set; }
        public double Lechlk_NGQty { get; set; }
        public double Coating_NGQty { get; set; }

        public double NGRate { get; set; }
        public string Unit { get; set; }
        public string KaizenHistory { get; set; }
    }
    public class Report_EachError
    {
        public string ErrorContent { get; set; }
        public double NGQty { get; set; }
        public double NGpercent { get; set; }
        public string Unit { get; set; }
    }
    public class Piegraphic
    {
        public string Content { get; set; }
        public double Qty { get; set; }
    }

    public class Xaxis_YProcess
    {
        public string X_axis { get; set; }
        public int TotalNGQty { get; set; }
        public int SMT_NGQty { get; set; }
        public int FLOW_NGQty { get; set; }
        public int ICT_NGQty { get; set; }
        public int FCT_NGQty { get; set; }
        public int KTM1_NGQty { get; set; }
        public int KTM2_NGQty { get; set; }
        public int OQC_NGQty { get; set; }
        public int AOI_NGQty { get; set; }
        public int SMTtoFAT { get; set; }
    }
    public class Xaxis_YTypeofError
    {
        public string X_axis { get; set; }

        public double Thieuthiec_NGQty { get; set; }
        public double Noichan_NGQty { get; set; }
        public double Thuathiec_NGQty { get; set; }
        public double Kenhchan_NGQty { get; set; }
        public double Tunglk_NGQty { get; set; }
        public double Volk_NGQty { get; set; }
        public double Nguoclk_NGQty { get; set; }
        public double Nhamlk_NGQty { get; set; }
        public double Lechlk_NGQty { get; set; }
        public double Coating_NGQty { get; set; }

        public int TotalNGQty { get; set; }
        public int TotalProQty { get; set; }
    }

}
