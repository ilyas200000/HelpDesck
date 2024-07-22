using Microsoft.EntityFrameworkCore;
using Service_Incidents.Models;

namespace Service_Incidents.Data
{
    public class IncidentsDbContext : DbContext
    {
        public IncidentsDbContext(DbContextOptions<IncidentsDbContext> options ) : base( options )
        {
        }
        public virtual DbSet<Incident> Incidents { get; set; }
        public virtual DbSet<Types> Types { get; set; }
        public virtual DbSet<Statut> Statuts { get; set; }
        public virtual DbSet<Priorite> Priorites { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<IncidentHistory> IncidentHistories { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Incident>(entity =>
            {
                entity.HasKey(e => e.INCD_ID);
                entity.Property(e => e.INCD_DESC).HasMaxLength(255);
                entity.Property(e => e.agn_code).HasMaxLength(255);
                entity.Property(e => e.incd_audit).HasMaxLength(255);
                entity.Property(e => e.Phone).HasMaxLength(20);
                // Configure other property lengths and types as needed

                // Configure relationships
                entity.HasOne<Types>()
                      .WithMany()
                      .HasForeignKey(e => e.INCD_TYPE_ID);


                entity.HasOne<Statut>()   
                      .WithMany()
                      .HasForeignKey(e => e.INCD_STAT_ID);

                entity.HasOne<Priorite>()
                      .WithMany()
                      .HasForeignKey(e => e.INCD_PRIO_ID);
            });

            modelBuilder.Entity<Types>(entity =>
            {
                entity.HasKey(e => e.INCD_TYPE_ID);
                entity.Property(e => e.TYPE_DESC).HasMaxLength(255);
                
                entity.HasOne(e => e.Category)
                      .WithMany(c=>c.Types)
                      .HasForeignKey(e => e.CategoryID);
            });

            modelBuilder.Entity<Statut>(entity =>
            {
                entity.HasKey(e => e.INCD_STAT_ID);
                entity.Property(e => e.STAT_DESC).HasMaxLength(255);
            });

            modelBuilder.Entity<Priorite>(entity =>
            {
                entity.HasKey(e => e.INCD_PRIO_ID);
                entity.Property(e => e.PRIO_DESC).HasMaxLength(255);
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(e => e.CategoryID);
                entity.Property(e => e.CategoryName).HasMaxLength(255);
            });

            modelBuilder.Entity<IncidentHistory>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.ChangeDate).IsRequired();
                entity.Property(e => e.ChangedBy).HasMaxLength(255);

                entity.HasOne(e => e.Incident)
                      .WithMany()
                      .HasForeignKey(e => e.IncidentId);

                entity.HasOne(e => e.OldStatus)
                      .WithMany()
                      .HasForeignKey(e => e.OldStatusId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.NewStatus)
                      .WithMany()
                      .HasForeignKey(e => e.NewStatusId)
                      .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}