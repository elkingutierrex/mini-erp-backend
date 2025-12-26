using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniErp.Infrastructure.Persistence;
using MiniErp.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace MiniErp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize] 
public class SalesController : ControllerBase
{
    private readonly AppDbContext _context;

    public SalesController(AppDbContext context)
    {
        _context = context;
    }

    // POST: api/sales
    // Cualquier usuario autenticado puede crear ventas
  public async Task<IActionResult> Create([FromBody] CreateSaleRequest request)
{
    if (request.Items.Count == 0)
        return BadRequest("Sale must contain items");

    var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
    if (string.IsNullOrEmpty(userIdClaim))
        return Unauthorized();

    var userId = Guid.Parse(userIdClaim);

    var sale = new Sale(userId, request.Total);

    foreach (var item in request.Items)
    {
        sale.AddItemSnapshot(
            item.ProductId,
            item.Name,
            item.Price,
            item.Quantity
        );
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
    [HttpGet("my-sales")]
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
      public decimal Total { get; set; }
}

public class CreateSaleItemRequest
{
    public int ProductId { get; set; }
    public string Name { get; set; } = default!;
    public decimal Price { get; set; }
    public int Quantity { get; set; }
}

#endregion
