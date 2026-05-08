using BankGuayaquil.Inventory.Domain.Entities;

namespace BankGuayaquil.Inventory.Domain.Interfaces;

public interface IProductRepository
{
    Task<Product?> GetByIdAsync(Guid id);
    Task<IEnumerable<Product>> GetAllAsync();
    Task AddAsync(Product product);
    void Update(Product product);
    void Delete(Product product);
    
    // Operaciones específicas de proveedores
    Task<ProductSupplier?> GetProviderByIdAsync(Guid id);
}
