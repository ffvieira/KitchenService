using KitchenService.Application.Interfaces;

public class AcceptOrderCommand
{
    public string OrderId { get; set; } = default!;
}

public class AcceptOrderCommandHandler
{
    private readonly IOrderRepository _repository;
    private readonly IOrderStatusPublisher _publisher;

    public AcceptOrderCommandHandler(IOrderRepository repository, IOrderStatusPublisher publisher)
    {
        _repository = repository;
        _publisher = publisher;
    }

    public async Task HandleAsync(AcceptOrderCommand command)
    {
        var order = await _repository.GetByIdAsync(command.OrderId)
            ?? throw new InvalidOperationException("Order not found.");

        order.Accept();
        await _repository.UpdateAsync(order);
        await _publisher.PublishAcceptedAsync(order.Id);
    }
}
