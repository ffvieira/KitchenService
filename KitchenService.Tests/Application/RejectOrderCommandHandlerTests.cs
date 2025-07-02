using KitchenService.Application.Commands;
using KitchenService.Application.Interfaces;
using KitchenService.Domain.Entities;
using KitchenService.Domain.Enums;
using NSubstitute;

namespace KitchenService.Tests.Application;

public class RejectOrderCommandHandlerTests
{
    [Fact]
    public async Task HandleAsync_Should_Set_Status_To_Rejected_And_Publish_Event()
    {
        // Arrange
        var guid = Guid.NewGuid();
        var order = new Order(guid, [new(Guid.NewGuid(), "X-Salada", "Sanduíche de carne com queijo e salada", 1)], "delivery", DateTime.UtcNow);
        var repo = Substitute.For<IOrderRepository>();
        repo.GetByIdAsync(order.Id).Returns(order);

        var pubMock = Substitute.For<IOrderStatusPublisher>();
        var handler = new RejectOrderCommandHandler(repo, pubMock);

        var command = new OrderCommand
        {
            OrderId = guid,
            Status = OrderService.Contracts.Enums.AcceptOrRejectOrderEnum.Rejected
        };

        // Act
        await handler.HandleAsync(command);

        // Assert
        Assert.Equal(OrderStatus.Rejected, order.Status);
        await repo.Received(1).UpdateAsync(order);
        await pubMock.Received(1).PublishOrderStatusAsync(command.OrderId, command.Status);
    }
}
