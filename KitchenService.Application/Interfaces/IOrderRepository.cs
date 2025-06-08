using KitchenService.Domain.Entities;

namespace KitchenService.Application.Interfaces;

public interface IOrderRepository
{
    Task AddAsync(Order order);
    Task<Order?> GetByIdAsync(string orderId);
    Task<IEnumerable<Order>> GetPendingOrdersAsync();
    Task UpdateAsync(Order order);
}
