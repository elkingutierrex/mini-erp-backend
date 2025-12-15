using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniErp.Infrastructure.Persistence;
using MiniErp.Domain.Entities;

namespace MiniErp.Api.Controllers;

[ApiController]
[Route("api/sales")]
public class SalesController : ControllerBase
{
    private readonly AppDbContext _context;

    public SalesController(AppDbContext context)
    {
        _context = context;
    }

    // POST: api/sales
    // Create a sale (Seller)
    [HttpPost]
    public async Task<IActionResult> Create(CreateSaleRequest request)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == request.UserId);

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

    // GET: api/sales/my/{userId}
    [HttpGet("my/{userId:guid}")]
    public async Task<IActionResult> GetMySales(Guid userId)
    {
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
    // Admin / Manager
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
    public Guid UserId { get; set; }
    public List<CreateSaleItemRequest> Items { get; set; } = new();
}

public class CreateSaleItemRequest
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}

#endregion
