using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OfficeDepartment.Handlers;
using OfficeDepartment.Requests;

namespace OfficeDepartment.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DepartmentController(IDepartmentHandler handler) : BaseController
{
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var department = await handler.GetByIdAsync(id);
        if (department == null)
        {
            return NotFound();
        }
        return Ok(department);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] DepartmentFilterRequest filter)
    {
        var departments = await handler.GetAllAsync(filter);
        return Ok(departments);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] CreateDepartmentRequest request)
    {
        var userId = GetUserId();
        var ipAddress = GetIpAddress();
        var department = await handler.CreateAsync(request, userId, ipAddress);
        return CreatedAtAction(nameof(GetById), new { id = department.Id }, department);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateDepartmentRequest request)
    {
        var userId = GetUserId();
        var ipAddress = GetIpAddress();
        var department = await handler.UpdateAsync(id, request, userId, ipAddress);
        if (department == null)
        {
            return NotFound();
        }
        return Ok(department);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var userId = GetUserId();
        var ipAddress = GetIpAddress();
        var result = await handler.DeleteAsync(id, userId, ipAddress);
        if (!result)
        {
            return NotFound();
        }
        return NoContent();
    }
}

