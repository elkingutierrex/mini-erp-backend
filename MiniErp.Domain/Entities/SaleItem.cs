namespace MiniErp.Domain.Entities;

public class SaleItem
{
    public Guid Id { get; private set; }
    public Guid SaleId { get; private set; }
    public Guid ProductId { get; private set; }
    public string ProductName { get; private set; }
    public decimal UnitPrice { get; private set; }
    public int Quantity { get; private set; }
    public decimal SubTotal { get; private set; }

    protected SaleItem() { }

    public SaleItem(Product product, int quantity)
    {
        Id = Guid.NewGuid();
        ProductId = product.Id;
        ProductName = product.Name;
        UnitPrice = product.Price;
        Quantity = quantity;
        SubTotal = product.Price * quantity;
    }
}
