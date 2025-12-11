namespace OfficeDepartment.Requests;

public class CreateBranchOfficeRequest
{
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public Guid HeadOfficeId { get; set; }
}

public class UpdateBranchOfficeRequest
{
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public Guid HeadOfficeId { get; set; }
}

public class BranchOfficeFilterRequest
{
    public string? SearchTerm { get; set; }
    public Guid? HeadOfficeId { get; set; }
    public string? City { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

