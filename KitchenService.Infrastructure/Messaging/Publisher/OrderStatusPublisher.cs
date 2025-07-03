using KitchenService.Application.Interfaces;
using MassTransit;
using OrderService.Contracts.Enums;
using OrderService.Contracts.Events;

namespace KitchenService.Infrastructure.Messaging.Publisher;

public class OrderStatusPublisher(IPublishEndpoint publishEndpoint) : IOrderStatusPublisher
{
    private readonly IPublishEndpoint _publishEndpoint = publishEndpoint;

    public Task PublishOrderStatusAsync(Guid orderId, AcceptOrRejectOrderEnum status)
    {        
        if (orderId == Guid.Empty)
        {
            throw new ArgumentException("Order ID cannot be empty.", nameof(orderId));
        }
        
        var evt = new AcceptOrRejectOrderEvent
        {
            OrderId = orderId,
            Status = AcceptOrRejectOrderEnum.Accepted,
        };

        return _publishEndpoint.Publish(evt);
    }
}
