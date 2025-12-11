namespace OfficeDepartment.Domain.Entities;

public class User
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string Username { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string PasswordHash { get; init; } = string.Empty;
    public string Role { get; init; } = "User";
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; init; }
    public DateTime? LastLoginAt { get; set; }
}

