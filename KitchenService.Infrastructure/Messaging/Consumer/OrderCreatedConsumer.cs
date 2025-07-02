using KitchenService.Application.Commands;
using KitchenService.Domain.ValueObjects;
using MassTransit;
using OrderService.Contracts.Events;

namespace KitchenService.Infrastructure.Messaging.Consumer;

public class OrderCreatedConsumer(HandleNewOrderCommandHandler handler) : IConsumer<CreateOrderEvent>
{
    private readonly HandleNewOrderCommandHandler _handler = handler;

    public async Task Consume(ConsumeContext<CreateOrderEvent> context)
    {
        var msg = context.Message;

        var command = new HandleNewOrderCommand
        {
            OrderId = msg.OrderId,
            DeliveryMethod = msg.Mode,
            CreatedAt = DateTime.Now,
            Items = msg.Items
                .Select(i => new OrderItem(i.ProductId, i.Title, i.Description, i.Quantity))
                .ToList()
        };

        await _handler.HandleAsync(command);
    }
}
