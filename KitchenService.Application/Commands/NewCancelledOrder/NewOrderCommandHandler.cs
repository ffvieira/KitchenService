using KitchenService.Application.Interfaces;
using KitchenService.Domain.Entities;

namespace KitchenService.Application.Commands.NewCancelledOrder
{
    public class NewOrderCommandHandler(IOrderRepository repository) : ICommandHandler<NewOrderCommand>
    {
        private readonly IOrderRepository _repository = repository;

        public async Task HandleAsync(NewOrderCommand command)
        {
            var existing = await _repository.GetByIdAsync(command.OrderId);
            if (existing != null)
                throw new InvalidOperationException("Pedido já foi recebido.");

            var order = new Order(
                id: command.OrderId,
                items: command.Items,
                deliveryMethod: command.DeliveryMethod.ToString(),
                createdAt: command.CreatedAt
            );

            await _repository.AddAsync(order);
        }
    }
}
