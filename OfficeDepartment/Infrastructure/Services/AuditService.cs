using OfficeDepartment.Domain.Entities;
using OfficeDepartment.Infrastructure.Data;

namespace OfficeDepartment.Infrastructure.Services;

public interface IAuditService
{
    Task LogActionAsync(string action, string entityType, Guid? entityId, Guid? userId, string? oldValues = null, string? newValues = null, string? ipAddress = null);
}

public class AuditService(ApplicationDbContext context) : IAuditService
{
    public async Task LogActionAsync(string action, string entityType, Guid? entityId, Guid? userId, string? oldValues = null, string? newValues = null, string? ipAddress = null)
    {
        var auditLog = new AuditLog
        {
            Action = action,
            EntityType = entityType,
            EntityId = entityId,
            UserId = userId,
            OldValues = oldValues,
            NewValues = newValues,
            IpAddress = ipAddress,
            Timestamp = DateTime.UtcNow
        };

        context.AuditLogs.Add(auditLog);
        await context.SaveChangesAsync();
    }
}

