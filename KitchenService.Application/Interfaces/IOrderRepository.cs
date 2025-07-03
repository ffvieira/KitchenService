using KitchenService.Domain.Entities;

namespace KitchenService.Application.Interfaces;

public interface IOrderRepository
{
    Task AddAsync(Order order);
    Task<Order?> GetByIdAsync(Guid orderId);
    Task<IEnumerable<Order>> GetPendingOrdersAsync();
    Task<IEnumerable<Order>> GetOrdersAsync();
    Task UpdateAsync(Order order);
}
