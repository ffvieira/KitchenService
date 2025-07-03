using KitchenService.Application.Interfaces;

namespace KitchenService.Application.Commands.NewCancelledOrder
{
    public class CanceledOrderCommandHandler(IOrderRepository repository) : ICommandHandler<CanceledOrderCommand>
    {
        private readonly IOrderRepository _repository = repository;

        public async Task HandleAsync(CanceledOrderCommand command)
        {
            var existingOrder = await _repository.GetByIdAsync(command.OrderId) ??
                throw new InvalidOperationException("Pedido não encontrado.");

            existingOrder.OrderCanceled(
                canceled: true,
                justification: command.Justification,
                updatedAt: command.UpdatedAt
            );

            await _repository.UpdateAsync(existingOrder);
        }
    }
}
