using BankGuayaquil.Inventory.Domain.Common;

namespace BankGuayaquil.Inventory.Domain.Entities;

public class Supplier : AuditableEntity
{
    public string Name { get; set; } = string.Empty;
    public string ContactInfo { get; set; } = string.Empty;
    
    // Navigation
    public ICollection<ProductSupplier> Products { get; set; } = new List<ProductSupplier>();
}
