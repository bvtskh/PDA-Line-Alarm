using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ZedGraph;

namespace Alarmlines
{
    public class CalculatorChart
    {
        private static List<Tbl_PDADefineError> _lstFilterError = new List<Tbl_PDADefineError>();
        private static List<Tbl_PDADefineError> _lstFilterMidError = new List<Tbl_PDADefineError>();
        private static List<CustomerObj> _lstCustomer = new List<CustomerObj>();
        private static List<LocatonObj> _lstLocation = new List<LocatonObj>();
        //private static bool isPlot;

        public static void SetDataList(List<Tbl_PDADefineError> ferr, List<CustomerObj> fcus, List<LocatonObj> floc, List<Tbl_PDADefineError> fMerr)
        {
            _lstFilterError = ferr; _lstCustomer = fcus; _lstLocation = floc; _lstFilterMidError = fMerr;
        }

        public static List<Piegraphic> createDataRoundGraphic(List<DataTest> lst, DateTime Tfrom, DateTime Tto)
        {
            List<DataTest> _lstFilter = lst.Where(r => r.ErrorTime.Date >= Tfrom && r.ErrorTime.Date < Tto).ToList();
            //vẽ biểu đồ tỉ lệ lỗi các công đoạn (process)
            List<Piegraphic> lstPiegraphic = new List<Piegraphic>()
            {
                new Piegraphic { Content = "BIVN", Qty = 0},
                new Piegraphic { Content = "CVN", Qty = 0},
                new Piegraphic { Content = "YASKAWA", Qty = 0},
                new Piegraphic { Content = "AUTO", Qty = 0}
            };

            foreach (var item in _lstFilter)
            {
                if (item.Location == "BIVN") lstPiegraphic[0].Qty++;
                if (item.Location == "CVN") lstPiegraphic[1].Qty++;
                if (item.Location == "YASKAWA") lstPiegraphic[2].Qty++;
                if (item.Location == "AUTO") lstPiegraphic[3].Qty++;
            }

            // vẽ biểu đồ:
            lstPiegraphic = lstPiegraphic.OrderByDescending(r => r.Qty).ToList();
            return lstPiegraphic;
        }

