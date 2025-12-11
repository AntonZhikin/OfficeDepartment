using Microsoft.AspNetCore.Mvc;
using OfficeDepartment.Handlers;
using OfficeDepartment.Requests;

namespace OfficeDepartment.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IAuthHandler authHandler) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var user = await authHandler.RegisterAsync(request);
        if (user == null)
        {
            return BadRequest(new { message = "Username or email already exists" });
        }

        return Ok(new { message = "User registered successfully", userId = user.Id });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var (user, token) = await authHandler.LoginAsync(request);
        if (user == null || token == null)
        {
            return Unauthorized(new { message = "Invalid username or password" });
        }

        return Ok(new { token, user = new { user.Id, user.Username, user.Email, user.Role } });
    }
}

