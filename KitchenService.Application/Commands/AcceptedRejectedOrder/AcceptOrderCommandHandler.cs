using KitchenService.Application.Interfaces;
using KitchenService.Domain.Enums;

namespace KitchenService.Application.Commands.AcceptedRejectedOrder
{
    public class AcceptOrderCommandHandler(IOrderRepository repository, IOrderStatusPublisher publisher)
    {
        private readonly IOrderRepository _repository = repository;
        private readonly IOrderStatusPublisher _publisher = publisher;

        public async Task HandleAsync(OrderCommand command)
        {
            var order = await _repository.GetByIdAsync(command.OrderId)
                ?? throw new InvalidOperationException("Pedido não encontrado.");

            order.ChangeStatus(OrderStatus.Accepted);
            await _repository.UpdateAsync(order);
            await _publisher.PublishOrderStatusAsync(command.OrderId, command.Status);
        }
    }
}