        public static List<Xaxis_YLocation> GetListReportLocation(string Process, string ReportType, DateTime FromTime, DateTime ToTime, string level)
        {
            List<Xaxis_YLocation> _ListReportLocation = new List<Xaxis_YLocation>();
            DateTime _Today = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            #region báo cáo theo tháng (Tổng số lượng lỗi các khách hàng)
            if (ReportType == "Month")
            {
                //Tính toán dữ liệu tổng kết theo tháng (6 tháng liên tiếp)
                DateTime _12mothsbefore = _Today.AddMonths((-1) * 6);
                DateTime temp = _12mothsbefore;
                while (true)
                {
                    temp = temp.AddMonths(1); temp = new DateTime(temp.Year, temp.Month, 1);

                    Xaxis_YLocation _rc = new Xaxis_YLocation();
                    _rc.BIVN_Location = new NGinfo();
                    _rc.CVN_Location = new NGinfo();
                    _rc.YASKAWA_Location = new NGinfo();
                    _rc.AUTO_Location = new NGinfo();

                    _rc.Date = temp;
                    _rc.X_axis = DefineMonth._month(temp.Month);

                    //Lay du lieu tung thang
                    DateTime T1 = new DateTime(temp.Year, temp.Month, 1, 8, 0, 0);
                    DateTime T2 = T1.AddMonths(1); T2 = new DateTime(T2.Year, T2.Month, 1, 8, 0, 0);
                    try
                    {
                        //lấy dữ liệu toàn bộ model của toàn bộ các khách hàng
                        List<PDA_ErrorHistory> lst = new List<PDA_ErrorHistory>();

                        using (DBContext _dbcontext = new DBContext())
                        {
                            lst = _dbcontext.PDA_ErrorHistory.Where(r => r.ErrorTime >= T1 && r.ErrorTime < T2).ToList();
                        }
                        List<PDA_ErrorHistory> lstFiltertoError = new List<PDA_ErrorHistory>();
                        // Loc theo cac loi quan trong, loc theo khu vuc
                        // Loc theo cac loi quan trong, loc theo khu vuc
                        if (_lstFilterError == null && level == Const.HIGH_LEVEL) return null;
                        if (_lstFilterMidError == null && level == Const.MID_LEVEL) return null;
                        if ((_lstFilterError.Count > 0 && level == Const.HIGH_LEVEL) || (_lstFilterMidError.Count > 0 && level == Const.MID_LEVEL))                           
                        {
                            foreach (var p in lst)
                            {
                                var IfindArea = _lstLocation.FirstOrDefault(o => o.lstline.Contains(p.Line));
                                if (IfindArea != null) p.Location = IfindArea.name;

                                Tbl_PDADefineError IfindError = new Tbl_PDADefineError();
                                if (level == Const.HIGH_LEVEL)
                                    IfindError = _lstFilterError.FirstOrDefault(o => o.PDAErrorContent.Contains(p.ErrorContent));
                                else if (level == Const.MID_LEVEL)
                                    IfindError = _lstFilterMidError.FirstOrDefault(o => o.PDAErrorContent.Contains(p.ErrorContent));
                                if (IfindError != null /*&& (p.Location == AreaFilter || AreaFilter == "ALL")*/)
                                {
                                    p.ErrorContent = IfindError.VNContent;
                                    p.Customer = CustomerObj.getname(p.Customer, _lstCustomer);
                                    //p.Location = LocatonObj.getname(p.Line, _lstLocation);
                                    lstFiltertoError.Add(p);
                                }
                            }
                        }
                        var listlocation = lstFiltertoError.GroupBy(o => o.Location).Select(o => new { o.Key, Count = o.Count() }).ToList();

                        foreach (var item in listlocation)
                        {
                            //tính toán số lượng lỗi theo từng khách hàng
                            #region tính toán cho toàn bộ khách hàng
                            if (item.Key.Contains("BROTHER"))
                            {
                                _rc.BIVN_Location.ProQty = 0;
                                _rc.BIVN_Location.NGQty = (double)item.Count;
                            }
                            else if (item.Key.Contains("CANON"))
                            {
                                _rc.CVN_Location.ProQty = 0;
                                _rc.CVN_Location.NGQty = (double)item.Count;
                            }
                            else if (item.Key.Contains("YASKAWA"))
                            {
                                _rc.YASKAWA_Location.ProQty = 0;
                                _rc.YASKAWA_Location.NGQty = (double)item.Count;
                            }
                            else if (item.Key.Contains("AUTO"))
                            {
                                _rc.AUTO_Location.ProQty = 0;
                                _rc.AUTO_Location.NGQty = (double)item.Count;
                            }
                            #endregion
                        }
                        _ListReportLocation.Add(_rc);
                        if (temp.Month == _Today.Month && temp.Year == _Today.Year) break;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString(), "Error");
                    }
                }
            }
            #endregion (ALL công đoạn)

