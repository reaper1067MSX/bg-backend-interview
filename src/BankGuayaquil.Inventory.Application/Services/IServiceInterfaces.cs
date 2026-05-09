using BankGuayaquil.Inventory.Application.Common;
using BankGuayaquil.Inventory.Application.DTOs;
using BankGuayaquil.Inventory.Domain.Interfaces;

namespace BankGuayaquil.Inventory.Application.Services;

public interface IProductService
{
    Task<Result<IEnumerable<ProductResponse>>> GetProductsAsync();
    Task<Result<ProductResponse>> GetProductByIdAsync(Guid id);
    Task<Result<Guid>> CreateProductAsync(ProductRequest request);
    Task<Result> UpdateProductAsync(Guid id, ProductRequest request);
    Task<Result> DeleteProductAsync(Guid id);
    Task<Result> UpdateStockAsync(Guid providerId, int newStock);
    Task<Result> UpdateMinThresholdStockAsync(Guid productId, int newMinThreshold);
}

public interface ISupplierService
{
    Task<Result<IEnumerable<SupplierDto>>> GetSuppliersAsync();
    Task<Result<Guid>> CreateSupplierAsync(CreateSupplierRequest request);
    Task<Result> DeleteSupplierAsync(Guid id);
}

public interface IAuthService
{
    Task<Result<AuthResponse>> LoginAsync(LoginRequest request);
}
// Implementaciones se crearán en el siguiente paso
