using KitchenService.Application.Interfaces;

public class RejectOrderCommand
{
    public string OrderId { get; set; } = default!;
    public string Reason { get; set; } = default!;
}

public class RejectOrderCommandHandler
{
    private readonly IOrderRepository _repository;
    private readonly IOrderStatusPublisher _publisher;

    public RejectOrderCommandHandler(IOrderRepository repository, IOrderStatusPublisher publisher)
    {
        _repository = repository;
        _publisher = publisher;
    }

    public async Task HandleAsync(RejectOrderCommand command)
    {
        var order = await _repository.GetByIdAsync(command.OrderId)
            ?? throw new InvalidOperationException("Order not found.");

        order.Reject(command.Reason);
        await _repository.UpdateAsync(order);
        await _publisher.PublishRejectedAsync(order.Id, command.Reason);
    }
}
