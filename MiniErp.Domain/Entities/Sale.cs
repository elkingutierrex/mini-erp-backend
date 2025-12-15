namespace MiniErp.Domain.Entities;

public class Sale
{
    public Guid Id { get; private set; }
    public Guid SellerId { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private readonly List<SaleItem> _items = new();
    public IReadOnlyCollection<SaleItem> Items => _items.AsReadOnly();

    public decimal TotalAmount => _items.Sum(i => i.Total);

    protected Sale() { }

    public Sale(Guid sellerId)
    {
        Id = Guid.NewGuid();
        SellerId = sellerId;
        CreatedAt = DateTime.UtcNow;
    }

    public void AddItem(SaleItem item)
    {
        _items.Add(item);
    }
}