            #region Báo cáo theo ngày (Tổng số lượng lỗi của các khách hàng theo ngày trong tháng)
            if (ReportType == "Day")
            {
                //Tính toán dữ liệu tổng kết theo ngày
                try
                {
                    DateTime T11 = FromTime;
                    DateTime T_Start = T11.AddDays(-1);
                    T_Start = new DateTime(T_Start.Year, T_Start.Month, T_Start.Day, /*T11.Hour*/8, 0, 0);
                    //Lay du lieu thao ngay
                    List<PDA_ErrorHistory> _lstDataCollection = new List<PDA_ErrorHistory>();

                    using (DBContext _dbcontext = new DBContext())
                    {
                        if (FromTime > ToTime) return null;
                        _lstDataCollection = _dbcontext.PDA_ErrorHistory.Where(r => r.ErrorTime >= T_Start && r.ErrorTime < ToTime).ToList();
                    }

                    TimeSpan T = ToTime - FromTime;
                    int numberDays = (int)T.TotalDays;

                    
                    //phân tích dữ liệu theo từng ngày
                    for (int i = 1; i <= numberDays + 1; i++)
                    {
                        DateTime T8H_hnay = new DateTime();
                        DateTime T20H_hsau = new DateTime();
                        T8H_hnay = new DateTime(T11.Year, T11.Month, T11.Day, /*T11.Hour*/8, 0, 0);
                        

                        DateTime T8H_hsau = T8H_hnay.AddDays(-1);
                        T8H_hsau = new DateTime(T8H_hsau.Year, T8H_hsau.Month, T8H_hsau.Day, /*T11.Hour*/8, 0, 0);
                        T20H_hsau = new DateTime(T8H_hsau.Year, T8H_hsau.Month, T8H_hsau.Day, /*T11.Hour*/20, 0, 0);
                        List<PDA_ErrorHistory> _lstOneDay = new List<PDA_ErrorHistory>();
                        _lstOneDay = _lstDataCollection.Where(r => r.ErrorTime >= T8H_hsau && r.ErrorTime < T8H_hnay).ToList();                        
                        
                        List<PDA_ErrorHistory> lstFiltertoError = new List<PDA_ErrorHistory>();
                        // Loc theo cac loi quan trong, loc theo khu vuc
                        if (_lstFilterError == null && level == Const.HIGH_LEVEL) return null;
                        if (_lstFilterMidError == null && level == Const.MID_LEVEL) return null;
                        if ((_lstFilterError.Count > 0 && level == Const.HIGH_LEVEL) || (_lstFilterMidError.Count > 0 && level == Const.MID_LEVEL))
                            //if (_lstFilterError.Count > 0)
                        {
                            foreach (var p in _lstOneDay)
                            {
                                var IfindArea = _lstLocation.FirstOrDefault(o => o.lstline.Contains(p.Line));
                                if (IfindArea != null) p.Location = IfindArea.name;

                                Tbl_PDADefineError IfindError = new Tbl_PDADefineError();
                                if (level == Const.HIGH_LEVEL)
                                    IfindError = _lstFilterError.FirstOrDefault(o => o.PDAErrorContent.Contains(p.ErrorContent));
                                else if (level == Const.MID_LEVEL)
                                    IfindError = _lstFilterMidError.FirstOrDefault(o => o.PDAErrorContent.Contains(p.ErrorContent));
                                if (IfindError != null /*&& (p.Location == AreaFilter || AreaFilter == "ALL")*/)
                                {
                                    if ((Properties.Settings.Default.Verify == Const.UNVISIBLE && p.ProcessID != 2) || (Properties.Settings.Default.Verify == Const.VISIBLE))
                                    {
                                        p.ErrorContent = IfindError.VNContent;
                                        p.Customer = CustomerObj.getname(p.Customer, _lstCustomer);
                                        //p.Location = LocatonObj.getname(p.Line, _lstLocation);
                                        lstFiltertoError.Add(p);
                                    }
                                }
                            }
                        }
                        ////////////////////////////////////Loc trung lap ban tin/////////////////////////////////////////////////////////
                        var lsttrunglap = lstFiltertoError.GroupBy(o => new { o.PartCode, o.WO, o.Model, o.Slot, o.Line }).Select(o => new PDA_ErrorHistory()
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
                        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                        var listlocationAM = lsttrunglap.Where(o=>o.ErrorTime >= T8H_hsau && o.ErrorTime < T20H_hsau).GroupBy(o => o.Location).Select(o => new { o.Key, Count = o.Count() }).ToList();
                        var listlocationPM = lsttrunglap.Where(o => o.ErrorTime >= T20H_hsau && o.ErrorTime < T8H_hnay).GroupBy(o => o.Location).Select(o => new { o.Key, Count = o.Count() }).ToList();
                        Xaxis_YLocation _rc = new Xaxis_YLocation();
                        _rc.BIVN_AM_Location = new NGinfo();
                        _rc.CVN_AM_Location = new NGinfo();
                        _rc.YASKAWA_AM_Location = new NGinfo();
                        _rc.AUTO_AM_Location = new NGinfo();

                        _rc.BIVN_PM_Location = new NGinfo();
                        _rc.CVN_PM_Location = new NGinfo();
                        _rc.YASKAWA_PM_Location = new NGinfo();
                        _rc.AUTO_PM_Location = new NGinfo();

                        _rc.AUTO1_AM_Location = new NGinfo();
                        _rc.AUTO1_PM_Location = new NGinfo();

                        _rc.AUTO2_AM_Location = new NGinfo();
                        _rc.AUTO2_PM_Location = new NGinfo();

                        _rc.Date = T11;
                        _rc.X_axis = T11.AddDays(-1).Day.ToString();// 8hAM 29 den 8hAM 30 la ngay 29

                        //phân tích 1 ca ngày theo từng khách hàng:
                        foreach (var item in listlocationAM)
                        {
                            //tính toán số lượng lỗi theo từng khách hàng
                            #region tính toán cho toàn bộ khách hàng
                            //if (item.Key == "CS001")
                            if (item.Key.Contains("BROTHER"))
                            {
                                _rc.BIVN_AM_Location.ProQty = 0;
                                _rc.BIVN_AM_Location.NGQty = (double)item.Count;
                            }
                            //else if (item.Key == "CS005")
                            else if (item.Key.Contains("CANON"))
                            {
                                _rc.CVN_AM_Location.ProQty = 0;
                                _rc.CVN_AM_Location.NGQty = (double)item.Count;
                            }
                            //else if (item.Key == "CS045")
                            //else if (LocatonObj.getname(item.Key, _lstLocation) == "YASKAWA")
                            else if (item.Key.Contains("YASKAWA"))
                            {
                                _rc.YASKAWA_AM_Location.ProQty = 0;
                                _rc.YASKAWA_AM_Location.NGQty = (double)item.Count;
                            }
                            //else if (item.Key == "CS016")
                            //else if (LocatonObj.getname(item.Key, _lstLocation) == "AUTO")
                            else if (item.Key.Contains("AUTO1"))
                            {
                                _rc.AUTO1_AM_Location.ProQty = 0;
                                _rc.AUTO1_AM_Location.NGQty = (double)item.Count;
                            }
                            else if (item.Key.Contains("AUTO2"))
                            {
                                _rc.AUTO2_AM_Location.ProQty = 0;
                                _rc.AUTO2_AM_Location.NGQty = (double)item.Count;
                            }
                            #endregion
                        }

                        //phân tích 1 ca dem theo từng khách hàng:
                        foreach (var item in listlocationPM)
                        {
                            //tính toán số lượng lỗi theo từng khách hàng
                            #region tính toán cho toàn bộ khách hàng
                            //if (item.Key == "CS001")
                            if (item.Key.Contains("BROTHER"))
                            {
                                _rc.BIVN_PM_Location.ProQty = 0;
                                _rc.BIVN_PM_Location.NGQty = (double)item.Count;
                            }
                            //else if (item.Key == "CS005")
                            else if (item.Key.Contains("CANON"))
                            {
                                _rc.CVN_PM_Location.ProQty = 0;
                                _rc.CVN_PM_Location.NGQty = (double)item.Count;
                            }
                            //else if (item.Key == "CS045")
                            //else if (LocatonObj.getname(item.Key, _lstLocation) == "YASKAWA")
                            else if (item.Key.Contains("YASKAWA"))
                            {
                                _rc.YASKAWA_PM_Location.ProQty = 0;
                                _rc.YASKAWA_PM_Location.NGQty = (double)item.Count;
                            }
                            //else if (item.Key == "CS016")
                            //else if (LocatonObj.getname(item.Key, _lstLocation) == "AUTO")
                            else if (item.Key.Contains("AUTO1"))
                            {
                                _rc.AUTO1_PM_Location.ProQty = 0;
                                _rc.AUTO1_PM_Location.NGQty = (double)item.Count;
                            }
                            else if (item.Key.Contains("AUTO2"))
                            {
                                _rc.AUTO2_PM_Location.ProQty = 0;
                                _rc.AUTO2_PM_Location.NGQty = (double)item.Count;
                            }
                            #endregion
                        }

                        _ListReportLocation.Add(_rc);

                        T11 = T11.AddDays(1);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "Error");
                }
            }
            #endregion
            return _ListReportLocation;
        }

