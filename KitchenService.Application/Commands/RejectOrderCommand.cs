using KitchenService.Application.Interfaces;

namespace KitchenService.Application.Commands
{
    public class RejectOrderCommand
    {
        public string OrderId { get; set; } = default!;
        public string Reason { get; set; } = default!;
    }

    public class RejectOrderCommandHandler(IOrderRepository repository, IOrderStatusPublisher publisher)
    {
        private readonly IOrderRepository _repository = repository;
        private readonly IOrderStatusPublisher _publisher = publisher;

        public async Task HandleAsync(RejectOrderCommand command)
        {
            var order = await _repository.GetByIdAsync(command.OrderId)
                ?? throw new InvalidOperationException("Pedido não encontrado.");

            order.Reject(command.Reason);
            await _repository.UpdateAsync(order);
            await _publisher.PublishRejectedAsync(order.Id, command.Reason);
        }
    }
}