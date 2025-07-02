using OrderService.Contracts.Enums;

namespace KitchenService.Application.Interfaces
{

    public interface IOrderStatusPublisher
    {
        Task PublishOrderStatusAsync(Guid orderId, AcceptOrRejectOrderEnum status);
    }
}
