using KitchenService.Application.Commands;
using KitchenService.Application.Interfaces;
using KitchenService.Application.Queries;
using KitchenService.Infrastructure.Messaging.Consumer;
using KitchenService.Infrastructure.Messaging.Publisher;
using KitchenService.Infrastructure.MongoDb;
using MassTransit;
using Microsoft.AspNetCore.Routing;

var builder = WebApplication.CreateBuilder(args);

// 🔧 Configurações de serviços
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<RouteOptions>(o => o.LowercaseUrls = true);

// 📦 Repositórios e Handlers
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<HandleNewOrderCommandHandler>();
builder.Services.AddScoped<AcceptOrderCommandHandler>();
builder.Services.AddScoped<RejectOrderCommandHandler>();
builder.Services.AddScoped<KitchenOrderQuery>();

// 📤 Publicador de eventos
builder.Services.AddScoped<IOrderStatusPublisher, OrderStatusPublisher>();

// 🐇 MassTransit com RabbitMQ
builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<OrderCreatedConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("rabbitmq", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

        cfg.ReceiveEndpoint("kitchen-order-created", e =>
        {
            e.ConfigureConsumer<OrderCreatedConsumer>(context);
        });
    });
});

// 🔧 Constrói a app
var app = builder.Build();

// 🚀 Pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();

app.Run();
