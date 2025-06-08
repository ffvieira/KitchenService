using KitchenService.Domain.Enums;
using KitchenService.Domain.ValueObjects;

namespace KitchenService.Domain.Entities;

public class Order
{
    public string Id { get; private set; }
    public List<OrderItem> Items { get; private set; }
    public string DeliveryMethod { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public OrderStatus Status { get; private set; }
    public string? RejectionReason { get; private set; }

    public Order(string id, List<OrderItem> items, string deliveryMethod, DateTime createdAt)
    {
        Id = id;
        Items = items;
        DeliveryMethod = deliveryMethod;
        CreatedAt = createdAt;
        Status = OrderStatus.Pending;
    }

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
