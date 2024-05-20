namespace Alarmlines
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Tbl_PDAErrorVersion
    {
        public int Id { get; set; }

        [StringLength(50)]
        public string AppName { get; set; }

        [StringLength(10)]
        public string VersionCode { get; set; }

        public DateTime? CreateDate { get; set; }

        [StringLength(500)]
        public string Path { get; set; }

        [StringLength(500)]
        public string Descriptions { get; set; }
    }
}
