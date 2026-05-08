using FluentValidation;
using BankGuayaquil.Inventory.Application.DTOs;

namespace BankGuayaquil.Inventory.Application.Validators;

public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.Username).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Password).NotEmpty().MinimumLength(6);
    }
}

public class ProductRequestValidator : AbstractValidator<ProductRequest>
{
    public ProductRequestValidator()
    {
        RuleFor(x => x.SKU).NotEmpty().MaximumLength(20);
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Suppliers).NotEmpty().WithMessage("At least one supplier is required.");
        RuleForEach(x => x.Suppliers).SetValidator(new ProductSupplierDtoValidator());
    }
}

public class ProductSupplierDtoValidator : AbstractValidator<ProductSupplierDto>
{
    public ProductSupplierDtoValidator()
    {
        RuleFor(x => x.SupplierId).NotEmpty();
        RuleFor(x => x.CurrentPrice).GreaterThan(0);
        RuleFor(x => x.Stock).GreaterThanOrEqualTo(0);
    }
}
