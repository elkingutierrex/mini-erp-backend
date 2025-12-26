namespace MiniErp.Domain.Entities;

public class Sale
{
    public Guid Id { get; private set; }

    public Guid UserId { get; private set; }
    public User User { get; private set; } = default!;

    public DateTime CreatedAt { get; private set; }
    public decimal Total { get; private set; }

    private readonly List<SaleItem> _items = new();
    public IReadOnlyCollection<SaleItem> Items => _items;

    private Sale() { }
    
    public Sale(Guid userId, decimal total)
    {
        UserId = userId;
        Total = total;
        CreatedAt = DateTime.UtcNow;
    }

    public void AddItemSnapshot(
        int productId,
        string productName,
        decimal unitPrice,
        int quantity)
    {
        _items.Add(new SaleItem(
            productId,
            productName,
            unitPrice,
            quantity
        ));
    }
}