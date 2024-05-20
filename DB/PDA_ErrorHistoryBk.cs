namespace Alarmlines
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PDA_ErrorHistoryBk
    {
        public long id { get; set; }

        public DateTime? ErrorTime { get; set; }

        [StringLength(50)]
        public string Line { get; set; }

        [StringLength(50)]
        public string Model { get; set; }

        [StringLength(50)]
        public string WO { get; set; }

        [StringLength(100)]
        public string PartCode { get; set; }

        [StringLength(200)]
        public string ErrorContent { get; set; }

        [StringLength(50)]
        public string OperatorCode { get; set; }

        [StringLength(50)]
        public string Customer { get; set; }

        [StringLength(50)]
        public string Location { get; set; }

        [StringLength(100)]
        public string Comment { get; set; }

        public long? IdErr { get; set; }

        public int? Slot { get; set; }

        public int? ProcessID { get; set; }
    }
}
