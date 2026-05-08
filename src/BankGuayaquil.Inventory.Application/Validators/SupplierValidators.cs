using FluentValidation;
using BankGuayaquil.Inventory.Application.DTOs;

namespace BankGuayaquil.Inventory.Application.Validators;

public class CreateSupplierRequestValidator : AbstractValidator<CreateSupplierRequest>
{
    public CreateSupplierRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
    }
}
