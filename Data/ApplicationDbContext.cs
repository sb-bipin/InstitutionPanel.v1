using InstitutionPanel.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace InstitutionPanel.Data
{
    public class ApplicationDbContext
        : IdentityDbContext<ApplicationUser, IdentityRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
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


    }
}
