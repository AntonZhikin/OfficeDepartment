using OfficeDepartment.Domain.Entities;
using TaskStatus = OfficeDepartment.Domain.Entities.TaskStatus;

namespace OfficeDepartment.Requests;

public class CreateOfficeTaskRequest
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public TaskPriority Priority { get; set; } = TaskPriority.Medium;
    public Guid? BranchOfficeId { get; set; }
    public Guid? DepartmentId { get; set; }
    public Guid? AssignedEmployeeId { get; set; }
    public DateTime? DueDate { get; set; }
}

public class UpdateOfficeTaskRequest
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public TaskStatus Status { get; set; }
    public TaskPriority Priority { get; set; }
    public Guid? BranchOfficeId { get; set; }
    public Guid? DepartmentId { get; set; }
    public Guid? AssignedEmployeeId { get; set; }
    public DateTime? DueDate { get; set; }
}

public class OfficeTaskFilterRequest
{
    public string? SearchTerm { get; set; }
    public TaskStatus? Status { get; set; }
    public TaskPriority? Priority { get; set; }
    public Guid? BranchOfficeId { get; set; }
    public Guid? AssignedEmployeeId { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

