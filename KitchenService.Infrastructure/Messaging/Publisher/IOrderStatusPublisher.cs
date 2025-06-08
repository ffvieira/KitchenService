namespace KitchenService.Infrastructure.Messaging.Publisher
{

    public interface IOrderStatusPublisher
    {
        Task PublishAcceptedAsync(string orderId);
        Task PublishRejectedAsync(string orderId, string reason);
    }
}
