using BankGuayaquil.Inventory.WebAPI.Extensions;
using BankGuayaquil.Inventory.WebAPI.Endpoints;
using BankGuayaquil.Inventory.WebAPI.Middlewares;
using BankGuayaquil.Inventory.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// 1. Inyección de Dependencias por Capas
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();

// 2. OpenAPI / Scalar
builder.Services.AddOpenApi();

// 3. Seguridad & CORS
builder.Services.AddAuthorization();

builder.Services.AddAuthentication("Bearer").AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
    };
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
        policy.WithOrigins("http://localhost:3000")
              .AllowAnyMethod()
              .AllowAnyHeader());
});

var app = builder.Build();

// 4. Pipeline de Middlewares
app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();
app.UseCors("AllowFrontend");
app.UseAuthentication();
app.UseAuthorization();

// 5. Seeding Data y Migraciones
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<InventoryDbContext>();
    
    // Aplicar migraciones pendientes de forma automática
    await context.Database.MigrateAsync();
    
    await DataSeeder.SeedAsync(context);
}

// 6. Mapeo de Endpoints REPR
app.MapAuthEndpoints();
app.MapProductEndpoints();
app.MapSupplierEndpoints();

app.Run();
