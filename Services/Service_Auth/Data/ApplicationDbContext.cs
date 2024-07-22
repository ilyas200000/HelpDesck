using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Service_Auth.Models;
using Service_Auth.Models.Identity;

namespace Service_Auth.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, int, IdentityUserClaim<int>, ApplicationUserRole, IdentityUserLogin<int>, IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<EntiteSiege> ENTITE_SIEGE { get; set; }
        public DbSet<Droit> DROITS { get; set; }
      
        public DbSet<DROIT_ROLE> DROIT_ROLE { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>(b =>
            {
                b.HasMany(e => e.UserRoles)
                    .WithOne(e => e.User)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();

                b.HasOne(u => u.EntiteSiege)
                    .WithMany()
                    .HasForeignKey(u => u.entite_id);
            });

            builder.Entity<ApplicationRole>(b =>
            {
                b.HasMany(e => e.UserRoles)
                    .WithOne(e => e.Role)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();

                b.HasMany(e => e.RoleClaims)
                    .WithOne(e => e.Role)
                    .HasForeignKey(rc => rc.RoleId)
                    .IsRequired();

             


            });

            builder.Entity<ApplicationUserRole>(b =>
            {
                b.HasOne(e => e.User)
                    .WithMany(e => e.UserRoles)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();

                b.HasOne(e => e.Role)
                    .WithMany(e => e.UserRoles)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();
            });

            builder.Entity<ApplicationRoleClaim>(b =>
            {
                b.HasOne(e => e.Role)
                    .WithMany()
                    .HasForeignKey(rc => rc.RoleId)
                    .IsRequired();
            });

            builder.Entity<EntiteSiege>(entity =>
            {
                entity.HasKey(e => e.ENTT_SG_ID);
            });

            builder.Entity<Droit>(entity =>
            {
                entity.HasKey(e => e.DRT_ID);
            });

             
             builder.Entity<DROIT_ROLE>(entity =>
             {
                entity.HasKey(e => e.DROIT_ROLE_ID);
            
                 entity.HasOne(e => e.Droit)
                     .WithMany()
                     .HasForeignKey(e => e.DRT_ID)
                     .IsRequired();
            
                 entity.HasOne(e => e.Role)
                     .WithMany()
                     .HasForeignKey(e => e.RoleId)
                     .IsRequired();
             });
        }
    }
}
