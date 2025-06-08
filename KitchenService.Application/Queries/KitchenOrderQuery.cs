using KitchenService.Application.Interfaces;
using KitchenService.Domain.Entities;

namespace KitchenService.Application.Queries;

public class KitchenOrderQuery
{
    private readonly IOrderRepository _repository;

    public KitchenOrderQuery(IOrderRepository repository)
    {
        _repository = repository;
    }

    public Task<IEnumerable<Order>> GetPendingAsync()
    {
        return _repository.GetPendingOrdersAsync();
    }
}
