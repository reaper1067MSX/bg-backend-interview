using BankGuayaquil.Inventory.Domain.Common;

namespace BankGuayaquil.Inventory.Domain.Entities;

public class Product : AuditableEntity
{
    public string SKU { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    
    // Relación Multiproveedor (N:M)
    public ICollection<ProductSupplier> Suppliers { get; set; } = new List<ProductSupplier>();
}
