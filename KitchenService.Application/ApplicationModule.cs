using KitchenService.Application.Bus;
using KitchenService.Application.Commands.AcceptedRejectedOrder;
using KitchenService.Application.Commands.NewCancelledOrder;
using KitchenService.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace KitchenService.Application
{
    public static class ApplicationModule
    {
        public static IServiceCollection AddApplicationModule(this IServiceCollection services)
        {
            services.AddScoped<ICommandBus, CommandBus>();
            services.AddScoped<ICommandHandler<NewOrderCommand>, NewOrderCommandHandler>();
            services.AddScoped<ICommandHandler<CanceledOrderCommand>, CanceledOrderCommandHandler>();
            services.AddScoped<AcceptOrderCommandHandler>();
            services.AddScoped<RejectOrderCommandHandler>();

            return services;
        }
    }
}
