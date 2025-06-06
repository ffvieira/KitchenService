namespace KitchenService.Application;

public interface IKitchenOrderService
{
    Task HandleNewOrderAsync(OrderMessage order);
    IEnumerable<OrderMessage> GetPendingOrders();
    Task AcceptOrderAsync(string orderId);
    Task RejectOrderAsync(string order, string reason);
}