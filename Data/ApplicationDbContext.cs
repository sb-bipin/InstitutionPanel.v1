using InstitutionPanel.Models.Common;
using InstitutionPanel.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace InstitutionPanel.Data
{
    public class ApplicationDbContext
        : IdentityDbContext<ApplicationUser, IdentityRole, string>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IHttpContextAccessor httpContextAccessor)
            : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>(b =>
            {
                b.Property(u => u.Email).IsRequired();
                b.Property(u => u.NormalizedEmail).IsRequired();
                b.Property(u => u.CreatedAt).IsRequired();
                b.Property(u => u.CreatedBy).IsRequired();
            });

            builder.Entity<Institution>(entity =>
            {
                entity.OwnsOne(i => i.Email, cb =>
                {
                    cb.Property(e => e.Value)
                      .HasColumnName("Email")
                      .HasMaxLength(150)
                      .IsRequired(false);
                });

                entity.OwnsOne(i => i.ContactNo, cb =>
                {
                    cb.Property(c => c.Value)
                      .HasColumnName("ContactNo")
                      .HasMaxLength(9)
                      .IsRequired(false);
                });

                entity.OwnsOne(i => i.PhoneNo, cb =>
                {
                    cb.Property(p => p.Value)
                      .HasColumnName("PhoneNo")
                      .HasMaxLength(10)
                      .IsRequired(false);
                });
            });
        }

        public DbSet<Institution> Institutions { get; set; }


        public override int SaveChanges()
        {
            ApplyAuditInformation();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(
            CancellationToken cancellationToken = default)
        {
            ApplyAuditInformation();
            return await base.SaveChangesAsync(cancellationToken);
        }

        private void ApplyAuditInformation()
        {
            var entries = ChangeTracker.Entries<CommonEntities>();

            var userName =
                _httpContextAccessor.HttpContext?
                .User?
                .FindFirstValue(ClaimTypes.Name)
                ?? "System";

            var now = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedAt = now;
                    entry.Entity.CreatedBy = userName;
                }

                if (entry.State == EntityState.Modified)
                {
                    entry.Entity.UpdatedAt = now;
                    entry.Entity.UpdatedBy = userName;
                }
            }

        }
    }
}
