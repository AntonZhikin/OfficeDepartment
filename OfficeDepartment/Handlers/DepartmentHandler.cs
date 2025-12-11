using Microsoft.EntityFrameworkCore;
using OfficeDepartment.Domain.Entities;
using OfficeDepartment.Infrastructure.Data;
using OfficeDepartment.Infrastructure.Services;
using OfficeDepartment.Requests;

namespace OfficeDepartment.Handlers;

public interface IDepartmentHandler
{
    Task<Department?> GetByIdAsync(Guid id);
    Task<IEnumerable<Department>> GetAllAsync(DepartmentFilterRequest filter);
    Task<Department> CreateAsync(CreateDepartmentRequest request, Guid? userId, string? ipAddress);
    Task<Department?> UpdateAsync(Guid id, UpdateDepartmentRequest request, Guid? userId, string? ipAddress);
    Task<bool> DeleteAsync(Guid id, Guid? userId, string? ipAddress);
}

public class DepartmentHandler(ApplicationDbContext context, IAuditService auditService) : IDepartmentHandler
{
    public async Task<Department?> GetByIdAsync(Guid id)
    {
        return await context.Departments
            .Include(d => d.HeadOffice)
            .Include(d => d.Employees)
            .FirstOrDefaultAsync(d => d.Id == id);
    }

    public async Task<IEnumerable<Department>> GetAllAsync(DepartmentFilterRequest filter)
    {
        var query = context.Departments
            .Include(d => d.HeadOffice)
            .AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
        {
            query = query.Where(d => 
                d.Name.Contains(filter.SearchTerm) ||
                d.Description.Contains(filter.SearchTerm));
        }

        if (filter.HeadOfficeId.HasValue)
        {
            query = query.Where(d => d.HeadOfficeId == filter.HeadOfficeId.Value);
        }

        return await query
            .OrderBy(d => d.Name)
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync();
    }

    public async Task<Department> CreateAsync(CreateDepartmentRequest request, Guid? userId, string? ipAddress)
    {
        var department = new Department
        {
            Name = request.Name,
            Description = request.Description,
            HeadOfficeId = request.HeadOfficeId,
            ManagerId = request.ManagerId,
            CreatedAt = DateTime.UtcNow
        };

        context.Departments.Add(department);
        await context.SaveChangesAsync();

        await auditService.LogActionAsync("Create", "Department", department.Id, userId, null,
            System.Text.Json.JsonSerializer.Serialize(department), ipAddress);

        return department;
    }

    public async Task<Department?> UpdateAsync(Guid id, UpdateDepartmentRequest request, Guid? userId, string? ipAddress)
    {
        var department = await context.Departments.FindAsync(id);
        if (department == null) return null;

        var oldValues = System.Text.Json.JsonSerializer.Serialize(department);

        department.Name = request.Name;
        department.Description = request.Description;
        department.HeadOfficeId = request.HeadOfficeId;
        department.ManagerId = request.ManagerId;
        department.UpdatedAt = DateTime.UtcNow;

        await context.SaveChangesAsync();

        var newValues = System.Text.Json.JsonSerializer.Serialize(department);
        await auditService.LogActionAsync("Update", "Department", department.Id, userId, oldValues, newValues, ipAddress);

        return department;
    }

    public async Task<bool> DeleteAsync(Guid id, Guid? userId, string? ipAddress)
    {
        var department = await context.Departments.FindAsync(id);
        if (department == null) return false;

        var oldValues = System.Text.Json.JsonSerializer.Serialize(department);
        context.Departments.Remove(department);
        await context.SaveChangesAsync();

        await auditService.LogActionAsync("Delete", "Department", id, userId, oldValues, null, ipAddress);

        return true;
    }
}

