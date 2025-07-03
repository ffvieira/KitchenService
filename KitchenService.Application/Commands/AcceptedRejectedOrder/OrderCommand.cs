using OrderService.Contracts.Enums;

namespace KitchenService.Application.Commands.AcceptedRejectedOrder
{
    public class OrderCommand(Guid orderId, AcceptOrRejectOrderEnum status)
    {
        public Guid OrderId { get; set; } = orderId;
        public AcceptOrRejectOrderEnum Status { get; set; } = status;
    }
}
