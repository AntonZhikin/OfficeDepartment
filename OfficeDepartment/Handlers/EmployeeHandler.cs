using Microsoft.EntityFrameworkCore;
using OfficeDepartment.Domain.Entities;
using OfficeDepartment.Infrastructure.Data;
using OfficeDepartment.Infrastructure.Services;
using OfficeDepartment.Requests;

namespace OfficeDepartment.Handlers;

public interface IEmployeeHandler
{
    Task<Employee?> GetByIdAsync(Guid id);
    Task<IEnumerable<Employee>> GetAllAsync(EmployeeFilterRequest filter);
    Task<Employee> CreateAsync(CreateEmployeeRequest request, Guid? userId, string? ipAddress);
    Task<Employee?> UpdateAsync(Guid id, UpdateEmployeeRequest request, Guid? userId, string? ipAddress);
    Task<bool> DeleteAsync(Guid id, Guid? userId, string? ipAddress);
}

public class EmployeeHandler(ApplicationDbContext context, IAuditService auditService, IPasswordHasher passwordHasher) : IEmployeeHandler
{
    public async Task<Employee?> GetByIdAsync(Guid id)
    {
        return await context.Employees
            .Include(e => e.BranchOffice)
            .Include(e => e.Department)
            .Include(e => e.User)
            .Include(e => e.AssignedTasks)
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<IEnumerable<Employee>> GetAllAsync(EmployeeFilterRequest filter)
    {
        var query = context.Employees
            .Include(e => e.BranchOffice)
            .Include(e => e.Department)
            .AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
        {
            query = query.Where(e => 
                e.FirstName.Contains(filter.SearchTerm) ||
                e.LastName.Contains(filter.SearchTerm) ||
                e.Email.Contains(filter.SearchTerm) ||
                e.Position.Contains(filter.SearchTerm));
        }

        if (filter.BranchOfficeId.HasValue)
        {
            query = query.Where(e => e.BranchOfficeId == filter.BranchOfficeId.Value);
        }

        if (filter.DepartmentId.HasValue)
        {
            query = query.Where(e => e.DepartmentId == filter.DepartmentId.Value);
        }

        if (!string.IsNullOrWhiteSpace(filter.Position))
        {
            query = query.Where(e => e.Position == filter.Position);
        }

        return await query
            .OrderBy(e => e.LastName)
            .ThenBy(e => e.FirstName)
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync();
    }

    public async Task<Employee> CreateAsync(CreateEmployeeRequest request, Guid? userId, string? ipAddress)
    {
        // Проверяем, не существует ли уже пользователь с таким username или email
        if (await context.Users.AnyAsync(u => u.Username == request.Username || u.Email == request.Email))
        {
            throw new InvalidOperationException("Username or email already exists");
        }

        // Создаем User для сотрудника
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

        // Создаем Employee и связываем с User
        var employee = new Employee
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber,
            Position = request.Position,
            BranchOfficeId = request.BranchOfficeId,
            DepartmentId = request.DepartmentId,
            UserId = user.Id,
            HireDate = request.HireDate.Kind == DateTimeKind.Utc 
                ? request.HireDate 
                : request.HireDate.Kind == DateTimeKind.Unspecified
                    ? DateTime.SpecifyKind(request.HireDate, DateTimeKind.Utc)
                    : request.HireDate.ToUniversalTime(),
            CreatedAt = DateTime.UtcNow
        };

        context.Employees.Add(employee);
        await context.SaveChangesAsync();

        // Сериализуем только нужные поля без навигационных свойств
        var employeeData = new
        {
            employee.Id,
            employee.FirstName,
            employee.LastName,
            employee.Email,
            employee.PhoneNumber,
            employee.Position,
            employee.BranchOfficeId,
            employee.DepartmentId,
            employee.UserId,
            employee.HireDate,
            employee.CreatedAt
        };

        await auditService.LogActionAsync("Create", "Employee", employee.Id, userId, null,
            System.Text.Json.JsonSerializer.Serialize(employeeData), ipAddress);

        return employee;
    }

    public async Task<Employee?> UpdateAsync(Guid id, UpdateEmployeeRequest request, Guid? userId, string? ipAddress)
    {
        var employee = await context.Employees.FindAsync(id);
        if (employee == null) return null;

        var oldValues = System.Text.Json.JsonSerializer.Serialize(new
        {
            employee.Id,
            employee.FirstName,
            employee.LastName,
            employee.Email,
            employee.PhoneNumber,
            employee.Position,
            employee.BranchOfficeId,
            employee.DepartmentId,
            employee.UserId,
            employee.HireDate,
            employee.CreatedAt,
            employee.UpdatedAt
        });

        employee.FirstName = request.FirstName;
        employee.LastName = request.LastName;
        employee.Email = request.Email;
        employee.PhoneNumber = request.PhoneNumber;
        employee.Position = request.Position;
        employee.BranchOfficeId = request.BranchOfficeId;
        employee.DepartmentId = request.DepartmentId;
        employee.UpdatedAt = DateTime.UtcNow;

        await context.SaveChangesAsync();

        var newValues = System.Text.Json.JsonSerializer.Serialize(new
        {
            employee.Id,
            employee.FirstName,
            employee.LastName,
            employee.Email,
            employee.PhoneNumber,
            employee.Position,
            employee.BranchOfficeId,
            employee.DepartmentId,
            employee.UserId,
            employee.HireDate,
            employee.CreatedAt,
            employee.UpdatedAt
        });
        
        await auditService.LogActionAsync("Update", "Employee", employee.Id, userId, oldValues, newValues, ipAddress);

        return employee;
    }

    public async Task<bool> DeleteAsync(Guid id, Guid? userId, string? ipAddress)
    {
        var employee = await context.Employees.Include(e => e.User).FirstOrDefaultAsync(e => e.Id == id);
        if (employee == null) return false;

        var oldValues = System.Text.Json.JsonSerializer.Serialize(new
        {
            employee.Id,
            employee.FirstName,
            employee.LastName,
            employee.Email,
            employee.PhoneNumber,
            employee.Position,
            employee.BranchOfficeId,
            employee.DepartmentId,
            employee.UserId,
            employee.HireDate,
            employee.CreatedAt,
            employee.UpdatedAt
        });
        
        // Удаляем связанного User, если он есть
        if (employee.UserId.HasValue && employee.User != null)
        {
            context.Users.Remove(employee.User);
        }
        
        context.Employees.Remove(employee);
        await context.SaveChangesAsync();

        await auditService.LogActionAsync("Delete", "Employee", id, userId, oldValues, null, ipAddress);

        return true;
    }
}

