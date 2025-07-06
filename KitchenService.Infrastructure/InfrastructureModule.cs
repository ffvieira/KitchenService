using KitchenService.Application.Bus;
using KitchenService.Application.Commands.AcceptedRejectedOrder;
using KitchenService.Application.Commands.NewCancelledOrder;
using KitchenService.Application.Interfaces;
using KitchenService.Infrastructure.Messaging.Consumer;
using KitchenService.Infrastructure.Messaging.Publisher;
using KitchenService.Infrastructure.MongoDb;
using KitchenService.Infrastructure.Monitoring;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization;
using MongoDB.Bson;
using System.Text;

namespace KitchenService.Infrastructure
{
    public static class InfraestructureModule
    {
        public static IServiceCollection AddInfraestructureModule(this IServiceCollection services, IConfiguration configuration)
        {
            BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));
            services
                .AddAutentication(configuration)
                .AddRepositories()
                .AddHealthCheck()
                .AddHandlersConsumers()
                .AddMessaging();

            return services;
        }

        private static IServiceCollection AddHealthCheck(this IServiceCollection services)
        {
            services.AddScoped<IHealthCheck, HealthCheck>();
            return services;
        }

        private static IServiceCollection AddAutentication(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtKey = configuration["Jwt:Key"];

            services.AddAuthentication("Bearer")
            .AddJwtBearer("Bearer", options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.ASCII.GetBytes(jwtKey)
                    )
                };
            });

            services.AddAuthorization();

            return services;
        }

        private static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IOrderRepository, OrderRepository>();
            return services;
        }

        private static IServiceCollection AddHandlersConsumers(this IServiceCollection services)
        {
            services.AddScoped<ICommandBus, CommandBus>();

            services.AddScoped<ICommandHandler<NewOrderCommand>, NewOrderCommandHandler>();
            services.AddScoped<ICommandHandler<CanceledOrderCommand>, CanceledOrderCommandHandler>();

            services.AddScoped<NewOrderCommandHandler>();
            services.AddScoped<CanceledOrderCommandHandler>();

            services.AddScoped<AcceptOrderCommandHandler>();
            services.AddScoped<RejectOrderCommandHandler>();
            return services;
        }

        private static IServiceCollection AddMessaging(this IServiceCollection services)
        {
            var envHostRabbitMqServer = Environment.GetEnvironmentVariable("RABBITMQ_HOST") ?? "rabbitmq://localhost:31001";

            services.AddMassTransit(x =>
            {
                x.AddConsumer<OrderCreatedConsumer>();
                x.AddConsumer<OrderCanceledConsumer>();
                x.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Host(envHostRabbitMqServer);

                    cfg.ReceiveEndpoint("kitchen-order-created", e =>
                    {
                        e.ConfigureConsumer<OrderCreatedConsumer>(ctx);
                    });
                });
            });

            services.AddScoped<IOrderStatusPublisher, OrderStatusPublisher>();

            return services;
        }
    }
}

