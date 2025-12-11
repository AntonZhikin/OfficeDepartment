using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OfficeDepartment.Handlers;
using OfficeDepartment.Requests;

namespace OfficeDepartment.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class EmployeeController(IEmployeeHandler handler) : BaseController
{
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var employee = await handler.GetByIdAsync(id);
        if (employee == null)
        {
            return NotFound();
        }
        return Ok(employee);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] EmployeeFilterRequest filter)
    {
        var employees = await handler.GetAllAsync(filter);
        return Ok(employees);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] CreateEmployeeRequest request)
    {
        var userId = GetUserId();
        var ipAddress = GetIpAddress();
        var employee = await handler.CreateAsync(request, userId, ipAddress);
        return CreatedAtAction(nameof(GetById), new { id = employee.Id }, employee);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateEmployeeRequest request)
    {
        var userId = GetUserId();
        var ipAddress = GetIpAddress();
        var employee = await handler.UpdateAsync(id, request, userId, ipAddress);
        if (employee == null)
        {
            return NotFound();
        }
        return Ok(employee);
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

