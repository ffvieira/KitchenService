using KitchenService.Application.Commands;
using KitchenService.Application.Commands.NewCancelledOrder;
using KitchenService.Application.Interfaces;
using NSubstitute;
using Xunit;

namespace KitchenService.Tests.Application;

public class HandleNewOrderCommandHandlerTests
{
    [Fact]
    public async Task HandleAsync_Should_Add_Order_To_Repository()
    {
        // Arrange
        var repo = Substitute.For<IOrderRepository>();
        var handler = new NewOrderCommandHandler(repo);

        var command = new NewOrderCommand
        {
            OrderId = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow,
            DeliveryMethod = OrderService.Contracts.Enums.OrderMode.Delivery,
            Items =
            [
                new(Guid.NewGuid(), "Cheeseburger", "Burguer and cheese", 2)
            ]
        };

        // Act
        await handler.HandleAsync(command);

        // Assert
        await repo.Received(1).AddAsync(Arg.Any<Domain.Entities.Order>());
    }
}
