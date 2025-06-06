namespace KitchenService.Application;

public class KitchenOrderService(ILogger<KitchenOrderService> logger) : IKitchenOrderService
{
    private readonly ILogger<KitchenOrderService> _logger = logger;

    //simulate in memory
    private static readonly ConcurrentDictionary<string, OrderMessage> _pendingOrders = new();

    public Task HandleNewOrderAsync(OrderMessage order)
    {
        _pendingOrders[order.OrderId] = order;
        _logger.LogInformation($"Order {order.OrderId} added to pending list");
        return Task.CompletedTask;
    }

    public IEnumerable<OrderMessage> GetPendingOrders()
    {
        return _pendingOrders.Values;
    }

    public Task AcceptOrderAsync(string orderId)
    {
        if (_pendingOrders.TryRemove(orderId, out _))
        {
            _logger.LogInformation($"Order {orderId} accepted");
        }
        else
        {
            _logger.LogWarning($"Order {orderId} not found to accept");
        }

        return Task.CompletedTask;
    }

        public Task AcceptOrderAsync(string orderId, string reason)
    {
        if (_pendingOrders.TryRemove(orderId, out _))
        {
            _logger.LogInformation($"Order {orderId} rejected: {reason}");
        }
        else
        {
            _logger.LogWarning($"Order {orderId} not found to reject");
        }

        return Task.CompletedTask;
    }
}