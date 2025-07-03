namespace KitchenService.Application.Commands.NewCancelledOrder
{
    public class CanceledOrderCommand
    {
        public Guid OrderId { get; set; } = default!;
        public string Justification { get; set; } = string.Empty;
        public DateTime UpdatedAt { get; set; }
    }
}
