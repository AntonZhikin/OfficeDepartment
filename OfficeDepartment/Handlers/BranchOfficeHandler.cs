using Microsoft.EntityFrameworkCore;
using OfficeDepartment.Domain.Entities;
using OfficeDepartment.Infrastructure.Data;
using OfficeDepartment.Infrastructure.Services;
using OfficeDepartment.Requests;

namespace OfficeDepartment.Handlers;

public interface IBranchOfficeHandler
{
    Task<BranchOffice?> GetByIdAsync(Guid id);
    Task<IEnumerable<BranchOffice>> GetAllAsync(BranchOfficeFilterRequest filter);
    Task<BranchOffice> CreateAsync(CreateBranchOfficeRequest request, Guid? userId, string? ipAddress);
    Task<BranchOffice?> UpdateAsync(Guid id, UpdateBranchOfficeRequest request, Guid? userId, string? ipAddress);
    Task<bool> DeleteAsync(Guid id, Guid? userId, string? ipAddress);
}

public class BranchOfficeHandler(ApplicationDbContext context, IAuditService auditService) : IBranchOfficeHandler
{
    public async Task<BranchOffice?> GetByIdAsync(Guid id)
    {
        return await context.BranchOffices
            .Include(b => b.HeadOffice)
            .Include(b => b.Tasks)
            .Include(b => b.Employees)
            .FirstOrDefaultAsync(b => b.Id == id);
    }

    public async Task<IEnumerable<BranchOffice>> GetAllAsync(BranchOfficeFilterRequest filter)
    {
        var query = context.BranchOffices
            .Include(b => b.HeadOffice)
            .AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
        {
            query = query.Where(b => 
                b.Name.Contains(filter.SearchTerm) ||
                b.City.Contains(filter.SearchTerm));
        }

        if (filter.HeadOfficeId.HasValue)
        {
            query = query.Where(b => b.HeadOfficeId == filter.HeadOfficeId.Value);
        }

        if (!string.IsNullOrWhiteSpace(filter.City))
        {
            query = query.Where(b => b.City == filter.City);
        }

        return await query
            .OrderBy(b => b.Name)
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync();
    }

    public async Task<BranchOffice> CreateAsync(CreateBranchOfficeRequest request, Guid? userId, string? ipAddress)
    {
        // Проверяем существование HeadOffice
        var headOfficeExists = await context.HeadOffices
            .AnyAsync(h => h.Id == request.HeadOfficeId);
        
        if (!headOfficeExists)
        {
            throw new InvalidOperationException($"HeadOffice with ID {request.HeadOfficeId} does not exist.");
        }

        var branchOffice = new BranchOffice
        {
            Name = request.Name,
            Address = request.Address,
            City = request.City,
            Country = request.Country,
            PhoneNumber = request.PhoneNumber,
            Email = request.Email,
            HeadOfficeId = request.HeadOfficeId,
            CreatedAt = DateTime.UtcNow
        };

        context.BranchOffices.Add(branchOffice);
        await context.SaveChangesAsync();

        await auditService.LogActionAsync("Create", "BranchOffice", branchOffice.Id, userId, null,
            System.Text.Json.JsonSerializer.Serialize(branchOffice), ipAddress);

        return branchOffice;
    }

    public async Task<BranchOffice?> UpdateAsync(Guid id, UpdateBranchOfficeRequest request, Guid? userId, string? ipAddress)
    {
        var branchOffice = await context.BranchOffices.FindAsync(id);
        if (branchOffice == null) return null;

        // Проверяем существование HeadOffice
        var headOfficeExists = await context.HeadOffices
            .AnyAsync(h => h.Id == request.HeadOfficeId);
        
        if (!headOfficeExists)
        {
            throw new InvalidOperationException($"HeadOffice with ID {request.HeadOfficeId} does not exist.");
        }

        var oldValues = System.Text.Json.JsonSerializer.Serialize(branchOffice);

        branchOffice.Name = request.Name;
        branchOffice.Address = request.Address;
        branchOffice.City = request.City;
        branchOffice.Country = request.Country;
        branchOffice.PhoneNumber = request.PhoneNumber;
        branchOffice.Email = request.Email;
        branchOffice.HeadOfficeId = request.HeadOfficeId;
        branchOffice.UpdatedAt = DateTime.UtcNow;

        await context.SaveChangesAsync();

        var newValues = System.Text.Json.JsonSerializer.Serialize(branchOffice);
        await auditService.LogActionAsync("Update", "BranchOffice", branchOffice.Id, userId, oldValues, newValues, ipAddress);

        return branchOffice;
    }

    public async Task<bool> DeleteAsync(Guid id, Guid? userId, string? ipAddress)
    {
        var branchOffice = await context.BranchOffices.FindAsync(id);
        if (branchOffice == null) return false;

        var oldValues = System.Text.Json.JsonSerializer.Serialize(branchOffice);
        context.BranchOffices.Remove(branchOffice);
        await context.SaveChangesAsync();

        await auditService.LogActionAsync("Delete", "BranchOffice", id, userId, oldValues, null, ipAddress);

        return true;
    }
}

