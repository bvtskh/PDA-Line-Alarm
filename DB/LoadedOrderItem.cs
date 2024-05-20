namespace Alarmlines.DB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("LoadedOrderItem")]
    public partial class LoadedOrderItem
    {
        [Key]
        public string LINE_ID { get; set; }
        public string PRODUCTION_ORDER_ID { get; set; }
        public string PART_ID { get; set; }
        public string MACHINE_ID { get; set; }
        public int MACHINE_SLOT { get; set; }
        public DateTime? UPD_TIME { get; set; }
        public string PRODUCT_ID { get; set; }
        public bool IS_VERIFIED { get; set; }

    }
}