using KitchenService.Application.Interfaces;
using MassTransit;

namespace KitchenService.Infrastructure.Messaging.Publisher;

public class OrderStatusPublisher : IOrderStatusPublisher
{
    private readonly IPublishEndpoint _publishEndpoint;

    public OrderStatusPublisher(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public Task PublishAcceptedAsync(string orderId)
    {
        var evt = new OrderAcceptedEvent
        {
            OrderId = orderId,
            ProcessedAt = DateTime.UtcNow
        };

        return _publishEndpoint.Publish(evt);
    }

    public Task PublishRejectedAsync(string orderId, string reason)
    {
        var evt = new OrderRejectedEvent
        {
            OrderId = orderId,
            Reason = reason,
            ProcessedAt = DateTime.UtcNow
        };

        return _publishEndpoint.Publish(evt);
    }
}
