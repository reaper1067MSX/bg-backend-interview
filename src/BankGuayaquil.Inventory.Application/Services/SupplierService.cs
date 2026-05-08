using BankGuayaquil.Inventory.Application.Common;
using BankGuayaquil.Inventory.Application.DTOs;
using BankGuayaquil.Inventory.Domain.Entities;
using BankGuayaquil.Inventory.Domain.Interfaces;

namespace BankGuayaquil.Inventory.Application.Services;

public class SupplierService : ISupplierService
{
    private readonly ISupplierRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public SupplierService(ISupplierRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<IEnumerable<SupplierDto>>> GetSuppliersAsync()
    {
        var suppliers = await _repository.GetAllAsync();
        return Result<IEnumerable<SupplierDto>>.Success(suppliers.Select(s => new SupplierDto(s.Id, s.Name, s.ContactInfo)));
    }

    public async Task<Result<Guid>> CreateSupplierAsync(CreateSupplierRequest request)
    {
        var supplier = new Supplier
        {
            Name = request.Name,
            ContactInfo = request.ContactInfo
        };

        await _repository.AddAsync(supplier);
        await _unitOfWork.SaveChangesAsync();
        
        return Result<Guid>.Success(supplier.Id);
    }

    public async Task<Result> DeleteSupplierAsync(Guid id)
    {
        var supplier = await _repository.GetByIdAsync(id);
        if (supplier == null) return Result.Failure(new Error("Supplier.NotFound", "Supplier not found"));

        if (supplier.Products.Any())
        {
            return Result.Failure(new Error("Supplier.HasProducts", "No se puede eliminar el proveedor porque tiene productos asociados en el inventario."));
        }

        _repository.Delete(supplier);
        await _unitOfWork.SaveChangesAsync();
        return Result.Success();
    }
}
