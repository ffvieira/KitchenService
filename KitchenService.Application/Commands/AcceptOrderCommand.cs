using KitchenService.Application.Interfaces;

namespace KitchenService.Application.Commands
{
    public class AcceptOrderCommand
    {
        public string OrderId { get; set; } = default!;
    }

    public class AcceptOrderCommandHandler(IOrderRepository repository, IOrderStatusPublisher publisher)
    {
        private readonly IOrderRepository _repository = repository;
        private readonly IOrderStatusPublisher _publisher = publisher;

        public async Task HandleAsync(AcceptOrderCommand command)
        {
            var order = await _repository.GetByIdAsync(command.OrderId)
                ?? throw new InvalidOperationException("Pedido não encontrado.");

            order.Accept();
            await _repository.UpdateAsync(order);
            await _publisher.PublishAcceptedAsync(order.Id);
        }
    }
}