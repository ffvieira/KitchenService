using KitchenService.Application.Commands;
using KitchenService.Application.Interfaces;
using KitchenService.Domain.ValueObjects;
using Moq;
using Xunit;

namespace KitchenService.UnitTests.Commands;

public class HandleNewOrderCommandHandlerTests
{
    [Fact]
    public async Task HandleAsync_Should_Add_Order_To_Repository()
    {
        // Arrange
        var mockRepo = new Mock<IOrderRepository>();
        var handler = new HandleNewOrderCommandHandler(mockRepo.Object);

        var command = new HandleNewOrderCommand
        {
            OrderId = "123",
            CreatedAt = DateTime.UtcNow,
            DeliveryMethod = "drive-thru",
            Items = new List<OrderItem>
            {
                new("burger01", "Cheeseburger", "Burguer and cheese", 2)
            }
        };

        // Act
        await handler.HandleAsync(command);

        // Assert
        mockRepo.Verify(r => r.AddAsync(It.IsAny<Domain.Entities.Order>()), Times.Once);
    }
}
