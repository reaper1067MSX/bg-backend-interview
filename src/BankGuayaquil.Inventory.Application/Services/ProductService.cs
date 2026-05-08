using BankGuayaquil.Inventory.Application.Common;
using BankGuayaquil.Inventory.Application.DTOs;
using BankGuayaquil.Inventory.Domain.Entities;
using BankGuayaquil.Inventory.Domain.Interfaces;

namespace BankGuayaquil.Inventory.Application.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public ProductService(IProductRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<IEnumerable<ProductResponse>>> GetProductsAsync()
    {
        var products = await _repository.GetAllAsync();
        var response = products.Select(p => MapToResponse(p));
        return Result<IEnumerable<ProductResponse>>.Success(response);
    }

    public async Task<Result<ProductResponse>> GetProductByIdAsync(Guid id)
    {
        var product = await _repository.GetByIdAsync(id);
        if (product == null) return Result<ProductResponse>.Failure(new Error("Product.NotFound", "Product not found"));
        
        return Result<ProductResponse>.Success(MapToResponse(product));
    }

    public async Task<Result<Guid>> CreateProductAsync(ProductRequest request)
    {
        var product = new Product
        {
            SKU = request.SKU,
            Name = request.Name,
            Description = request.Description,
            Suppliers = request.Suppliers.Select(p => new ProductSupplier
            {
                SupplierId = p.SupplierId,
                CurrentPrice = p.CurrentPrice,
                Stock = p.Stock
            }).ToList()
        };

        await _repository.AddAsync(product);
        await _unitOfWork.SaveChangesAsync();
        
        return Result<Guid>.Success(product.Id);
    }

    public async Task<Result> UpdateProductAsync(Guid id, ProductRequest request)
    {
        var product = await _repository.GetByIdAsync(id);
        if (product == null) return Result.Failure(new Error("Product.NotFound", "Product not found"));

        // Validar duplicados en la solicitud
        if (request.Suppliers.GroupBy(s => s.SupplierId).Any(g => g.Count() > 1))
        {
            return Result.Failure(new Error("Product.DuplicateSupplier", "No se puede asignar el mismo proveedor múltiples veces al mismo producto."));
        }

        product.SKU = request.SKU;
        product.Name = request.Name;
        product.Description = request.Description;

        // --- Sincronización precisa de la colección ---
        
        // 1. Identificar entidades a eliminar (existen en DB pero su ID no viene en el request)
        var requestIds = request.Suppliers.Where(s => s.Id.HasValue).Select(s => s.Id!.Value).ToList();
        var toRemove = product.Suppliers.Where(ps => !requestIds.Contains(ps.Id)).ToList();
        foreach (var item in toRemove)
        {
            product.Suppliers.Remove(item);
        }

        // 2. Procesar cada item del request
        foreach (var s in request.Suppliers)
        {
            if (s.Id.HasValue)
            {
                // Es un registro existente que queremos actualizar
                var existing = product.Suppliers.FirstOrDefault(ps => ps.Id == s.Id.Value);
                if (existing != null)
                {
                    existing.SupplierId = s.SupplierId;
                    existing.CurrentPrice = s.CurrentPrice;
                    existing.Stock = s.Stock;
                }
            }
            else
            {
                // Es un registro nuevo
                product.Suppliers.Add(new ProductSupplier
                {
                    SupplierId = s.SupplierId,
                    CurrentPrice = s.CurrentPrice,
                    Stock = s.Stock
                });
            }
        }

        await _unitOfWork.SaveChangesAsync();
        
        return Result.Success();
    }

    public async Task<Result> DeleteProductAsync(Guid id)
    {
        var product = await _repository.GetByIdAsync(id);
        if (product == null) return Result.Failure(new Error("Product.NotFound", "Product not found"));

        _repository.Delete(product);
        await _unitOfWork.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<Result> UpdateStockAsync(Guid providerId, int newStock)
    {
        var provider = await _repository.GetProviderByIdAsync(providerId);
        if (provider == null) return Result.Failure(new Error("Provider.NotFound", "Provider not found"));

        provider.Stock = newStock;
        await _unitOfWork.SaveChangesAsync(); 
        
        return Result.Success();
    }

    private ProductResponse MapToResponse(Product p) => new(
        p.Id,
        p.SKU,
        p.Name,
        p.Description,
        p.Suppliers.Select(ps => new ProductSupplierDto(ps.Id, ps.SupplierId, ps.Supplier?.Name, ps.CurrentPrice, ps.Stock)).ToList(),
        p.Suppliers.Sum(ps => ps.Stock)
    );
}
