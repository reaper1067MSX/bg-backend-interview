using BankGuayaquil.Inventory.Domain.Entities;

namespace BankGuayaquil.Inventory.Domain.Interfaces;

public interface ISupplierRepository
{
    Task<Supplier?> GetByIdAsync(Guid id);
    Task<IEnumerable<Supplier>> GetAllAsync();
    Task AddAsync(Supplier supplier);
    void Update(Supplier supplier);
    void Delete(Supplier supplier);
}
