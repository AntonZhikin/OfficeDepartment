using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OfficeDepartment.Handlers;
using OfficeDepartment.Requests;
using System.Security.Claims;

namespace OfficeDepartment.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class HeadOfficeController(IHeadOfficeHandler handler) : BaseController
{
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var headOffice = await handler.GetByIdAsync(id);
        if (headOffice == null)
        {
            return NotFound();
        }
        return Ok(headOffice);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] HeadOfficeFilterRequest filter)
    {
        var headOffices = await handler.GetAllAsync(filter);
        return Ok(headOffices);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] CreateHeadOfficeRequest request)
    {
        var userId = GetUserId();
        var ipAddress = GetIpAddress();
        var headOffice = await handler.CreateAsync(request, userId, ipAddress);
        return CreatedAtAction(nameof(GetById), new { id = headOffice.Id }, headOffice);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateHeadOfficeRequest request)
    {
        var userId = GetUserId();
        var ipAddress = GetIpAddress();
        var headOffice = await handler.UpdateAsync(id, request, userId, ipAddress);
        if (headOffice == null)
        {
            return NotFound();
        }
        return Ok(headOffice);
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

