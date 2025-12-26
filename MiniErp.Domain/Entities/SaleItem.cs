namespace MiniErp.Domain.Entities;

public class SaleItem
{
    public Guid Id { get; private set; }

    public int ProductId { get; private set; }
    public string ProductName { get; private set; } = default!;
    public decimal UnitPrice { get; private set; }
    public int Quantity { get; private set; }
    public decimal SubTotal { get; private set; }

    private SaleItem() { }

    public SaleItem(
        int productId,
        string productName,
        decimal unitPrice,
        int quantity)
    {
        ProductId = productId;
        ProductName = productName;
        UnitPrice = unitPrice;
        Quantity = quantity;
        SubTotal = unitPrice * quantity;
    }
}
