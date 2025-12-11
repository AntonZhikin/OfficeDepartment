namespace OfficeDepartment.Requests;

public class CreateHeadOfficeRequest
{
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}

public class UpdateHeadOfficeRequest
{
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}

public class HeadOfficeFilterRequest
{
    public string? SearchTerm { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

