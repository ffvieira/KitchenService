namespace KitchenService.Infrastructure.Messaging.Publisher;

public class OrderRejectedEvent : BaseOrderEvent
{
    public string Reason { get; set; } = default!;
}
