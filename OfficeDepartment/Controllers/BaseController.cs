using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace OfficeDepartment.Controllers;

public abstract class BaseController : ControllerBase
{
    protected Guid? GetUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? 
                         User.FindFirst("sub")?.Value;
        return Guid.TryParse(userIdClaim, out var userId) ? userId : null;
    }

    protected string? GetIpAddress()
    {
        return HttpContext.Connection.RemoteIpAddress?.ToString();
    }
}

