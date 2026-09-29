using CWVehicleManagerAPI.Models.Domain;
using CWVehicleManagerAPI.Utils;
using Microsoft.EntityFrameworkCore;

namespace CWVehicleManagerAPI.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Vehicle> Vehicles => Set<Vehicle>();
    public DbSet<Manufacturer> Manufacturers => Set<Manufacturer>();
    public DbSet<VehicleCategory> VehicleCategories => Set<VehicleCategory>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Manufacturer>(entity =>
        {
            entity.ToTable("Manufacturers");
            entity.Property(m => m.Name).IsRequired().HasMaxLength(100);
            entity.HasIndex(m => m.Name).IsUnique();
            entity.HasData(InitData.Manufacturers());
        });

        modelBuilder.Entity<VehicleCategory>(entity =>
        {
            entity.ToTable("VehicleCategories", table =>
            {
                table.HasCheckConstraint("CK_VehicleCategories_MinWeightKg", "[MinWeightKg] >= 0");
                table.HasCheckConstraint("CK_VehicleCategories_Range", "[MaxWeightKg] IS NULL OR [MaxWeightKg] > [MinWeightKg]");
            });
            entity.Property(c => c.Name).IsRequired().HasMaxLength(CategoryValidator.MaxNameLength);
            entity.HasIndex(c => c.Name).IsUnique();
            entity.Property(c => c.MinWeightKg).HasPrecision(9, 2);
            entity.Property(c => c.MaxWeightKg).HasPrecision(9, 2);
            entity.Property(c => c.IconKey).IsRequired().HasMaxLength(50);
            entity.HasData(InitData.Categories());
        });

        modelBuilder.Entity<Vehicle>(entity =>
        {
            entity.ToTable("Vehicles", table =>
            {
                table.HasCheckConstraint("CK_Vehicles_WeightKg", "[WeightKg] > 0");
                table.HasCheckConstraint("CK_Vehicles_YearOfManufacture", $"[YearOfManufacture] >= {VehicleValidator.EarliestYear}");
            });
            entity.Property(v => v.OwnerName).IsRequired().HasMaxLength(VehicleValidator.MaxOwnerNameLength);
            entity.Property(v => v.WeightKg).HasPrecision(9, 2);
            entity.HasOne(v => v.Manufacturer)
                .WithMany()
                .HasForeignKey(v => v.ManufacturerId)
                .OnDelete(DeleteBehavior.Restrict);

            // Indexes for the sorting.
            entity.HasIndex(v => v.OwnerName);
            entity.HasIndex(v => v.YearOfManufacture);
            entity.HasIndex(v => v.WeightKg);
        });
    }
}