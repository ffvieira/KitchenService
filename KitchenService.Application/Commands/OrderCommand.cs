using OrderService.Contracts.Enums;

namespace KitchenService.Application.Commands
{
    public class OrderCommand
    {
        public Guid OrderId { get; set; } = default!;
        public AcceptOrRejectOrderEnum Status { get; set; }
    }
}
