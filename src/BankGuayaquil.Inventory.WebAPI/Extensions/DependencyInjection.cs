using BankGuayaquil.Inventory.Application.Services;
using BankGuayaquil.Inventory.Domain.Interfaces;
using BankGuayaquil.Inventory.Infrastructure.Persistence;
using BankGuayaquil.Inventory.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

using BankGuayaquil.Inventory.Infrastructure.Persistence.Interceptors;

namespace BankGuayaquil.Inventory.WebAPI.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpContextAccessor();
        services.AddScoped<AuditableEntityInterceptor>();

        services.AddDbContext<InventoryDbContext>((sp, options) =>
        {
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"))
                   .AddInterceptors(sp.GetRequiredService<AuditableEntityInterceptor>());
        });

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<ISupplierRepository, SupplierRepository>();
        services.AddScoped<IUserRepository, UserRepository>();

        return services;
    }

    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<ISupplierService, SupplierService>();
        services.AddScoped<IAuthService, AuthService>();
        return services;
    }
}
