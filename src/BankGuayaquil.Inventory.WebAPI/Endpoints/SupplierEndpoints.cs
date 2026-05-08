using BankGuayaquil.Inventory.Application.DTOs;
using BankGuayaquil.Inventory.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace BankGuayaquil.Inventory.WebAPI.Endpoints;

public static class SupplierEndpoints
{
    public static void MapSupplierEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/suppliers")
                       .WithTags("Suppliers")
                       .RequireAuthorization();

        group.MapGet("/", async (ISupplierService supplierService) =>
        {
            var result = await supplierService.GetSuppliersAsync();
            return Results.Ok(result.Value);
        });

        group.MapPost("/", async ([FromBody] CreateSupplierRequest request, ISupplierService supplierService) =>
        {
            var result = await supplierService.CreateSupplierAsync(request);
            return Results.Created($"/api/suppliers/{result.Value}", result.Value);
        }).RequireAuthorization(policy => policy.RequireRole("Admin"));

        group.MapDelete("/{id:guid}", async (Guid id, ISupplierService supplierService) =>
        {
            var result = await supplierService.DeleteSupplierAsync(id);
            if (result.IsSuccess) return Results.NoContent();
            
            return result.Error.Code == "Supplier.NotFound" 
                ? Results.NotFound(result.Error) 
                : Results.BadRequest(result.Error);
        }).RequireAuthorization(policy => policy.RequireRole("Admin"));
    }
}
