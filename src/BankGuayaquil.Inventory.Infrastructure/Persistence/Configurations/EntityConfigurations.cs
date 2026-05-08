using BankGuayaquil.Inventory.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BankGuayaquil.Inventory.Infrastructure.Persistence.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(p => p.SKU).IsRequired().HasMaxLength(20);
        builder.HasIndex(p => p.SKU).IsUnique();
        builder.Property(p => p.Name).IsRequired().HasMaxLength(100);
        
        builder.HasMany(p => p.Suppliers)
               .WithOne(ps => ps.Product)
               .HasForeignKey(ps => ps.ProductId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}

public class SupplierConfiguration : IEntityTypeConfiguration<Supplier>
{
    public void Configure(EntityTypeBuilder<Supplier> builder)
    {
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Name).IsRequired().HasMaxLength(100);
        builder.HasIndex(s => s.Name).IsUnique(); // Nombres únicos de proveedores
    }
}

public class ProductSupplierConfiguration : IEntityTypeConfiguration<ProductSupplier>
{
    public void Configure(EntityTypeBuilder<ProductSupplier> builder)
    {
        builder.HasKey(ps => ps.Id);
        
        builder.HasOne(ps => ps.Supplier)
               .WithMany(s => s.Products)
               .HasForeignKey(ps => ps.SupplierId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.Property(ps => ps.CurrentPrice).HasPrecision(18, 2);
        
        // Optimistic Concurrency usando xmin (Postgres)
        builder.Property<uint>("Version").IsRowVersion();
    }
}

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);
        builder.Property(u => u.Username).IsRequired().HasMaxLength(50);
        builder.HasIndex(u => u.Username).IsUnique();
    }
}
