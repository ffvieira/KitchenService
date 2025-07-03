using KitchenService.Application.Commands.NewCancelledOrder;
using MassTransit;
using OrderService.Contracts.Events;

namespace KitchenService.Infrastructure.Messaging.Consumer
{
    public class OrderCanceledConsumer(CanceledOrderCommandHandler handler) : IConsumer<CancelOrderEvent>
    {
        private readonly CanceledOrderCommandHandler _handler = handler;

        public async Task Consume(ConsumeContext<CancelOrderEvent> context)
        {
            var msg = context.Message;

            var command = new CanceledOrderCommand
            {
                OrderId = msg.OrderId,
                Justification = msg.Justification,
                UpdatedAt = DateTime.Now
            };

            await _handler.HandleAsync(command);
        }
    }
}
