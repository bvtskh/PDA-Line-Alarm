namespace Alarmlines
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class DBContext : DbContext
    {
        public DBContext()
            : base("data source=172.28.6.96;initial catalog=IOT2021;persist security info=True;user id=sa;password=umc@123;MultipleActiveResultSets=True;App=EntityFramework")
        {
        }

        public virtual DbSet<PDA_ErrorHistory> PDA_ErrorHistory { get; set; }
        public virtual DbSet<PDA_ErrorHistoryBk> PDA_ErrorHistoryBk { get; set; }
        public virtual DbSet<Tbl_PDADefineError> Tbl_PDADefineError { get; set; }
        public virtual DbSet<Tbl_PDAErrorVersion> Tbl_PDAErrorVersion { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PDA_ErrorHistory>()
                .Property(e => e.Model)
                .IsUnicode(false);

            modelBuilder.Entity<PDA_ErrorHistoryBk>()
                .Property(e => e.Model)
                .IsUnicode(false);

            modelBuilder.Entity<Tbl_PDADefineError>()
                .Property(e => e.PDAErrorContent)
                .IsUnicode(false);

            modelBuilder.Entity<Tbl_PDADefineError>()
                .Property(e => e.ErrorLevel)
                .IsUnicode(false);

            modelBuilder.Entity<Tbl_PDAErrorVersion>()
                .Property(e => e.AppName)
                .IsUnicode(false);

            modelBuilder.Entity<Tbl_PDAErrorVersion>()
                .Property(e => e.VersionCode)
                .IsUnicode(false);

            modelBuilder.Entity<Tbl_PDAErrorVersion>()
                .Property(e => e.Path)
                .IsUnicode(false);
        }
    }
}
