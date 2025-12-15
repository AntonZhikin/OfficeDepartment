namespace OfficeDepartment.Requests;

public class CreateEmployeeRequest
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Position { get; set; } = string.Empty;
    public Guid? BranchOfficeId { get; set; }
    public Guid? DepartmentId { get; set; }
    public DateTime HireDate { get; set; } = DateTime.UtcNow;
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class UpdateEmployeeRequest
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Position { get; set; } = string.Empty;
    public Guid? BranchOfficeId { get; set; }
    public Guid? DepartmentId { get; set; }
}

public class EmployeeFilterRequest
{
    public string? SearchTerm { get; set; }
    public Guid? BranchOfficeId { get; set; }
    public Guid? DepartmentId { get; set; }
    public string? Position { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

