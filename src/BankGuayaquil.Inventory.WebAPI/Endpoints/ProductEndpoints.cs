using BankGuayaquil.Inventory.Application.DTOs;
using BankGuayaquil.Inventory.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace BankGuayaquil.Inventory.WebAPI.Endpoints;

public static class ProductEndpoints
{
    public static void MapProductEndpoints(this IEndpointRouteBuilder app)
    {
        // Protegemos TODO el grupo requiriendo autenticación
        var group = app.MapGroup("/api/products")
                       .WithTags("Products")
                       .RequireAuthorization();

        // GET: Todos pueden leer (si están autenticados)
        group.MapGet("/", async (IProductService productService) =>
        {
            var result = await productService.GetProductsAsync();
            return Results.Ok(result.Value);
        });

        group.MapGet("/{id:guid}", async (Guid id, IProductService productService) =>
        {
            var result = await productService.GetProductByIdAsync(id);
            return result.IsSuccess ? Results.Ok(result.Value) : Results.NotFound(result.Error);
        });

        // POST/PUT/DELETE/PATCH: Solo usuarios con Rol "Admin" pueden modificar
        group.MapPost("/", async ([FromBody] ProductRequest request, IProductService productService) =>
        {
            var result = await productService.CreateProductAsync(request);
            return Results.Created($"/api/products/{result.Value}", result.Value);
        }).RequireAuthorization(policy => policy.RequireRole("Admin"));

        group.MapPut("/{id:guid}", async (Guid id, [FromBody] ProductRequest request, IProductService productService) =>
        {
            var result = await productService.UpdateProductAsync(id, request);
            if (result.IsSuccess) return Results.NoContent();
            
            return result.Error.Code == "Product.NotFound" 
                ? Results.NotFound(result.Error) 
                : Results.BadRequest(result.Error);
        }).RequireAuthorization(policy => policy.RequireRole("Admin"));

        group.MapDelete("/{id:guid}", async (Guid id, IProductService productService) =>
        {
            var result = await productService.DeleteProductAsync(id);
            if (result.IsSuccess) return Results.NoContent();
            
            return result.Error.Code == "Product.NotFound" 
                ? Results.NotFound(result.Error) 
                : Results.BadRequest(result.Error);
        }).RequireAuthorization(policy => policy.RequireRole("Admin"));

        group.MapPatch("/{providerId:guid}/stock", async (Guid providerId, [FromBody] UpdateStockRequest request, IProductService productService) =>
        {
            var result = await productService.UpdateStockAsync(providerId, request.NewStock);
            if (result.IsSuccess) return Results.NoContent();
            
            return result.Error.Code == "Provider.NotFound" 
                ? Results.NotFound(result.Error) 
                : Results.BadRequest(result.Error);
        }).RequireAuthorization(policy => policy.RequireRole("Admin"));
        
        group.MapPatch("/{productId:guid}/min-threshold",
            async (Guid productId, [FromBody] UpdateMinThresholdStock request, IProductService productService) =>
            {
                var result = await productService.UpdateMinThresholdStockAsync(productId, request.NewMinThreshold);
                if (result.IsSuccess) return Results.NoContent();
                
                return result.Error.Code == "Product.NotFound" 
                    ? Results.NotFound(result.Error) 
                    : Results.BadRequest(result.Error);
            }).RequireAuthorization(policy => policy.RequireRole("Admin"));
    
    }
}
