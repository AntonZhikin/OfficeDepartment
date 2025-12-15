namespace OfficeDepartment.Domain.Entities;

public class BranchOffice
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    
    public ICollection<Department> Departments { get; init; } = new List<Department>();
    public ICollection<OfficeTask> Tasks { get; init; } = new List<OfficeTask>();
    public ICollection<Employee> Employees { get; init; } = new List<Employee>();
}

