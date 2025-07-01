using KitchenService.Application.Interfaces;
using KitchenService.Domain.Entities;

namespace KitchenService.Application.Queries;

public class KitchenOrderQuery(IOrderRepository repository)
{
    private readonly IOrderRepository _repository = repository;

    public Task<IEnumerable<Order>> GetPendingAsync()
    {
        return _repository.GetPendingOrdersAsync();
    }
}
