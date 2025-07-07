using KitchenService.Application.Abstractions;
using KitchenService.Application.Interfaces;
using KitchenService.Domain.Enums;

namespace KitchenService.Application.Commands.AcceptedRejectedOrder
{
    public class AcceptOrderCommandHandler(IOrderRepository repository, IOrderStatusPublisher publisher)
    {
        private readonly IOrderRepository _repository = repository;
        private readonly IOrderStatusPublisher _publisher = publisher;

        public async Task<Result<Guid>> HandleAsync(OrderCommand command)
        {
            var order = await _repository.GetByIdAsync(command.OrderId);

            if (order is null)
            {
                return Result<Guid>.Failure("Pedido não encontrado.");
            }

            if(!order.ChangeStatus(OrderStatus.Accepted))
            {
                return Result<Guid>.Failure("Só é possível aceitar os pedidos que estão como pendentes");
            }

            await _repository.UpdateAsync(order);
            await _publisher.PublishOrderStatusAsync(command.OrderId, command.Status);

            return Result<Guid>.Success(order.Id);
        }
    }
}