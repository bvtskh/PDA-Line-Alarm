using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace Alarmlines.DB
{
    public partial class DXContext : DbContext
    {
        public DXContext()
            : base("name=DXConnection")
        {
        }

        public virtual DbSet<Location> Locations { get; set; }
        public virtual DbSet<LoadedOrderItem> LoadedOrderItems { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