        public static CalLocation CalculatorLocation(List<Xaxis_YLocation> lst, string select)
        {
            CalLocation p = new CalLocation();
            string[] x = new string[lst.Count];
            double[] BIVN_AM = new double[lst.Count];
            double[] BIVN_PM = new double[lst.Count];
            double[] CVN_AM = new double[lst.Count];
            double[] CVN_PM = new double[lst.Count];
            double[] YASKAWA_AM = new double[lst.Count];
            double[] YASKAWA_PM = new double[lst.Count];
            double[] AUTO_AM = new double[lst.Count];
            double[] AUTO_PM = new double[lst.Count];
            double[] BIVN = new double[lst.Count];
            double[] CVN = new double[lst.Count];
            double[] YASKAWA = new double[lst.Count];
            double[] AUTO = new double[lst.Count];
            double[] Total = new double[lst.Count];

            double[] AUTO1_AM = new double[lst.Count];
            double[] AUTO1_PM = new double[lst.Count];

            double[] AUTO2_AM = new double[lst.Count];
            double[] AUTO2_PM = new double[lst.Count];

            double[] AUTO1 = new double[lst.Count];
            double[] AUTO2 = new double[lst.Count];


            for (int i = 0; i < lst.Count; i++)
            {
                x[i] = lst[i].X_axis;
                BIVN_AM[i] = lst[i].BIVN_AM_Location.NGQty;
                BIVN_PM[i] = lst[i].BIVN_PM_Location.NGQty;
                BIVN[i] = BIVN_AM[i] + BIVN_PM[i];
                CVN_AM[i] = lst[i].CVN_AM_Location.NGQty;
                CVN_PM[i] = lst[i].CVN_PM_Location.NGQty;
                CVN[i] = CVN_AM[i] + CVN_PM[i];
                YASKAWA_AM[i] = lst[i].YASKAWA_AM_Location.NGQty;
                YASKAWA_PM[i] = lst[i].YASKAWA_PM_Location.NGQty;
                YASKAWA[i] = YASKAWA_AM[i] + YASKAWA_PM[i];

                AUTO1_AM[i] = lst[i].AUTO1_AM_Location.NGQty;
                AUTO1_PM[i] = lst[i].AUTO1_PM_Location.NGQty;

                AUTO2_AM[i] = lst[i].AUTO2_AM_Location.NGQty;
                AUTO2_PM[i] = lst[i].AUTO2_PM_Location.NGQty;

                AUTO1[i] = AUTO1_AM[i] + AUTO1_PM[i];
                AUTO2[i] = AUTO2_AM[i] + AUTO2_PM[i];

                if (select == "BROTHER") Total[i] = BIVN[i];
                else if (select == "CANON") Total[i] = CVN[i];
                else if (select == "AUTO1") Total[i] = AUTO1[i];
                else if (select == "AUTO2") Total[i] = AUTO2[i];
                else if (select == "YASKAWA") Total[i] = YASKAWA[i];
                else
                    Total[i] = BIVN[i] + CVN[i] + YASKAWA[i] + AUTO1[i] + AUTO2[i];
            }
            p.Xaxis = x;
            p.BIVN = BIVN;
            p.CVN = CVN;
            p.AUTO1 = AUTO1;
            p.AUTO2 = AUTO2;
            p.YASKAWA = YASKAWA;
            p.Total = Total;

            p.AUTO1_AM = AUTO1_AM;
            p.AUTO1_PM = AUTO1_PM;
            p.AUTO2_AM = AUTO2_AM;
            p.AUTO2_PM = AUTO2_PM;
            p.BIVN_AM = BIVN_AM;
            p.BIVN_PM = BIVN_PM;
            p.CVN_AM = CVN_AM;
            p.CVN_PM = CVN_PM;
            p.YASKAWA_AM = YASKAWA_AM;
            p.YASKAWA_PM = YASKAWA_PM;

            return p;
        }
    }
}
