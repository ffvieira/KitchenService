using KitchenService.Application.Interfaces;
using KitchenService.Domain.Entities;
using KitchenService.Domain.Enums;
using NSubstitute;
using Xunit;

namespace KitchenService.UnitTests.Commands;

public class RejectOrderCommandHandlerTests
{
    [Fact]
    public async Task HandleAsync_Should_Set_Status_To_Rejected_And_Publish_Event()
    {
        // Arrange
        var order = new Order("123", [new("x-salada", "X-Salada", "Sanduíche de carne com queijo e salada", 1)], "delivery", DateTime.UtcNow);
        var repo = Substitute.For<IOrderRepository>();
        repo.GetByIdAsync(order.Id).Returns(order);

        var pubMock = Substitute.For<IOrderStatusPublisher>();
        var handler = new RejectOrderCommandHandler(repo, pubMock);

        var command = new RejectOrderCommand
        {
            OrderId = "123",
            Reason = "Fora de estoque"
        };

        // Act
        await handler.HandleAsync(command);

        // Assert
        Assert.Equal(OrderStatus.Rejected, order.Status);
        Assert.Equal("Fora de estoque", order.RejectionReason);
        await repo.Received(1).UpdateAsync(order);
        await pubMock.Received(1).PublishRejectedAsync(order.Id, "Fora de estoque");
    }
}
