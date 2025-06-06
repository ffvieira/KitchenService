namespace KitchenService.Domain.Entities;

public class Order
{
    public string Id { get; private set; }
    public List<OrderItem> Items { get; private set; }
    public string DeliveryMethod { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public OrderStatus Status { get; private set; }
    public string? RejectedReason { get; private set; }

    public Order(string id, List<OrderItem> items, string deliveryMethod, DateTime createdAt, OrderStatus status, string? rejectedReason)
    {
        Id = id;
        Items = items ?? throw new ArgumentNullException(nameof(items));
        DeliveryMethod = deliveryMethod ?? throw new ArgumentNullException(nameof(deliveryMethod));
        CreatedAt = createdAt;
        Status = status;
        RejectedReason = rejectedReason;
    }

    public void Accept()
    {
        if (Status != OrderStatus.Pending)
            throw new InvalidOperationException("Only pending orders can be accepted.");

        Status = OrderStatus.Accepted;
    }

    public void Reject(string reason)
    {
        if (Status != OrderStatus.Pending)
            throw new InvalidOperationException("Only pending orders can be rejected.");

        Status = OrderStatus.Rejected;
        RejectedReason = reason ?? throw new ArgumentNullException(nameof(reason));
    }
}

