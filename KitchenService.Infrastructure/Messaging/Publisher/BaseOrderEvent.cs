namespace KitchenService.Infrastructure.Messaging.Publisher
{
    public abstract class BaseOrderEvent
    {
        public string OrderId { get; set; } = default!;
        public DateTime ProcessedAt { get; set; } = DateTime.UtcNow;
    }
}
