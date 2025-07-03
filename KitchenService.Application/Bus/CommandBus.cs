using KitchenService.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace KitchenService.Application.Bus;

public class CommandBus(IServiceProvider serviceProvider) : ICommandBus
{
    public async Task SendAsync<TCommand>(TCommand command)
    {
        var handler = serviceProvider.GetRequiredService<ICommandHandler<TCommand>>();
        await handler.HandleAsync(command);
    }
}
