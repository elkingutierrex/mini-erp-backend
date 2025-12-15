using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniErp.Infrastructure.Persistence;
using MiniErp.Domain.Entities;

namespace MiniErp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RolesController : ControllerBase
{
    private readonly AppDbContext _context;

    public RolesController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/roles
    [HttpGet]
    public async Task<IActionResult> GetRoles()
    {
        var roles = await _context.Roles
            .Include(r => r.Permissions)
            .Select(r => new
            {
                r.Id,
                r.Name,
                Permissions = r.Permissions.Select(p => p.Name)
            })
            .ToListAsync();

        return Ok(roles);
    }

    // GET: api/roles/{id}
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetRoleById(Guid id)
    {
        var role = await _context.Roles
            .Include(r => r.Permissions)
            .FirstOrDefaultAsync(r => r.Id == id);

        if (role == null)
            return NotFound();

        return Ok(new
        {
            role.Id,
            role.Name,
            Permissions = role.Permissions.Select(p => p.Name)
        });
    }

    // PUT: api/roles/{id}/permissions
    [HttpPut("{id:guid}/permissions")]
    public async Task<IActionResult> UpdateRolePermissions(
        Guid id,
        [FromBody] List<string> permissions)
    {
        var role = await _context.Roles
            .Include(r => r.Permissions)
            .FirstOrDefaultAsync(r => r.Id == id);

        if (role == null)
            return NotFound("Role not found");

        var dbPermissions = await _context.Permissions
            .Where(p => permissions.Contains(p.Name))
            .ToListAsync();

        role.Permissions.Clear();

        foreach (var permission in dbPermissions)
        {
            role.Permissions.Add(permission);
        }

        await _context.SaveChangesAsync();

        return NoContent();
    }
}
