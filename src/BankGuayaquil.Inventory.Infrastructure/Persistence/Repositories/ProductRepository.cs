using BankGuayaquil.Inventory.Domain.Entities;
using BankGuayaquil.Inventory.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BankGuayaquil.Inventory.Infrastructure.Persistence.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly InventoryDbContext _context;

    public ProductRepository(InventoryDbContext context)
    {
        _context = context;
    }

    public async Task<Product?> GetByIdAsync(Guid id)
    {
        return await _context.Products
            .Include(p => p.Suppliers)
            .ThenInclude(ps => ps.Supplier)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        return await _context.Products
            .Include(p => p.Suppliers)
            .ThenInclude(ps => ps.Supplier)
            .ToListAsync();
    }

    public async Task AddAsync(Product product)
    {
        await _context.Products.AddAsync(product);
    }

    public void Update(Product product)
    {
        _context.Products.Update(product);
    }

    public void Delete(Product product)
    {
        _context.Products.Remove(product);
    }

    public async Task<ProductSupplier?> GetProviderByIdAsync(Guid id)
    {
        return await _context.ProductSuppliers.FindAsync(id);
    }
}
