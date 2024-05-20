using CommonProject.Entities;
using NPOI.OpenXmlFormats.Dml;
using NPOI.POIFS.Crypt.Dsig;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Alarmlines.Class
{
    public static class WIPHelper
    {
        public static DataTable GetListUPNMinusErrorByWO(string WO, string line, int actualQty)
        {
            DataTable CuonLK2met = GetLK2metData();
            SQLHelper.ConnectString(new UMESConfig());
            var data = SQLHelper.ExecProcedureDataAsDataTable("DX_GetListUPNMinusError_2", new { wo = WO, line = line, actualQty = actualQty });
            DataTable resultDataError = data.Clone();
            var query = (from t1 in data.AsEnumerable()
                         join t2 in CuonLK2met.AsEnumerable()
                         on t1.Field<string>("Part") equals t2.Field<string>("PartId") into gr // left join
                         from subtable in gr.DefaultIfEmpty()
                         select new
                         {
                             WO = t1.Field<string>("WO"),
                             Product = t1.Field<string>("Product"),
                             Customer = t1.Field<string>("Customer"),
                             Line = t1.Field<string>("Line"),
                             Material_Order = t1.Field<string>("Material Order"),
                             Machine = t1.Field<string>("Machine"),
                             Slot = t1.Field<int>("Slot"),
                             Part = t1.Field<string>("Part"),
                             UPN = t1.Field<string>("UPN"),
                             Deduct_Qty = t1.Field<double>("Deduct Qty") - (subtable == null ? 0 : subtable.Field<int>("Quantity")), // trừ đi giá trị tại cuộn 2 mét
                             UPN_Qty = t1.Field<object>("UPN Qty"),
                             Actual_WO_Qty = t1.Field<object>("Actual WO Qty"),
                             Unit = t1.Field<object>("Unit"),
                             WO_Qty = t1.Field<object>("WO Qty"),
                             Update_Date = t1.Field<DateTime>("Update Date")
                         }).ToList().Where(w => w.Deduct_Qty <= 0).ToList();

            foreach (var item in query)
            {
                resultDataError.Rows.Add(
                    item.WO,
                    item.Product,
                    item.Customer,
                    item.Line,
                    item.Material_Order,
                    item.Machine,
                    item.Slot,
                    item.Part,
                    item.UPN,
                    item.Deduct_Qty,
                    item.UPN_Qty,
                    item.Actual_WO_Qty,
                    item.Unit,
                    item.WO_Qty,
                    item.Update_Date);
            }
            return resultDataError;
        }

        private static DataTable GetLK2metData()
        {
            SQLHelper.ConnectString(new KittingConfig());
            string sql = @"select *  FROM [KittingManagement].[dbo].[ReelDemension]";
            return SQLHelper.ExecQueryDataAsDataTable(sql, "");
        }

        public static List<WOInfo> GetListWOMinusError()
        {
            SQLHelper.ConnectString(new UMESConfig());
            return SQLHelper.ExecProcedureData<WOInfo>("DX_GetListWOMinusError").ToList();
        }

        internal static DataTable GetListUPNMinusErrorByWOFinish(string wO, string line)
        {
            DataTable CuonLK2met = GetLK2metData();
            SQLHelper.ConnectString(new UMESConfig());
            var sql = $@"SELECT  
					t0.[PRODUCTION_ORDER_ID] AS [WO]
					,t0.[PRODUCT_ID] AS Product
					,t0.[CUSTOMER_ID] AS Customer
					,t0.[LINE_ID] AS Line
					,t0.[MATERIAL_ORDER_ID] AS [Material Order]
					,t0.[MACHINE_ID] AS Machine
					,t0.[MACHINE_SLOT] AS Slot
					,t0.[PART_ID] AS [Part]
					,t0.[UPN_ID] AS UPN
                    ,t0.CURRENT_QUANTITY AS [Deduct Qty]
					,t0.[UPDATE_TIME] AS [Update Date]
					FROM UPN_MINUS_INFO t0
					WHERE PRODUCTION_ORDER_ID = '{wO}'
					AND LINE_ID = '{line}'";

            var data = SQLHelper.ExecQueryDataAsDataTable(sql);
            DataTable resultDataError = data.Clone();
            var query = (from t1 in data.AsEnumerable()
                         join t2 in CuonLK2met.AsEnumerable()
                         on t1.Field<string>("Part") equals t2.Field<string>("PartId") into gr
                         from subtable in gr.DefaultIfEmpty()
                         select new
                         {
                             A = t1.Field<double>("Deduct Qty"),
                             LK2M = subtable == null ? 0 : subtable.Field<int>("Quantity"),
                             WO = t1.Field<string>("WO"),
                             Product = t1.Field<string>("Product"),
                             Customer = t1.Field<string>("Customer"),
                             Line = t1.Field<string>("Line"),
                             Material_Order = t1.Field<string>("Material Order"),
                             Machine = t1.Field<string>("Machine"),
                             Slot = t1.Field<int>("Slot"),
                             Part = t1.Field<string>("Part"),
                             UPN = t1.Field<string>("UPN"),
                             Deduct_Qty = t1.Field<double>("Deduct Qty") - (subtable == null ? 0 : subtable.Field<int>("Quantity")), // trừ đi giá trị tại cuộn 2 mét
                             Update_Date = t1.Field<DateTime>("Update Date")
                         }).ToList().Where(w => w.Deduct_Qty <= 0).ToList();

            foreach (var item in query)
            {
                resultDataError.Rows.Add(
                    item.WO,
                    item.Product,
                    item.Customer,
                    item.Line,
                    item.Material_Order,
                    item.Machine,
                    item.Slot,
                    item.Part,
                    item.UPN,
                    item.Deduct_Qty,
                    item.Update_Date);
            }
            return resultDataError;
        }

        internal static bool CheckWOFinish(string wO, string line)
        {
            SQLHelper.ConnectString(new UMESConfig());
            var list = SQLHelper.ExecProcedureData<MaterialOrderItem>("DX_FindMaterialOrderItem", new { LineID = line }).FirstOrDefault();
            if (list == null || wO != list.PRODUCTION_ORDER_ID) return true;
            return false;
        }
    }
}
