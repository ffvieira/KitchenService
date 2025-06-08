using KitchenService.Application.Queries;
using Microsoft.AspNetCore.Mvc;

namespace KitchenService.API.Controllers;

[ApiController]
[Route("api/orders")]
public class OrdersController : ControllerBase
{
    private readonly KitchenOrderQuery _query;
    private readonly AcceptOrderCommandHandler _acceptHandler;
    private readonly RejectOrderCommandHandler _rejectHandler;

    public OrdersController(
        KitchenOrderQuery query,
        AcceptOrderCommandHandler acceptHandler,
        RejectOrderCommandHandler rejectHandler)
    {
        _query = query;
        _acceptHandler = acceptHandler;
        _rejectHandler = rejectHandler;
    }

    [HttpGet("pending")]
    public async Task<IActionResult> GetPendingOrders()
    {
        var result = await _query.GetPendingAsync();
        return Ok(result);
    }

    [HttpPost("{orderId}/accept")]
    public async Task<IActionResult> Accept(string orderId)
    {
        await _acceptHandler.HandleAsync(new AcceptOrderCommand { OrderId = orderId });
        return Ok(new { message = $"Order {orderId} accepted" });
    }

    [HttpPost("{orderId}/reject")]
    public async Task<IActionResult> Reject(string orderId, [FromBody] RejectRequest body)
    {
        await _rejectHandler.HandleAsync(new RejectOrderCommand
        {
            OrderId = orderId,
            Reason = body.Reason
        });

        return Ok(new { message = $"Order {orderId} rejected", body.Reason });
    }

    public class RejectRequest
    {
        public string Reason { get; set; } = string.Empty;
    }
}
