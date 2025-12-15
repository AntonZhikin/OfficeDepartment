using Microsoft.EntityFrameworkCore;
using OfficeDepartment.Domain.Entities;
using OfficeDepartment.Infrastructure.Data;
using OfficeDepartment.Infrastructure.Services;
using OfficeDepartment.Requests;

namespace OfficeDepartment.Handlers;

public interface IAuthHandler
{
    Task<(User? user, Employee? employee, string? token)> LoginAsync(LoginRequest request);
}

public class AuthHandler(ApplicationDbContext context, IPasswordHasher passwordHasher, IJwtService jwtService)
    : IAuthHandler
{
    public async Task<(User? user, Employee? employee, string? token)> LoginAsync(LoginRequest request)
    {
        var user = await context.Users.FirstOrDefaultAsync(u => u.Username == request.Username);
        
        if (user == null || !passwordHasher.VerifyPassword(request.Password, user.PasswordHash))
        {
            return (null, null, null);
        }

        user.LastLoginAt = DateTime.UtcNow;
        await context.SaveChangesAsync();

        // Получаем связанного сотрудника, если есть
        var employee = await context.Employees
            .Include(e => e.BranchOffice)
            .Include(e => e.Department)
            .FirstOrDefaultAsync(e => e.UserId == user.Id);

        var token = jwtService.GenerateToken(user.Id, user.Username, user.Email, user.Role);

        return (user, employee, token);
    }
}

