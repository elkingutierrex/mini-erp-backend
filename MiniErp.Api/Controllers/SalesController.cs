using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniErp.Infrastructure.Persistence;
using MiniErp.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace MiniErp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize] // mantiene seguridad con JWT
public class SalesController : ControllerBase
{
    private readonly AppDbContext _context;

    public SalesController(AppDbContext context)
    {
        _context = context;
    }

    // POST: api/sales
    // Cualquier usuario autenticado puede crear ventas
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateSaleRequest request)
    {
        // MEJOR PRÁCTICA: obtener el UserId desde el token
        var userIdClaim =
            User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userIdClaim))
            return Unauthorized();

        var userId = Guid.Parse(userIdClaim);

        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
            return BadRequest("Invalid user");

        var sale = new Sale(user);

        foreach (var item in request.Items)
        {
            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.Id == item.ProductId);

            if (product == null)
                return BadRequest($"Product not found: {item.ProductId}");

            sale.AddItem(product, item.Quantity);
        }

        _context.Sales.Add(sale);
        await _context.SaveChangesAsync();

        return Ok(new
        {
            sale.Id,
            sale.CreatedAt,
            sale.Total
        });
    }

    // GET: api/sales/my
    [HttpGet("my")]
    public async Task<IActionResult> GetMySales()
    {
        var userIdClaim =
            User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userIdClaim))
            return Unauthorized();

        var userId = Guid.Parse(userIdClaim);

        var sales = await _context.Sales
            .Include(s => s.Items)
            .Where(s => s.UserId == userId)
            .Select(s => new
            {
                s.Id,
                s.CreatedAt,
                s.Total,
                Items = s.Items.Select(i => new
                {
                    i.ProductName,
                    i.Quantity,
                    i.UnitPrice,
                    i.SubTotal
                })
            })
            .ToListAsync();

        return Ok(sales);
    }

    // GET: api/sales
    // Visible para cualquiera (por ahora)
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var sales = await _context.Sales
            .Include(s => s.User)
            .Include(s => s.Items)
            .Select(s => new
            {
                s.Id,
                Seller = s.User.Email,
                s.CreatedAt,
                s.Total
            })
            .ToListAsync();

        return Ok(sales);
    }
}

#region DTOs

public class CreateSaleRequest
{
    public List<CreateSaleItemRequest> Items { get; set; } = new();
}

public class CreateSaleItemRequest
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}

#endregion
