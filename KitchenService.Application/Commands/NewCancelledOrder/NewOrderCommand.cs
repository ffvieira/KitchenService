using KitchenService.Domain.ValueObjects;
using OrderService.Contracts.Enums;

namespace KitchenService.Application.Commands.NewCancelledOrder;

public class NewOrderCommand
{
    public Guid OrderId { get; set; } = default!;
    public List<OrderItem> Items { get; set; } = [];
    public OrderMode DeliveryMethod { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
}
