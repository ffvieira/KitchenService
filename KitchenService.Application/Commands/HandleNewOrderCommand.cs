using KitchenService.Application.Interfaces;
using KitchenService.Domain.Entities;
using KitchenService.Domain.ValueObjects;

namespace KitchenService.Application.Commands;

public class HandleNewOrderCommand
{
    public string OrderId { get; set; } = default!;
    public List<OrderItem> Items { get; set; } = [];
    public string DeliveryMethod { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
}

public class HandleNewOrderCommandHandler(IOrderRepository repository)
{
    private readonly IOrderRepository _repository = repository;

    public async Task HandleAsync(HandleNewOrderCommand command)
    {
        var existing = await _repository.GetByIdAsync(command.OrderId);
        if (existing != null)
            throw new InvalidOperationException("Pedido já foi recebido.");

        var order = new Order(
            id: command.OrderId,
            items: command.Items,
            deliveryMethod: command.DeliveryMethod,
            createdAt: command.CreatedAt
        );

        await _repository.AddAsync(order);
    }
}
