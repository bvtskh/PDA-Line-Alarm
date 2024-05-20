using CommonProject.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alarmlines.Class
{
    public class SMTHelper
    {
        public static List<string> GetLineByCustomer(string cus)
        {
            SQLHelper.ConnectString(new SMTConfig());
            var sql = $@"SELECT [LineId]
                                FROM[SMT].[dbo].[Location] 
                                WHERE 1=1 ";
            if (!string.IsNullOrEmpty(cus) && cus != "ALL")
            {
                sql += $@" AND LocationId = '{cus}'";
            }
            return SQLHelper.ExecQueryData<string>(sql).ToList();
        }
        public static List<WOInfo> GetWoDoingVerify()
        {
            SQLHelper.ConnectString(new SMTConfig());
            var sql = $@"SELECT [PRODUCTION_ORDER_ID] AS WO
                              ,[LINE_ID] AS Line
                          FROM [SMT].[dbo].[Replace_Verify]
                          WHERE 1=1
                          GROUP BY PRODUCTION_ORDER_ID,LINE_ID";
            return SQLHelper.ExecQueryData<WOInfo>(sql).ToList();
        }
        public static WOInfo CheckWOVerifying(string WO, string Line)
        {
            SQLHelper.ConnectString(new SMTConfig());
            var sql = $@"SELECT  PRODUCTION_ORDER_ID AS WO
                                ,IS_VERIFY
                          FROM [SMT].[dbo].[Replace_Verify]
                          WHERE 1=1
                           AND IS_VERIFY = 0   
                           AND PRODUCTION_ORDER_ID = '{WO}'
                           AND LINE_ID = '{Line}'";
            return SQLHelper.ExecQueryData<WOInfo>(sql).FirstOrDefault();
        }
      
        public static string SaveToVerify(List<DataRow> dr, string WO = "", string line = "")
        {
            try
            {
                SQLHelper.ConnectString(new SMTConfig());
                var dataTable = new DataTable();
                dataTable.Columns.Add("PRODUCTION_ORDER_ID", typeof(string));
                dataTable.Columns.Add("PRODUCT_ID", typeof(string));
                dataTable.Columns.Add("CUSTOMER_ID", typeof(string));
                dataTable.Columns.Add("LINE_ID", typeof(string));
                dataTable.Columns.Add("MATERIAL_ORDER_ID", typeof(string));
                dataTable.Columns.Add("MACHINE_ID", typeof(string));
                dataTable.Columns.Add("MACHINE_SLOT", typeof(int));
                dataTable.Columns.Add("PART_ID", typeof(string));
                dataTable.Columns.Add("UPN_ID", typeof(string));
                dataTable.Columns.Add("UPD_TIME", typeof(DateTime));
                dataTable.Columns.Add("IS_VERIFY", typeof(bool));
                dataTable.Columns.Add("DEDUCT_QUANTITY", typeof(double));
                DateTime dateTime = DateTime.Now;
                string wo = "";
                if(dr.Count > 0)
                {
                    wo = dr[0].ItemArray[0].ToString();
                    var sql = $"SELECT PRODUCTION_ORDER_ID FROM [SMT].[dbo].[Replace_Verify]" +
                              $" WHERE PRODUCTION_ORDER_ID = '{wo}' AND IS_VERIFY = 0";
                    var result = SQLHelper.ExecQuerySacalar(sql);
                    if (result != null)
                    {
                        return $"{wo} Đang thực hiện confirm!";
                    }
                   
                }
                else
                {
                    dataTable.Rows.Add(WO, "", "", line, "", "", 0, "", "", dateTime, false, 0);
                }

                foreach (DataRow dtRow in dr)
                {
                    if (dtRow.ItemArray[0].ToString() != wo)
                    {
                        return $"{dtRow.ItemArray[0]} Không nằm trong danh sách cần confirm!";
                    }
                    dataTable.Rows.Add(dtRow.ItemArray[0],
                        dtRow.ItemArray[1],
                        dtRow.ItemArray[2],
                        dtRow.ItemArray[3],
                        dtRow.ItemArray[4],
                        dtRow.ItemArray[5],
                        dtRow.ItemArray[6],
                        dtRow.ItemArray[7],
                        dtRow.ItemArray[8],
                        dateTime,
                        false,
                        dtRow.ItemArray[9]
                        );
                }
                
                var r = SQLHelper.ExecProcedureNonData("Replace_Verify_Update1", new { Data = dataTable });
                return (int)r > 0 ? "" : "Có lỗi xảy ra trong quá trình lưu dữ liệu!";
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }

        }

        public static string Replace_WO_Update()
        {
            var dt = new DataTable();
            dt.Columns.Add("PRODUCTION_ORDER_ID", typeof(string));
            dt.Columns.Add("MATERIAL_ORDER_ID", typeof(string));
            dt.Columns.Add("CUSTOMER_ID", typeof(string));
            dt.Columns.Add("LINE_ID", typeof(string));
            dt.Columns.Add("STATE", typeof(string));
            dt.Columns.Add("VERIFY", typeof(bool));
            dt.Columns.Add("TIME_START", typeof(DateTime));
            dt.Columns.Add("TIME_END", typeof(DateTime));
            dt.Columns.Add("QUANTITY", typeof(double));
            var listError = WIPHelper.GetListWOMinusError().ToList();
            var a = listError.Where(w => w.WO == "2000989984").ToList();
            foreach (var item in listError)
            {
                dt.Rows.Add(new object[]
                {
                    item.WO,
                    "",
                    "",
                    item.Line,
                    Const.RUNNING,
                    false,
                    DateTime.MaxValue,
                    DateTime.MaxValue,
                    item.QUANTITY
                });
            }

            SQLHelper.ConnectString(new SMTConfig());
            var result = SQLHelper.ExecProcedureNonData("Replace_WO_Update", new { Data = dt });
            return (int)result > 0 ? "" : "Có lỗi xảy ra trong quá trình lưu dữ liệu!";
        }
        public static bool IsNeedVerifyNow(List<string> line)
        {
            var sql = $@"SELECT LINE_ID FROM [SMT].[dbo].[Replace_WO_Status] 
                            WHERE 1=1
                            AND (VERIFY = {Const.DOING}
                            OR (VERIFY = {Const.NONE} AND [STATE] = '{Const.FINISH}'))
                            ";
            var result = SQLHelper.ExecQueryData<string>(sql).ToList();
            if(line.Count > 0)
            {
                result = result.Where(m => line.Contains(m)).ToList();
            }
           
            return result.Count > 0;
        }

        public static DataTable GetWONeedVerify(List<string> line)
        {
            var listError = new List<WOInfo>();
            var listVerifying = new List<WOInfo>();
            if (line.Count == 0)
            {
                listError = WIPHelper.GetListWOMinusError().ToList();
                listVerifying = GetWoDoingVerify();
            }
            else
            {
                listError = WIPHelper.GetListWOMinusError().Where(m => line.Contains(m.Line)).ToList();
                listVerifying = GetWoDoingVerify().Where(m => line.Contains(m.Line)).ToList();
            }

            var dt = new DataTable();
            dt.Columns.Add("WO", typeof(string));
            dt.Columns.Add("Line", typeof(string));
            dt.Columns.Add("State", typeof(string));
            dt.Columns.Add("IsVerify", typeof(bool));
            foreach (var item in listError)
            {
                var check = listVerifying.Where(m => m.WO == item.WO && m.Line == item.Line).FirstOrDefault();
                if (check == null)
                {
                    dt.Rows.Add(new object[]
                    {
                        item.WO,
                        item.Line,
                        Const.RUNNING,
                        false
                    });
                }
            }
            foreach (var item in listVerifying)
            {
                var isRunning = listError.Where(m => m.WO == item.WO && m.Line == item.Line).FirstOrDefault();
                dt.Rows.Add(new object[]
                    {
                        item.WO,
                        item.Line,
                        isRunning == null ? Const.FINISH : Const.RUNNING,
                        true
                    });
            }
            return dt;
        }

        internal static DataTable GetListUPNMinusErrorByWO(string wO, string line)
        {
            SQLHelper.ConnectString(new SMTConfig());
            var sql = $@"SELECT 
											t0.[PRODUCTION_ORDER_ID] AS [WO]
					                        ,t0.[PRODUCT_ID] AS Product
					                        ,t0.[LINE_ID] AS Line
					                        ,t0.[MATERIAL_ORDER_ID] AS [Material Order]
					                        ,t0.[MACHINE_ID] AS Machine
					                        ,t0.[MACHINE_SLOT] AS Slot
					                        ,t0.[PART_ID] AS [Part]
					                        ,t0.[UPN_ID] AS UPN
                                            ,t0.[DEDUCT_QUANTITY] AS [Deduct Qty]
                                            FROM Replace_Verify t0
											WHERE PRODUCTION_ORDER_ID = '{wO}'
											AND LINE_ID = '{line}'";
            return SQLHelper.ExecQueryDataAsDataTable(sql);
        }
        internal static string UpdatePDAByConfirm(WODetailInfo info)
        {
            try
            {
                SQLHelper.ConnectString(new SMTConfig());
                var sql = $@"UPDATE Replace_Verify_Status 
                    SET IS_VERIFY=1
                    , UPD_TIME=GETDATE() 
                    , REMARK = N'{info.Remark}'
	                where PRODUCTION_ORDER_ID='{info.WO}'
                    and MACHINE_ID='{info.Machine}'
                    and UPN_ID='{info.UPN}'
                    and MACHINE_SLOT='{info.Slot}'
                    ";
                SQLHelper.ExecQueryNonData(sql);
                SQLHelper.ExecProcedureNonData("DELETE_ITEM_REPLACE_VERIFY",
                    new
                    {
                        WO = info.WO,
                        Machine = info.Machine,
                        slot = info.Slot,
                        UPN = info.UPN
                    });
                sql = $@"INSERT INTO [dbo].[Replace_Verify_LOG]
                           ([PRODUCTION_ORDER_ID]
                           ,[PRODUCT_ID]
                           ,[LINE_ID]
                           ,[MATERIAL_ORDER_ID]
                           ,[MACHINE_ID]
                           ,[MACHINE_SLOT]
                           ,[PART_ID]
                           ,[UPN_ID]
                           ,[UPN_SCAN]
                           ,[UPD_TIME]
                           ,[ERROR_MESSAGE]
                           ,[USER_UPD]
                           ,[IS_VERIFY]
                           ,[REMARK])
                     VALUES
                           ('{info.WO}'
                           ,'{info.ProductId}'
                           ,'{info.Line}'
                           ,'{info.MaterialOrderId}'
                           ,'{info.Machine}'
                           ,'{info.Slot}'
                           ,'{info.Part}'
                           ,'{info.UPN}'
                           ,'{info.UPN}'
                           ,GETDATE()
                           ,'OK'
                           ,'Test'
                           ,1
                           ,N'{info.Remark}')";
                SQLHelper.ExecQueryNonData(sql);

                return "Cập nhật thành công!";
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }


        }
        internal static bool IsWOVerifingInLine(string wO, string line)
        {
            SQLHelper.ConnectString(new SMTConfig());
            var sql = $@"SELECT PRODUCTION_ORDER_ID AS WO
                                FROM Replace_Verify t0
                                WHERE PRODUCTION_ORDER_ID = '{wO}'
                                AND LINE_ID = '{line}'
                                AND IS_VERIFY = 0";
            var result = SQLHelper.ExecQueryData<WOInfo>(sql).FirstOrDefault();
            if (result != null) return true;
            return false;
        }
        internal static bool RemoveWoInLine(string wO, string line)
        {
            try
            {
                SQLHelper.ConnectString(new SMTConfig());
                var sql = $@"DELETE FROM Replace_Verify WHERE PRODUCTION_ORDER_ID = '{wO}'
                                AND LINE_ID = '{line}'";
                SQLHelper.ExecQueryNonData(sql);
                sql = $@"DELETE FROM Replace_Verify_Status WHERE  PRODUCTION_ORDER_ID = '{wO}' AND LINE_ID = '{line}'";
                SQLHelper.ExecQueryNonData(sql);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        internal static bool IsExistUPNVerified(string WO, string line)
        {
            try
            {
                SQLHelper.ConnectString(new SMTConfig());
                var sql = $@"SELECT TOP 1  UPD_TIME FROM [SMT].[dbo].[Replace_Verify] WHERE PRODUCTION_ORDER_ID = '{WO}' AND LINE_ID = '{line}' ORDER BY UPD_TIME DESC";
                var timeRun = SQLHelper.ExecQueryData<DateTime>(sql).FirstOrDefault();
                if (timeRun == null) return false;
                sql = $@"SELECT TOP 1 PRODUCTION_ORDER_ID FROM [SMT].[dbo].[Replace_Verify_Status] 
                            WHERE PRODUCTION_ORDER_ID = '{WO}' AND LINE_ID = '{line}' AND UPD_TIME = '{timeRun.ToString("yyyy-MM-dd HH:mm:ss.fff")}' AND IS_VERIFY = 1";
                var result = SQLHelper.ExecQueryData<string>(sql).FirstOrDefault();
                return !string.IsNullOrEmpty(result);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static string UpdateWhenWOFinishedNoError(string Wo, string lineID)
        {
            try
            {
                SQLHelper.ConnectString(new SMTConfig());
                var sql = $@"UPDATE  [SMT].[dbo].[Replace_WO_Status] SET VERIFY = 2 WHERE PRODUCTION_ORDER_ID = '{Wo}' AND LINE_ID = '{lineID}' ";
                SQLHelper.ExecQuerySacalar(sql);
                return "";
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
           
        }

        internal static DataTable GetLineReplaceError(List<string> line)
        {
            try
            {
                string lineSql = "";
                if (line.Count > 0)
                {
                    List<string> arr = new List<string>();
                    foreach (var item in line)
                    {
                        arr.Add($"'{item}'");
                    }
                    lineSql = $"AND LINE_ID IN ({string.Join(",", arr.ToArray())})";
                }
                SQLHelper.ConnectString(new SMTConfig());
                var sql = $@"SELECT LINE_ID AS 'Line'
                            ,COUNT(DISTINCT UPN_ID) AS 'Số lỗi'
                          FROM [SMT].[dbo].[Replace_Verify_Status] 
                          WHERE 1=1
                          AND (REMARK like '2.%' OR REMARK like '3.%')
                          {lineSql}
                          AND UPD_TIME <= '{DateTime.Now.ToString("yyyy-MM-dd")}'
                          AND UPD_TIME >= '{DateTime.Now.AddDays(-6).ToString("yyyy-MM-dd")}'
                          GROUP BY LINE_ID";
                return SQLHelper.ExecQueryDataAsDataTable(sql);
            }
            catch (Exception ex)
            {
                return new DataTable();
            }
        }
        internal static List<WODetailInfo> GetUPNNotReplace(List<string> line)
        {
            try
            {
                string lineSql = "";
                if (line.Count > 0)
                {
                    List<string> arr = new List<string>();
                    foreach (var item in line)
                    {
                        arr.Add($"'{item}'");
                    }
                    lineSql = $"AND LINE_ID IN ({string.Join(",", arr.ToArray())})";
                }
                SQLHelper.ConnectString(new SMTConfig());
                var sql = $@"SELECT PRODUCTION_ORDER_ID AS WO,
		                            LINE_ID AS Line,
		                            UPN_ID AS UPN,
		                            UPD_TIME AS Upd_Time,
		                            REMARK AS Remark 
                              FROM [SMT].[dbo].[Replace_Verify_Status] 
                              WHERE 1=1
                              AND REMARK like '2.%'
                              {lineSql}
                              AND UPD_TIME <= '{DateTime.Now.ToString("yyyy-MM-dd")}'
                              AND UPD_TIME >= '{DateTime.Now.AddDays(-6).ToString("yyyy-MM-dd")}'
                              ";
                return SQLHelper.ExecQueryData<WODetailInfo>(sql).ToList();
            }
            catch (Exception ex)
            {
                return new List<WODetailInfo>();
            }
        }
        internal static List<WODetailInfo> GetUPNWrong(List<string> line)
        {
            try
            {
                string lineSql = "";
                if (line.Count > 0)
                {
                    List<string> arr = new List<string>();
                    foreach (var item in line)
                    {
                        arr.Add($"'{item}'");
                    }
                    lineSql = $"AND LINE_ID IN ({string.Join(",", arr.ToArray())})";
                }
                SQLHelper.ConnectString(new SMTConfig());
                var sql = $@"SELECT PRODUCTION_ORDER_ID AS WO,
		                            LINE_ID AS Line,
		                            UPN_ID AS UPN,
		                            UPD_TIME AS Upd_Time,
		                            REMARK AS Remark 
                              FROM [SMT].[dbo].[Replace_Verify_Status] 
                              WHERE 1=1
                              AND REMARK like '3.%'
                              {lineSql}
                              AND UPD_TIME <= '{DateTime.Now.ToString("yyyy-MM-dd")}'
                              AND UPD_TIME >= '{DateTime.Now.AddDays(-6).ToString("yyyy-MM-dd")}'
                              ";
                return SQLHelper.ExecQueryData<WODetailInfo>(sql).ToList();
            }
            catch (Exception ex)
            {
                return new List<WODetailInfo>();
            }
        }

    }
}
