namespace KitchenService.Domain;

public class OrderItem
{
    public string ProductId { get; private set; } = default!;
    public string Name { get; private set; } = default!;
    public string Description { get; private set; } = default!;
    public int Quantity { get; private set; }

    public OrderItem(string productId, string name, string description, int quantity)
    {
        ProductId = productId;
        Name = name;
        Description = description;
        Quantity = quantity;
    }
}