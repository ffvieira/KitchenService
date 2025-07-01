using KitchenService.Domain.Enums;
using KitchenService.Domain.ValueObjects;

namespace KitchenService.Domain.Entities;

public class Order(string id, List<OrderItem> items, string deliveryMethod, DateTime createdAt)
{
    public string Id { get; private set; } = id;
    public List<OrderItem> Items { get; private set; } = items;
    public string DeliveryMethod { get; private set; } = deliveryMethod;
    public DateTime CreatedAt { get; private set; } = createdAt;
    public OrderStatus Status { get; private set; } = OrderStatus.Pending;
    public string? RejectionReason { get; private set; }

    public void Accept()
    {
        if (Status != OrderStatus.Pending)
            throw new InvalidOperationException("Order is not in a state that can be accepted.");
        Status = OrderStatus.Accepted;
    }

    public void Reject(string reason)
    {
        if (Status != OrderStatus.Pending)
            throw new InvalidOperationException("Order is not in a state that can be rejected.");
        Status = OrderStatus.Rejected;
        RejectionReason = reason;
    }
}
