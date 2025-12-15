using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OfficeDepartment.Handlers;
using OfficeDepartment.Requests;

namespace OfficeDepartment.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class OfficeTaskController(IOfficeTaskHandler handler) : BaseController
{
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var task = await handler.GetByIdAsync(id);
        if (task == null)
        {
            return NotFound();
        }
        return Ok(task);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] OfficeTaskFilterRequest filter)
    {
        var tasks = await handler.GetAllAsync(filter);
        return Ok(tasks);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] CreateOfficeTaskRequest request)
    {
        var userId = GetUserId();
        var ipAddress = GetIpAddress();
        var task = await handler.CreateAsync(request, userId, ipAddress);
        return CreatedAtAction(nameof(GetById), new { id = task.Id }, task);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateOfficeTaskRequest request)
    {
        var userId = GetUserId();
        var ipAddress = GetIpAddress();
        var task = await handler.UpdateAsync(id, request, userId, ipAddress);
        if (task == null)
        {
            return NotFound();
        }
        return Ok(task);
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

