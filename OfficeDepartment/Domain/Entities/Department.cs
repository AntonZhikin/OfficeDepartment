namespace OfficeDepartment.Domain.Entities;

public class Department
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Guid BranchOfficeId { get; set; }
    public Guid? ManagerId { get; set; }
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    
    public BranchOffice BranchOffice { get; init; } = null!;
    public ICollection<Employee> Employees { get; init; } = new List<Employee>();
}

