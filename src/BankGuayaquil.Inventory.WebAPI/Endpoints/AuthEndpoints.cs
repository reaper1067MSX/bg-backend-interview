using BankGuayaquil.Inventory.Application.DTOs;
using BankGuayaquil.Inventory.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace BankGuayaquil.Inventory.WebAPI.Endpoints;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/auth").WithTags("Authentication");

        group.MapPost("/login", async ([FromBody] LoginRequest request, IAuthService authService) =>
        {
            var result = await authService.LoginAsync(request);
            
            return result.IsSuccess 
                ? Results.Ok(result.Value) 
                : Results.BadRequest(new { error = result.Error.Description });
        })
        .AllowAnonymous();
    }
}
