namespace BankGuayaquil.Inventory.Application.DTOs;

public record LoginRequest(string Username, string Password);
public record AuthResponse(string Token, string Username, string Role);

public record SupplierDto(Guid Id, string Name, string ContactInfo);
public record CreateSupplierRequest(string Name, string ContactInfo);

public record ProductSupplierDto(
    Guid? Id,
    Guid SupplierId,
    string? SupplierName, // Solo para lectura en el GET
    decimal CurrentPrice,
    int Stock);

public record ProductRequest(
    string SKU,
    string Name,
    string Description,
    int? MinStockThreshold,
    List<ProductSupplierDto> Suppliers);

public record ProductResponse(
    Guid Id,
    string SKU,
    string Name,
    string Description,
    List<ProductSupplierDto> Suppliers,
    int? MinStockThreshold,
    int TotalStock);
    
public record UpdateStockRequest(int NewStock);

public record UpdateMinThresholdStock(int NewMinThreshold);
