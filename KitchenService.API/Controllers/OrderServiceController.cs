using KitchenService.Infrastructure.Monitoring;
using MassTransit.Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace KitchenService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderServiceController(ILogger<OrderServiceController> logger, IHealthCheck healthCheck) : ControllerBase
    {
        private readonly ILogger<OrderServiceController> _logger = logger;
        private readonly IHealthCheck _healthCheck = healthCheck;

        [HttpGet("health")]
        public async Task<IActionResult> GetHealthAsync(CancellationToken cancellationToken)
        {
            if (!await _healthCheck.IsMongoDbHealthyAsync(cancellationToken) || !await _healthCheck.IsRabbitMqHealthyAsync())
            {
                Console.WriteLine("Falha na verificação de Health check. Encerrando aplicação.");
                Environment.Exit(1);
            }

            _logger.LogInformation("Health check endpoint called");
            return Ok(new { Status = "Healthy" });
        }

        [HttpGet("Orders")]
        [Authorize(Roles = "Cliente")]

        public async Task<IActionResult> GetOrdersAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("GetOrders endpoint called");
            // Here you would typically call a service to get the orders
            // For now, we return a placeholder response
            return Ok(new { Message = "This endpoint will return orders in the future." });
        }
    }
}
