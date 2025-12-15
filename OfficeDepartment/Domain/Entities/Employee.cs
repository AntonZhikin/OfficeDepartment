namespace OfficeDepartment.Domain.Entities;

public class Employee
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Position { get; set; } = string.Empty;
    public Guid? BranchOfficeId { get; set; }
    public Guid? DepartmentId { get; set; }
    public Guid? UserId { get; set; }
    public DateTime HireDate { get; init; } = DateTime.UtcNow;
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    
    public BranchOffice? BranchOffice { get; init; }
    public Department? Department { get; init; }
    public User? User { get; init; }
    public ICollection<OfficeTask> AssignedTasks { get; init; } = new List<OfficeTask>();
}

