using Microsoft.AspNetCore.Mvc;
using OfficeDepartment.Handlers;
using OfficeDepartment.Requests;

namespace OfficeDepartment.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IAuthHandler authHandler) : ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var (user, employee, token) = await authHandler.LoginAsync(request);
        if (user == null || token == null)
        {
            return Unauthorized(new { message = "Invalid username or password" });
        }

        return Ok(new 
        { 
            token, 
            user = new 
            { 
                user.Id, 
                user.Username, 
                user.Email, 
                user.Role,
                employee = employee != null ? new
                {
                    employee.Id,
                    employee.FirstName,
                    employee.LastName,
                    employee.Position
                } : null
            } 
        });
    }
}


