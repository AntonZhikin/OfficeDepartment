using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OfficeDepartment.Handlers;
using OfficeDepartment.Requests;

namespace OfficeDepartment.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class BranchOfficeController(IBranchOfficeHandler handler) : BaseController
{
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var branchOffice = await handler.GetByIdAsync(id);
        if (branchOffice == null)
        {
            return NotFound();
        }
        return Ok(branchOffice);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] BranchOfficeFilterRequest filter)
    {
        var branchOffices = await handler.GetAllAsync(filter);
        return Ok(branchOffices);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] CreateBranchOfficeRequest request)
    {
        try
        {
            var userId = GetUserId();
            var ipAddress = GetIpAddress();
            var branchOffice = await handler.CreateAsync(request, userId, ipAddress);
            return CreatedAtAction(nameof(GetById), new { id = branchOffice.Id }, branchOffice);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateBranchOfficeRequest request)
    {
        try
        {
            var userId = GetUserId();
            var ipAddress = GetIpAddress();
            var branchOffice = await handler.UpdateAsync(id, request, userId, ipAddress);
            if (branchOffice == null)
            {
                return NotFound();
            }
            return Ok(branchOffice);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
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

