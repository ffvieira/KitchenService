using KitchenService.Infrastructure.Monitoring;
using MassTransit;

namespace KitchenService.Worker
{
    public class Worker(IBusControl bus, IHealthCheck healthCheck) : BackgroundService
    {
        private readonly IBusControl _bus = bus;
        private readonly IHealthCheck _healthCheck = healthCheck;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            if (!await _healthCheck.IsMongoDbHealthyAsync(stoppingToken) || !await _healthCheck.IsRabbitMqHealthyAsync())
            {
                Console.WriteLine("Falha na verificação de Health check. Encerrando aplicação.");
                Environment.Exit(1);
            }

            await _bus.StartAsync(stoppingToken);
            Console.WriteLine("KitchenService Worker iniciado.");
            await Task.Delay(Timeout.Infinite, stoppingToken);
        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            await _bus.StopAsync(stoppingToken);
            Console.WriteLine("KitchenService Worker parado.");
        }
    }
}