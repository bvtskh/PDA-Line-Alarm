using CommonProject.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alarmlines.Class.WOVerify
{
    class WOVerify2 : IWOVerify
    {
        public DataTable GetWONeedVerify(List<string> line)
        {
           
            string lineSql = "";
            if(line.Count > 0)
            {
                List<string> arr = new List<string>();
                foreach (var item in line)
                {
                    arr.Add($"'{item}'");
                }
                lineSql = $"AND LINE_ID IN ({string.Join(",",arr.ToArray())})";
            }
            SQLHelper.ConnectString(new SMTConfig());
            string sql = $@"SELECT [PRODUCTION_ORDER_ID] AS WO
                          ,[LINE_ID] AS Line
                          ,[STATE] AS [State]
                          ,[VERIFY] AS IsVerify
                          ,[TIME_START] AS [TimeVerify]
                          ,[QUANTITY]
                      FROM [SMT].[dbo].[Replace_WO_Status]
                      WHERE 1=1
                        {lineSql}
                        AND ([STATE] = 'Running' 
                        OR ([STATE] = 'Finish' AND VERIFY < 2))
                      ORDER BY IsVerify";
            return SQLHelper.ExecQueryDataAsDataTable(sql);
        }
        
        public bool IsExistUPNVerified(string WO, string line)
        {
            try
            {
                SQLHelper.ConnectString(new SMTConfig());
                var sql = $@"SELECT Count(*) FROM [SMT].[dbo].[Replace_Verify_Status] WHERE PRODUCT_ID = '{WO}' AND LINE_ID = '{line}' AND IS_VERIFY = 1";
                int verifyState = (int)SQLHelper.ExecQueryData<int>(sql).FirstOrDefault();
                if (verifyState > 0) return true;
                else return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
