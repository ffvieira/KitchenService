namespace KitchenService.Application.Interfaces
{

    public interface IOrderStatusPublisher
    {
        Task PublishAcceptedAsync(string orderId);
        Task PublishRejectedAsync(string orderId, string reason);
    }
}
