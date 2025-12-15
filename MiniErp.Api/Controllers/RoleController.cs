using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniErp.Infrastructure.Persistence;
using MiniErp.Domain.Entities;

namespace MiniErp.Api.Controllers;

[ApiController]
[Route("api/roles")]
public class RolesController : ControllerBase
{
    private readonly AppDbContext _context;

    public RolesController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/roles
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var roles = await _context.Roles
            .Select(r => new
            {
                r.Id,
                r.Name
            })
            .ToListAsync();

        return Ok(roles);
    }

    // GET: api/roles/{id}
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var role = await _context.Roles.FindAsync(id);

        if (role == null)
            return NotFound();

        return Ok(new
        {
            role.Id,
            role.Name
        });
    }

    // POST: api/roles
    [HttpPost]
    public async Task<IActionResult> Create(CreateRoleRequest request)
    {
        var exists = await _context.Roles
            .AnyAsync(r => r.Name == request.Name);

        if (exists)
            return BadRequest("Role already exists");

        var role = new Role(request.Name);

        _context.Roles.Add(role);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = role.Id }, new
        {
            role.Id,
            role.Name
        });
    }

    // PUT: api/roles/{id}
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, UpdateRoleRequest request)
    {
        var role = await _context.Roles.FindAsync(id);

        if (role == null)
            return NotFound();

        role.Update(request.Name);

        await _context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: api/roles/{id}
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var role = await _context.Roles.FindAsync(id);

        if (role == null)
            return NotFound();

        _context.Roles.Remove(role);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}

#region DTOs

public class CreateRoleRequest
{
    public string Name { get; set; } = string.Empty;
}

public class UpdateRoleRequest
{
    public string Name { get; set; } = string.Empty;
}

#endregion
