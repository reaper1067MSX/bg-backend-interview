using BankGuayaquil.Inventory.Domain.Entities;
using BankGuayaquil.Inventory.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BankGuayaquil.Inventory.Infrastructure.Persistence;

public static class DataSeeder
{
    public static async Task SeedAsync(InventoryDbContext context)
    {
        if (await context.Products.AnyAsync()) return;

        var adminUser = new User
        {
            Username = "admin",
            PasswordHash = "password123", 
            Role = "Admin"
        };

        var provA = new Supplier { Name = "Proveedor A", ContactInfo = "contacto@a.com" };
        var provB = new Supplier { Name = "Proveedor B", ContactInfo = "contacto@b.com" };
        var provN = new Supplier { Name = "Proveedor N", ContactInfo = "contacto@n.com" };

        var products = new List<Product>
        {
            new Product
            {
                SKU = "MON-50-4K",
                Name = "Monitor 50 pulgadas 4K",
                Description = "Monitor de alta resolución para diseño y gaming",
                Suppliers = new List<ProductSupplier>
                {
                    new ProductSupplier { Supplier = provA, CurrentPrice = 250, Stock = 10 },
                    new ProductSupplier { Supplier = provB, CurrentPrice = 300, Stock = 5 },
                    new ProductSupplier { Supplier = provN, CurrentPrice = 200, Stock = 20 }
                }
            },
            new Product
            {
                SKU = "SND-20000",
                Name = "Equipo de Sonido 20000",
                Description = "Sistema de audio profesional",
                Suppliers = new List<ProductSupplier>
                {
                    new ProductSupplier { Supplier = provA, CurrentPrice = 150, Stock = 15 },
                    new ProductSupplier { Supplier = provB, CurrentPrice = 200, Stock = 8 },
                    new ProductSupplier { Supplier = provN, CurrentPrice = 100, Stock = 30 }
                }
            }
        };

        await context.Users.AddAsync(adminUser);
        await context.Suppliers.AddRangeAsync(provA, provB, provN);
        await context.Products.AddRangeAsync(products);
        await context.SaveChangesAsync();
    }
}
