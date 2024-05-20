using CommonProject.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alarmlines.Class.WOVerify
{
    class WOVerify1 : IWOVerify
    {
        public DataTable GetWONeedVerify(List<string> line)
        {
            var listError = new List<WOInfo>();
            var listVerifying = new List<WOInfo>();
            if (line.Count == 0)
            {
                listError = WIPHelper.GetListWOMinusError().ToList();
                listVerifying = SMTHelper.GetWoDoingVerify();
            }
            else
            {
                listError = WIPHelper.GetListWOMinusError().Where(m => line.Contains(m.Line)).ToList();
                listVerifying = SMTHelper.GetWoDoingVerify().Where(m => line.Contains(m.Line)).ToList();
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
        public bool IsExistUPNVerified(string WO, string line)
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
    }
}
