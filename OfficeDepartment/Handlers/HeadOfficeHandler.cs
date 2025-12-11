using Microsoft.EntityFrameworkCore;
using OfficeDepartment.Domain.Entities;
using OfficeDepartment.Infrastructure.Data;
using OfficeDepartment.Infrastructure.Services;
using OfficeDepartment.Requests;

namespace OfficeDepartment.Handlers;

public interface IHeadOfficeHandler
{
    Task<HeadOffice?> GetByIdAsync(Guid id);
    Task<IEnumerable<HeadOffice>> GetAllAsync(HeadOfficeFilterRequest filter);
    Task<HeadOffice> CreateAsync(CreateHeadOfficeRequest request, Guid? userId, string? ipAddress);
    Task<HeadOffice?> UpdateAsync(Guid id, UpdateHeadOfficeRequest request, Guid? userId, string? ipAddress);
    Task<bool> DeleteAsync(Guid id, Guid? userId, string? ipAddress);
}

public class HeadOfficeHandler(ApplicationDbContext context, IAuditService auditService) : IHeadOfficeHandler
{
    public async Task<HeadOffice?> GetByIdAsync(Guid id)
    {
        return await context.HeadOffices
            .Include(h => h.BranchOffices)
            .Include(h => h.Departments)
            .FirstOrDefaultAsync(h => h.Id == id);
    }

    public async Task<IEnumerable<HeadOffice>> GetAllAsync(HeadOfficeFilterRequest filter)
    {
        var query = context.HeadOffices.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
        {
            query = query.Where(h => 
                h.Name.Contains(filter.SearchTerm) ||
                h.City.Contains(filter.SearchTerm) ||
                h.Country.Contains(filter.SearchTerm));
        }

        if (!string.IsNullOrWhiteSpace(filter.City))
        {
            query = query.Where(h => h.City == filter.City);
        }

        if (!string.IsNullOrWhiteSpace(filter.Country))
        {
            query = query.Where(h => h.Country == filter.Country);
        }

        return await query
            .OrderBy(h => h.Name)
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync();
    }

    public async Task<HeadOffice> CreateAsync(CreateHeadOfficeRequest request, Guid? userId, string? ipAddress)
    {
        var headOffice = new HeadOffice
        {
            Name = request.Name,
            Address = request.Address,
            City = request.City,
            Country = request.Country,
            PhoneNumber = request.PhoneNumber,
            Email = request.Email,
            CreatedAt = DateTime.UtcNow
        };

        context.HeadOffices.Add(headOffice);
        await context.SaveChangesAsync();

        await auditService.LogActionAsync("Create", "HeadOffice", headOffice.Id, userId, null, 
            System.Text.Json.JsonSerializer.Serialize(headOffice), ipAddress);

        return headOffice;
    }

    public async Task<HeadOffice?> UpdateAsync(Guid id, UpdateHeadOfficeRequest request, Guid? userId, string? ipAddress)
    {
        var headOffice = await context.HeadOffices.FindAsync(id);
        if (headOffice == null) return null;

        var oldValues = System.Text.Json.JsonSerializer.Serialize(headOffice);

        headOffice.Name = request.Name;
        headOffice.Address = request.Address;
        headOffice.City = request.City;
        headOffice.Country = request.Country;
        headOffice.PhoneNumber = request.PhoneNumber;
        headOffice.Email = request.Email;
        headOffice.UpdatedAt = DateTime.UtcNow;

        await context.SaveChangesAsync();

        var newValues = System.Text.Json.JsonSerializer.Serialize(headOffice);
        await auditService.LogActionAsync("Update", "HeadOffice", headOffice.Id, userId, oldValues, newValues, ipAddress);

        return headOffice;
    }

    public async Task<bool> DeleteAsync(Guid id, Guid? userId, string? ipAddress)
    {
        var headOffice = await context.HeadOffices.FindAsync(id);
        if (headOffice == null) return false;

        var oldValues = System.Text.Json.JsonSerializer.Serialize(headOffice);
        context.HeadOffices.Remove(headOffice);
        await context.SaveChangesAsync();

        await auditService.LogActionAsync("Delete", "HeadOffice", id, userId, oldValues, null, ipAddress);

        return true;
    }
}

