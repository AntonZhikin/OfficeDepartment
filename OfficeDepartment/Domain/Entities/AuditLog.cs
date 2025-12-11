namespace OfficeDepartment.Domain.Entities;

public class AuditLog
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string Action { get; init; } = string.Empty; 
    public string EntityType { get; init; } = string.Empty; 
    public Guid? EntityId { get; init; }
    public Guid? UserId { get; init; }
    public string? OldValues { get; init; }
    public string? NewValues { get; init; }
    public DateTime Timestamp { get; init; } = DateTime.UtcNow;
    public string? IpAddress { get; init; }
}

