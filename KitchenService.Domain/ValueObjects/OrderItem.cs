namespace KitchenService.Domain.ValueObjects;

public class OrderItem(Guid productId, string title, string description, int quantity)
{
    public Guid ProductId { get; private set; } = productId;
    public string Title { get; private set; } = title;
    public string Description { get; private set; } = description;
    public int Quantity { get; private set; } = quantity;
}