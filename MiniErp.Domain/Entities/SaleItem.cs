namespace MiniErp.Domain.Entities;

public class SaleItem
{
    public Guid Id { get; private set; }
    public Guid ProductId { get; private set; }
    public string ProductName { get; private set; }
    public decimal UnitPrice { get; private set; }
    public int Quantity { get; private set; }

    public decimal Total => UnitPrice * Quantity;

    protected SaleItem() { }

    public SaleItem(Guid productId, string productName, decimal unitPrice, int quantity)
    {
        Id = Guid.NewGuid();
        ProductId = productId;
        ProductName = productName;
        UnitPrice = unitPrice;
        Quantity = quantity;
    }

    public void UpdateQuantity(int quantity)
    {
        Quantity = quantity;
    }
}
