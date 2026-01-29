using Bakery.Domains.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Bakery.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }
        public DbSet<ProductCategory> ProductCategories => Set<ProductCategory>();
        public DbSet<Product> Products => Set<Product>();
        public DbSet<ProductImage> ProductImages => Set<ProductImage>();

        public DbSet<OpeningHours> OpeningHours => Set<OpeningHours>();
        public DbSet<OpeningHoursInterval> OpeningHoursIntervals => Set<OpeningHoursInterval>();

        public DbSet<SpecialDay> SpecialDays => Set<SpecialDay>();
        public DbSet<SpecialDayInterval> SpecialDayIntervals => Set<SpecialDayInterval>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // -----------------------------
            // ProductCategory
            // -----------------------------
            builder.Entity<ProductCategory>(entity =>
            {
                entity.ToTable("ProductCategories");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Name)
                    .HasMaxLength(80)
                    .IsRequired();

                entity.Property(e => e.SortOrder)
                    .HasDefaultValue(0);

                entity.Property(e => e.IsActive)
                    .HasDefaultValue(true);

                entity.HasIndex(e => e.Name)
                    .IsUnique();

                entity.HasMany(e => e.Products)
                    .WithOne(p => p.Category)
                    .HasForeignKey(p => p.CategoryId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // -----------------------------
            // Product
            // -----------------------------
            builder.Entity<Product>(entity =>
            {
                entity.ToTable("Products");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Name)
                    .HasMaxLength(120)
                    .IsRequired();

                entity.Property(e => e.Description)
                    .HasMaxLength(1000);

                entity.Property(e => e.Price)
                    .HasPrecision(10, 2)
                    .IsRequired();

                entity.Property(e => e.IsActive)
                    .HasDefaultValue(true);

                entity.Property(e => e.SortOrder)
                    .HasDefaultValue(0);

                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("SYSUTCDATETIME()");

                entity.Property(e => e.UpdatedAt);

                entity.HasIndex(e => new { e.CategoryId, e.SortOrder });

                entity.HasMany(e => e.Images)
                    .WithOne(i => i.Product)
                    .HasForeignKey(i => i.ProductId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // -----------------------------
            // ProductImage
            // -----------------------------
            builder.Entity<ProductImage>(entity =>
            {
                entity.ToTable("ProductImages");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.ImageUrl)
                    .HasMaxLength(400)
                    .IsRequired();

                entity.Property(e => e.AltText)
                    .HasMaxLength(140);

                entity.Property(e => e.IsPrimary)
                    .HasDefaultValue(false);

                entity.Property(e => e.SortOrder)
                    .HasDefaultValue(0);

                entity.HasIndex(e => new { e.ProductId, e.SortOrder });

                // Optional: prevent 2 primary images per product
                entity.HasIndex(e => new { e.ProductId, e.IsPrimary })
                    .HasFilter("[IsPrimary] = 1")
                    .IsUnique();
            });

            // -----------------------------
            // OpeningHours (weekly)
            // -----------------------------
            builder.Entity<OpeningHours>(entity =>
            {
                entity.ToTable("OpeningHours");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.DayOfWeek)
                    .IsRequired();

                entity.Property(e => e.IsClosed)
                    .HasDefaultValue(false);

                // Ensure only one row per weekday
                entity.HasIndex(e => e.DayOfWeek)
                    .IsUnique();

                entity.HasMany(e => e.Intervals)
                    .WithOne(i => i.OpeningHours)
                    .HasForeignKey(i => i.OpeningHoursId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // -----------------------------
            // OpeningHoursInterval (weekly)
            // -----------------------------
            builder.Entity<OpeningHoursInterval>(entity =>
            {
                entity.ToTable("OpeningHoursIntervals");

                entity.HasKey(e => e.Id);

                // TimeOnly -> SQL 'time'
                entity.Property(e => e.OpenTime)
                    .HasColumnType("time")
                    .IsRequired();

                entity.Property(e => e.CloseTime)
                    .HasColumnType("time")
                    .IsRequired();

                entity.HasIndex(e => new { e.OpeningHoursId, e.OpenTime, e.CloseTime });

                // CloseTime must be after OpenTime
                entity.ToTable(t => t.HasCheckConstraint(
                    "CK_OpeningHoursIntervals_TimeRange",
                    "[CloseTime] > [OpenTime]"
                ));
            });

            // -----------------------------
            // SpecialDay (exceptions)
            // -----------------------------
            builder.Entity<SpecialDay>(entity =>
            {
                entity.ToTable("SpecialDays");

                entity.HasKey(e => e.Id);

                // DateOnly -> SQL 'date'
                entity.Property(e => e.Date)
                    .HasColumnType("date")
                    .IsRequired();

                entity.Property(e => e.IsClosedAllDay)
                    .HasDefaultValue(false);

                entity.Property(e => e.Note)
                    .HasMaxLength(200);

                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("SYSUTCDATETIME()");

                entity.Property(e => e.UpdatedAt);

                // Only one exception definition per date
                entity.HasIndex(e => e.Date)
                    .IsUnique();

                entity.HasMany(e => e.Intervals)
                    .WithOne(i => i.SpecialDay)
                    .HasForeignKey(i => i.SpecialDayId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // -----------------------------
            // SpecialDayInterval (exceptions)
            // -----------------------------
            builder.Entity<SpecialDayInterval>(entity =>
            {
                entity.ToTable("SpecialDayIntervals");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.OpenTime)
                    .HasColumnType("time")
                    .IsRequired();

                entity.Property(e => e.CloseTime)
                    .HasColumnType("time")
                    .IsRequired();

                entity.HasIndex(e => new { e.SpecialDayId, e.OpenTime, e.CloseTime });

                entity.ToTable(t => t.HasCheckConstraint(
                    "CK_SpecialDayIntervals_TimeRange",
                    "[CloseTime] > [OpenTime]"
                ));
            });
        }

    }
}
