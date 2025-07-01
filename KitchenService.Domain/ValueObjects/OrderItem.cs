namespace KitchenService.Domain.ValueObjects;

public class OrderItem(string productId, string name, string description, int quantity)
{
    public string ProductId { get; private set; } = productId;
    public string Name { get; private set; } = name;
    public string Description { get; private set; } = description;
    public int Quantity { get; private set; } = quantity;
}