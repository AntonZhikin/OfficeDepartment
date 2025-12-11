using Microsoft.EntityFrameworkCore;
using OfficeDepartment.Domain.Entities;
using OfficeDepartment.Infrastructure.Data;
using OfficeDepartment.Infrastructure.Services;
using OfficeDepartment.Requests;
using TaskStatus = OfficeDepartment.Domain.Entities.TaskStatus;

namespace OfficeDepartment.Handlers;

public interface IOfficeTaskHandler
{
    Task<OfficeTask?> GetByIdAsync(Guid id);
    Task<IEnumerable<OfficeTask>> GetAllAsync(OfficeTaskFilterRequest filter);
    Task<OfficeTask> CreateAsync(CreateOfficeTaskRequest request, Guid? userId, string? ipAddress);
    Task<OfficeTask?> UpdateAsync(Guid id, UpdateOfficeTaskRequest request, Guid? userId, string? ipAddress);
    Task<bool> DeleteAsync(Guid id, Guid? userId, string? ipAddress);
}

public class OfficeTaskHandler(ApplicationDbContext context, IAuditService auditService) : IOfficeTaskHandler
{
    public async Task<OfficeTask?> GetByIdAsync(Guid id)
    {
        return await context.OfficeTasks
            .Include(t => t.BranchOffice)
            .Include(t => t.AssignedEmployee)
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<IEnumerable<OfficeTask>> GetAllAsync(OfficeTaskFilterRequest filter)
    {
        var query = context.OfficeTasks
            .Include(t => t.BranchOffice)
            .Include(t => t.AssignedEmployee)
            .AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
        {
            query = query.Where(t => 
                t.Title.Contains(filter.SearchTerm) ||
                t.Description.Contains(filter.SearchTerm));
        }

        if (filter.Status.HasValue)
        {
            query = query.Where(t => t.Status == filter.Status.Value);
        }

        if (filter.Priority.HasValue)
        {
            query = query.Where(t => t.Priority == filter.Priority.Value);
        }

        if (filter.BranchOfficeId.HasValue)
        {
            query = query.Where(t => t.BranchOfficeId == filter.BranchOfficeId.Value);
        }

        if (filter.AssignedEmployeeId.HasValue)
        {
            query = query.Where(t => t.AssignedEmployeeId == filter.AssignedEmployeeId.Value);
        }

        return await query
            .OrderBy(t => t.CreatedAt)
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync();
    }

    public async Task<OfficeTask> CreateAsync(CreateOfficeTaskRequest request, Guid? userId, string? ipAddress)
    {
        var task = new OfficeTask
        {
            Title = request.Title,
            Description = request.Description,
            Priority = request.Priority,
            BranchOfficeId = request.BranchOfficeId,
            AssignedEmployeeId = request.AssignedEmployeeId,
            DueDate = request.DueDate.HasValue 
                ? (request.DueDate.Value.Kind == DateTimeKind.Utc 
                    ? request.DueDate.Value 
                    : request.DueDate.Value.Kind == DateTimeKind.Unspecified
                        ? DateTime.SpecifyKind(request.DueDate.Value, DateTimeKind.Utc)
                        : request.DueDate.Value.ToUniversalTime())
                : null,
            Status = TaskStatus.Pending,
            CreatedAt = DateTime.UtcNow
        };

        context.OfficeTasks.Add(task);
        await context.SaveChangesAsync();

        await auditService.LogActionAsync("Create", "OfficeTask", task.Id, userId, null,
            System.Text.Json.JsonSerializer.Serialize(task), ipAddress);

        return task;
    }

    public async Task<OfficeTask?> UpdateAsync(Guid id, UpdateOfficeTaskRequest request, Guid? userId, string? ipAddress)
    {
        var task = await context.OfficeTasks.FindAsync(id);
        if (task == null) return null;

        var oldValues = System.Text.Json.JsonSerializer.Serialize(task);

        task.Title = request.Title;
        task.Description = request.Description;
        task.Status = request.Status;
        task.Priority = request.Priority;
        task.BranchOfficeId = request.BranchOfficeId;
        task.AssignedEmployeeId = request.AssignedEmployeeId;
        task.DueDate = request.DueDate.HasValue 
            ? (request.DueDate.Value.Kind == DateTimeKind.Utc 
                ? request.DueDate.Value 
                : request.DueDate.Value.Kind == DateTimeKind.Unspecified
                    ? DateTime.SpecifyKind(request.DueDate.Value, DateTimeKind.Utc)
                    : request.DueDate.Value.ToUniversalTime())
            : null;
        task.UpdatedAt = DateTime.UtcNow;

        if (request.Status == TaskStatus.Completed && task.CompletedAt == null)
        {
            task.CompletedAt = DateTime.UtcNow;
        }

        await context.SaveChangesAsync();

        var newValues = System.Text.Json.JsonSerializer.Serialize(task);
        await auditService.LogActionAsync("Update", "OfficeTask", task.Id, userId, oldValues, newValues, ipAddress);

        return task;
    }

    public async Task<bool> DeleteAsync(Guid id, Guid? userId, string? ipAddress)
    {
        var task = await context.OfficeTasks.FindAsync(id);
        if (task == null) return false;

        var oldValues = System.Text.Json.JsonSerializer.Serialize(task);
        context.OfficeTasks.Remove(task);
        await context.SaveChangesAsync();

        await auditService.LogActionAsync("Delete", "OfficeTask", id, userId, oldValues, null, ipAddress);

        return true;
    }
}

