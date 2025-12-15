namespace OfficeDepartment.Domain.Entities;

public class OfficeTask
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public TaskStatus Status { get; set; } = TaskStatus.Pending;
    public TaskPriority Priority { get; set; } = TaskPriority.Medium;
    public Guid? BranchOfficeId { get; set; }
    public Guid? DepartmentId { get; set; }
    public Guid? AssignedEmployeeId { get; set; }
    public DateTime? DueDate { get; set; }
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    

    public BranchOffice? BranchOffice { get; init; }
    public Department? Department { get; init; }
    public Employee? AssignedEmployee { get; init; }
}

public enum TaskStatus
{
    Pending = 0,
    InProgress = 1,
    Completed = 2,
    Cancelled = 3
}

public enum TaskPriority
{
    Low = 0,
    Medium = 1,
    High = 2,
    Critical = 3
}

