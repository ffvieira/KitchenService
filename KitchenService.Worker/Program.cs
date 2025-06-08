using KitchenService.Application.Commands;
using KitchenService.Application.Interfaces;
using KitchenService.Infrastructure.Messaging.Consumer;
using KitchenService.Infrastructure.Messaging.Publisher;
using KitchenService.Infrastructure.MongoDb;
using MassTransit;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IOrderStatusPublisher, OrderStatusPublisher>();
        services.AddScoped<HandleNewOrderCommandHandler>();

        services.AddMassTransit(x =>
        {
            x.AddConsumer<OrderCreatedConsumer>();
            x.UsingRabbitMq((ctx, cfg) =>
            {
                cfg.Host("rabbitmq", "/", h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });

                cfg.ReceiveEndpoint("kitchen-order-created", e =>
                {
                    e.ConfigureConsumer<OrderCreatedConsumer>(ctx);
                });
            });
        });

        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();
