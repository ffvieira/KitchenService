namespace KitchenService.Infrastructure.Messaging.Consumer;

public class OrderCreatedEvent
{
    public string OrderId { get; set; } = default!;
    public List<OrderItemDto> Items { get; set; } = [];
    public string DeliveryMethod { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
}

public class OrderItemDto
{
    public string ProductId { get; set; } = default!;
    public int Quantity { get; set; }
    public string Description { get; private set; } = default!;
    public string Name { get; private set; } = default!;
}
