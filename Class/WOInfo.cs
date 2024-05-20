using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alarmlines.Class
{
    public class WOInfo
    {
        public string WO { get; set; }
        public string Line { get; set; }
        public bool IS_VERIFY { get; set; }
        public double QUANTITY { get; set; }
    }
    public class WODetailInfo
    {
        public string WO { get; set; }
        public string ProductId { get; set; }
        public string Line { get; set; }
        public string MaterialOrderId { get; set; }
        public string Machine { get; set; }
        public string Slot { get; set; }
        public string Part { get; set; }
        public string UPN { get; set; }
        public string Remark { get; set; }
        public string TimeStart { get; set; }
        public DateTime Upd_Time { get; set; }
    }

    public class MaterialOrderItem
    {
        public string PRODUCTION_ORDER_ID { get; set; }
    }
}
