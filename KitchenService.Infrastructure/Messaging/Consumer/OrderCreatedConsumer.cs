using KitchenService.Application.Commands.NewCancelledOrder;
using KitchenService.Domain.ValueObjects;
using MassTransit;
using OrderService.Contracts.Events;

namespace KitchenService.Infrastructure.Messaging.Consumer;

public class OrderCreatedConsumer(NewOrderCommandHandler handler) : IConsumer<CreateOrderEvent>
{
    private readonly NewOrderCommandHandler _handler = handler;

    public async Task Consume(ConsumeContext<CreateOrderEvent> context)
    {
        var msg = context.Message;

        var command = new NewOrderCommand
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
