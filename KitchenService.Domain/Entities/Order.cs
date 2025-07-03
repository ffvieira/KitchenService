using KitchenService.Domain.Enums;
using KitchenService.Domain.ValueObjects;

namespace KitchenService.Domain.Entities;

public class Order(Guid id, List<OrderItem> items, string deliveryMethod, DateTime createdAt)
{
    public Guid Id { get; private set; } = id;
    public List<OrderItem> Items { get; private set; } = items;
    public string DeliveryMethod { get; private set; } = deliveryMethod;
    public DateTime CreatedAt { get; private set; } = createdAt;
    public DateTime? UpdatedAt { get; private set; }
    public OrderStatus Status { get; private set; } = OrderStatus.Pending;
    public bool Canceled { get; private set; } = false;
    public string? CanceledJustification { get; private set; }

    public void ChangeStatus(OrderStatus status)
    {
        if (Status != OrderStatus.Pending)
            throw new InvalidOperationException("Order is not in a state that can be accepted.");
        Status = status;
    }

    public void OrderCanceled(bool canceled, string justification, DateTime updatedAt)
    {
        Canceled = canceled;
        CanceledJustification = justification;
        UpdatedAt = updatedAt;
    }
}
