using KitchenService.Application.Commands;
using KitchenService.Application.Interfaces;
using KitchenService.Domain.Entities;
using KitchenService.Domain.Enums;
using KitchenService.Domain.ValueObjects;
using Moq;
using Xunit;

namespace KitchenService.UnitTests.Commands;

public class RejectOrderCommandHandlerTests
{
    [Fact]
    public async Task HandleAsync_Should_Set_Status_To_Rejected_And_Publish_Event()
    {
        // Arrange
        var order = new Order("123", [new("x-salada", "X-Salada", "Sanduíche de carne com queijo e salada", 1)], "delivery", DateTime.UtcNow);
        var repoMock = new Mock<IOrderRepository>();
        repoMock.Setup(r => r.GetByIdAsync("123")).ReturnsAsync(order);

        var pubMock = new Mock<IOrderStatusPublisher>();
        var handler = new RejectOrderCommandHandler(repoMock.Object, pubMock.Object);

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
        repoMock.Verify(r => r.UpdateAsync(order), Times.Once);
        pubMock.Verify(p => p.PublishRejectedAsync("123", "Fora de estoque"), Times.Once);
    }
}
