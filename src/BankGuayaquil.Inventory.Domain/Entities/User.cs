using BankGuayaquil.Inventory.Domain.Common;

namespace BankGuayaquil.Inventory.Domain.Entities;

public class User : AuditableEntity
{
    public string Username { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string Role { get; set; } = "User";
}
