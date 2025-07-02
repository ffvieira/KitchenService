using KitchenService.Application.Commands;
using KitchenService.Application.Interfaces;
using KitchenService.Domain.Entities;
using KitchenService.Domain.Enums;
using NSubstitute;

namespace KitchenService.Tests.Application;

public class AcceptOrderCommandHandlerTests
{
    [Fact]
    public async Task HandleAsync_Should_Set_Status_To_Accepted_And_Publish_Event()
    {
        // Arrange
        var order = new Order(Guid.NewGuid(), [new(Guid.NewGuid(), "Cheeseburger", "Burguer and cheese", 2)], "balcão", DateTime.UtcNow);
        var repo = Substitute.For<IOrderRepository>();
        repo.GetByIdAsync(order.Id).Returns(order);

        var publisher = Substitute.For<IOrderStatusPublisher>();
        var handler = new AcceptOrderCommandHandler(repo, publisher);

        // Act
        var command = new OrderCommand { OrderId = order.Id, Status = OrderService.Contracts.Enums.AcceptOrRejectOrderEnum.Accepted };
        await handler.HandleAsync(command);

        // Assert
        Assert.Equal(OrderStatus.Accepted, order.Status);
        await repo.Received(1).UpdateAsync(order);
        await publisher.Received(1).PublishOrderStatusAsync(command.OrderId, command.Status);
    }
}
