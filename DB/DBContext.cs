namespace Alarmlines
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class DBContext : DbContext
    {
        public DBContext()
            : base("name=DBContext")
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
