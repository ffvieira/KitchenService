using KitchenService.Application.Commands;
using KitchenService.Domain.ValueObjects;
using MassTransit;

namespace KitchenService.Infrastructure.Messaging.Consumer;

public class OrderCreatedConsumer(HandleNewOrderCommandHandler handler) : IConsumer<OrderCreatedEvent>
{
    private readonly HandleNewOrderCommandHandler _handler = handler;

    public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
    {
        var msg = context.Message;

        var command = new HandleNewOrderCommand
        {
            OrderId = msg.OrderId,
            DeliveryMethod = msg.DeliveryMethod,
            CreatedAt = msg.CreatedAt,
            Items = msg.Items
                .Select(i => new OrderItem(i.ProductId, i.Name, i.Description, i.Quantity))
                .ToList()
        };

        await _handler.HandleAsync(command);
    }
}
