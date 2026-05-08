using BankGuayaquil.Inventory.Domain.Entities;
using BankGuayaquil.Inventory.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BankGuayaquil.Inventory.Infrastructure.Persistence.Repositories;

public class SupplierRepository : ISupplierRepository
{
    private readonly InventoryDbContext _context;

    public SupplierRepository(InventoryDbContext context)
    {
        _context = context;
    }

    public async Task<Supplier?> GetByIdAsync(Guid id)
    {
        return await _context.Suppliers
            .Include(s => s.Products)
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<IEnumerable<Supplier>> GetAllAsync()
    {
        return await _context.Suppliers.OrderBy(s => s.Name).ToListAsync();
    }

    public async Task AddAsync(Supplier supplier)
    {
        await _context.Suppliers.AddAsync(supplier);
    }

    public void Update(Supplier supplier)
    {
        _context.Suppliers.Update(supplier);
    }

    public void Delete(Supplier supplier)
    {
        _context.Suppliers.Remove(supplier);
    }
}
