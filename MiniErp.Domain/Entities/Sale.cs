namespace MiniErp.Domain.Entities;

public class Sale
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public User User { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public decimal Total { get; private set; }

    private readonly List<SaleItem> _items = new();
    public IReadOnlyCollection<SaleItem> Items => _items;

    protected Sale() { }

    public Sale(User user)
    {
        Id = Guid.NewGuid();
        User = user;
        UserId = user.Id;
        CreatedAt = DateTime.UtcNow;
        Total = 0;
    }

    public void AddItem(Product product, int quantity)
    {
        var item = new SaleItem(product, quantity);
        _items.Add(item);
        Total += item.SubTotal;
    }
}
