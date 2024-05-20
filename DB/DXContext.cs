using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace Alarmlines.DB
{
    public partial class DXContext : DbContext
    {
        public DXContext()
            : base("data source=172.28.10.17;initial catalog=SMT;user id=sa;password=umc@2019;MultipleActiveResultSets=True;App=EntityFramework")
        {
        }

        public virtual DbSet<Location> Locations { get; set; }
        public virtual DbSet<LoadedOrderItem> LoadedOrderItems { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
