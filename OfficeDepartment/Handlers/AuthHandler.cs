using Microsoft.EntityFrameworkCore;
using OfficeDepartment.Domain.Entities;
using OfficeDepartment.Infrastructure.Data;
using OfficeDepartment.Infrastructure.Services;
using OfficeDepartment.Requests;

namespace OfficeDepartment.Handlers;

public interface IAuthHandler
{
    Task<User?> RegisterAsync(RegisterRequest request);
    Task<(User? user, string? token)> LoginAsync(LoginRequest request);
}

public class AuthHandler(ApplicationDbContext context, IPasswordHasher passwordHasher, IJwtService jwtService)
    : IAuthHandler
{
    public async Task<User?> RegisterAsync(RegisterRequest request)
    {
        if (await context.Users.AnyAsync(u => u.Username == request.Username || u.Email == request.Email))
        {
            return null; // User already exists
        }

        var user = new User
        {
            Username = request.Username,
            Email = request.Email,
            PasswordHash = passwordHasher.HashPassword(request.Password),
            Role = "User",
            CreatedAt = DateTime.UtcNow
        };

        context.Users.Add(user);
        await context.SaveChangesAsync();

        return user;
    }

    public async Task<(User? user, string? token)> LoginAsync(LoginRequest request)
    {
        var user = await context.Users.FirstOrDefaultAsync(u => u.Username == request.Username);
        
        if (user == null || !passwordHasher.VerifyPassword(request.Password, user.PasswordHash))
        {
            return (null, null);
        }

        user.LastLoginAt = DateTime.UtcNow;
        await context.SaveChangesAsync();

        var token = jwtService.GenerateToken(user.Id, user.Username, user.Email, user.Role);

        return (user, token);
    }
}

