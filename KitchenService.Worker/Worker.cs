using MassTransit;

public class Worker : BackgroundService
{
    private readonly IBusControl _bus;

    public Worker(IBusControl bus)
    {
        _bus = bus;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _bus.StartAsync(stoppingToken);
        Console.WriteLine("KitchenService Worker started.");
        await Task.Delay(Timeout.Infinite, stoppingToken);
    }

    public override async Task StopAsync(CancellationToken stoppingToken)
    {
        await _bus.StopAsync(stoppingToken);
        Console.WriteLine("KitchenService Worker stopped.");
    }
}
