namespace KitchenService.Application.Interfaces
{
    public interface ICommandBus
    {
        Task SendAsync<TCommand>(TCommand command);
    }
}
