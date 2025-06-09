using KitchenService.Application.Commands;
using KitchenService.Application.Interfaces;
using KitchenService.Domain.Entities;
using KitchenService.Domain.Enums;
using KitchenService.Domain.ValueObjects;
using Moq;
using Xunit;

namespace KitchenService.UnitTests.Commands;

public class AcceptOrderCommandHandlerTests
{
    [Fact]
    public async Task HandleAsync_Should_Set_Status_To_Accepted_And_Publish_Event()
    {
        // Arrange
        var order = new Order("123", new List<OrderItem> { new("burger01", "Cheeseburger", "Burguer and cheese", 2) }, "balcão", DateTime.UtcNow);
        var repoMock = new Mock<IOrderRepository>();
        repoMock.Setup(r => r.GetByIdAsync("123")).ReturnsAsync(order);

        var pubMock = new Mock<IOrderStatusPublisher>();
        var handler = new AcceptOrderCommandHandler(repoMock.Object, pubMock.Object);

        // Act
        await handler.HandleAsync(new AcceptOrderCommand { OrderId = "123" });

        // Assert
        Assert.Equal(OrderStatus.Accepted, order.Status);
        repoMock.Verify(r => r.UpdateAsync(order), Times.Once);
        pubMock.Verify(p => p.PublishAcceptedAsync("123"), Times.Once);
    }
}
