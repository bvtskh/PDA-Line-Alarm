namespace Alarmlines
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Tbl_PDADefineError
    {
        public int Id { get; set; }

        [StringLength(100)]
        public string PDAErrorContent { get; set; }

        [StringLength(200)]
        public string VNContent { get; set; }

        [StringLength(20)]
        public string ErrorLevel { get; set; }
    }
}
