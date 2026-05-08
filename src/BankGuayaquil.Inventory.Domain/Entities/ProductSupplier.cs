using BankGuayaquil.Inventory.Domain.Common;

namespace BankGuayaquil.Inventory.Domain.Entities;

public class ProductSupplier : AuditableEntity
{
    public Guid ProductId { get; set; }
    public Guid SupplierId { get; set; }
    
    public decimal CurrentPrice { get; set; }
    public int Stock { get; set; }
    
    // Navigation properties
    public Product? Product { get; set; }
    public Supplier? Supplier { get; set; }
}
