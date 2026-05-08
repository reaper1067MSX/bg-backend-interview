using BankGuayaquil.Inventory.Domain.Entities;

namespace BankGuayaquil.Inventory.Tests.Mothers;

public static class ProductMother
{
    public static Product Standard()
    {
        return new Product
        {
            Id = Guid.NewGuid(),
            SKU = "MON-50-4K",
            Name = "Monitor 50 pulgadas 4K",
            Description = "Monitor de prueba",
            Suppliers = new List<ProductSupplier>
            {
                new ProductSupplier { Id = Guid.NewGuid(), SupplierId = Guid.NewGuid(), CurrentPrice = 250, Stock = 10 },
                new ProductSupplier { Id = Guid.NewGuid(), SupplierId = Guid.NewGuid(), CurrentPrice = 300, Stock = 5 }
            }
        };
    }

    public static Product WithNoStock()
    {
        var p = Standard();
        foreach (var prov in p.Suppliers) prov.Stock = 0;
        return p;
    }
}
