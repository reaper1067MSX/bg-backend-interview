using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BankGuayaquil.Inventory.Application.Common;
using BankGuayaquil.Inventory.Application.DTOs;
using BankGuayaquil.Inventory.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace BankGuayaquil.Inventory.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;

    public AuthService(IUserRepository userRepository, IConfiguration configuration)
    {
        _userRepository = userRepository;
        _configuration = configuration;
    }

    public async Task<Result<AuthResponse>> LoginAsync(LoginRequest request)
    {
        var user = await _userRepository.GetByUsernameAsync(request.Username);
        
        // En una implementación real, usar BCrypt o Argon2 para verificar PasswordHash
        if (user == null || request.Password != "password123") 
            return Result<AuthResponse>.Failure(new Error("Auth.InvalidCredentials", "Invalid username or password"));

        // Generación de JWT Real
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()), // Crucial para la auditoría (CreatedBy)
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, user.Role) // Inyectamos el rol Admin
        };

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddHours(2),
            signingCredentials: credentials);

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
        
        return Result<AuthResponse>.Success(new AuthResponse(tokenString, user.Username, user.Role));
    }
}
