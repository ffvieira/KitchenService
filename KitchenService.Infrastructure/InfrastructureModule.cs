using KitchenService.Application.Interfaces;
using KitchenService.Infrastructure.Messaging.Consumer;
using KitchenService.Infrastructure.Messaging.Publisher;
using KitchenService.Infrastructure.MongoDb;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace KitchenService.Infrastructure
{
    public static class InfraestructureModule
    {
        public static IServiceCollection AddInfraestructureModule(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddAutentication(configuration)
                .AddRepositories()
                .AddMessaging();

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

        private static IServiceCollection AddMessaging(this IServiceCollection services)
        {
            var envHostRabbitMqServer = Environment.GetEnvironmentVariable("RABBITMQ_HOST") ?? "localhost";

            services.AddMassTransit(x =>
            {
                x.AddConsumer<OrderCreatedConsumer>();
                x.AddConsumer<OrderCanceledConsumer>();
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

            services.AddScoped<IOrderStatusPublisher, OrderStatusPublisher>();

            return services;
        }
    }
}

