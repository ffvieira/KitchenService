using KitchenService.Application.Interfaces;
using KitchenService.Domain.Entities;
using KitchenService.Domain.Enums;
using NSubstitute;
using Xunit;

namespace KitchenService.Tests.Application;

public class AcceptOrderCommandHandlerTests
{
    [Fact]
    public async Task HandleAsync_Should_Set_Status_To_Accepted_And_Publish_Event()
    {
        // Arrange
        var order = new Order("123", [new("burger01", "Cheeseburger", "Burguer and cheese", 2)], "balcão", DateTime.UtcNow);
        var repo = Substitute.For<IOrderRepository>();
        repo.GetByIdAsync(order.Id).Returns(order);

        var publisher = Substitute.For<IOrderStatusPublisher>();
        var handler = new AcceptOrderCommandHandler(repo, publisher);

        // Act
        await handler.HandleAsync(new AcceptOrderCommand { OrderId = "123" });

        // Assert
        Assert.Equal(OrderStatus.Accepted, order.Status);
        await repo.Received(1).UpdateAsync(order);
        await publisher.Received(1).PublishAcceptedAsync(order.Id);
    }
}
