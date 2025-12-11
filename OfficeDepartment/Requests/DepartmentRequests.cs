namespace OfficeDepartment.Requests;

public class CreateDepartmentRequest
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Guid HeadOfficeId { get; set; }
    public Guid? ManagerId { get; set; }
}

public class UpdateDepartmentRequest
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Guid HeadOfficeId { get; set; }
    public Guid? ManagerId { get; set; }
}

public class DepartmentFilterRequest
{
    public string? SearchTerm { get; set; }
    public Guid? HeadOfficeId { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

