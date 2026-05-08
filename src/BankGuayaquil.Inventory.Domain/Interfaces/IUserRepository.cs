using BankGuayaquil.Inventory.Domain.Entities;

namespace BankGuayaquil.Inventory.Domain.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByUsernameAsync(string username);
    Task AddAsync(User user);
}
