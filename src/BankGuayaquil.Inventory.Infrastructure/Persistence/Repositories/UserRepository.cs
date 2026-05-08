using BankGuayaquil.Inventory.Domain.Entities;
using BankGuayaquil.Inventory.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BankGuayaquil.Inventory.Infrastructure.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly InventoryDbContext _context;

    public UserRepository(InventoryDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
    }

    public async Task AddAsync(User user)
    {
        await _context.Users.AddAsync(user);
    }
}
