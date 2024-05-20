namespace Alarmlines.DB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Location")]
    public partial class Location
    {
        [Key]
        [StringLength(50)]
        public string LineId { get; set; }

        [Required]
        [StringLength(50)]
        public string LocationId { get; set; }

        [StringLength(50)]
        public string GroupID { get; set; }
    }
}
