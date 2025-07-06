using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using RabbitMQ.Client;

namespace KitchenService.Infrastructure.Monitoring;

public interface IHealthCheck
{
    Task<bool> IsMongoDbHealthyAsync(CancellationToken cancellationToken = default);
}

public class HealthCheck : IHealthCheck
{
    private readonly ILogger<HealthCheck> _logger;
    private readonly MongoClient _mongoClient;
    private readonly IConfiguration _configuration;

    public HealthCheck(IConfiguration configuration, ILogger<HealthCheck> logger)
    {
        _configuration = configuration;
        _logger = logger;
        var connectionString = _configuration.GetConnectionString("MongoDb")!;
        _mongoClient = new MongoClient(connectionString);
    }

    public async Task<bool> IsMongoDbHealthyAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            await _mongoClient.GetDatabase("admin")
                              .RunCommandAsync((Command<BsonDocument>)"{ping:1}", cancellationToken: cancellationToken);
            _logger.LogInformation("MongoDB conectado com sucesso");
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao conectar ao MongoDB");
            return false;
        }
    }
}

