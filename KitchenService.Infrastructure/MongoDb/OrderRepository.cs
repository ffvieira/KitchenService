using KitchenService.Application.Interfaces;
using KitchenService.Domain.Entities;
using KitchenService.Domain.Enums;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace KitchenService.Infrastructure.MongoDb;

public class OrderRepository : IOrderRepository
{
    private readonly IMongoCollection<Order> _collection;

    public OrderRepository(IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("MongoDb") ?? Environment.GetEnvironmentVariable("DATABASE_CONNECTION");
        var mongoClient = new MongoClient(connectionString);
        var database = mongoClient.GetDatabase("KitchenDb");
        _collection = database.GetCollection<Order>("Orders");
    }

    public async Task AddAsync(Order order)
    {
        await _collection.InsertOneAsync(order);
    }

    public async Task<Order?> GetByIdAsync(Guid orderId)
    {
        var order = await _collection.FindAsync(x => x.Id == orderId);
        return order.FirstOrDefault();
    }

    public async Task<IEnumerable<Order>> GetPendingOrdersAsync()
    {
        var result = await _collection.FindAsync(x => x.Status == OrderStatus.Pending);
        return result.ToList();
    }

    public async Task<IEnumerable<Order>> GetOrdersAsync()
    {
        var result = await _collection.FindAsync(x => x.Status == OrderStatus.Pending || x.Status == OrderStatus.Accepted || x.Status == OrderStatus.Rejected);
        return result.ToList();
    }

    public async Task UpdateAsync(Order order)
    {
        await _collection.ReplaceOneAsync(x => x.Id == order.Id, order, new ReplaceOptions { IsUpsert = true });
    }
}
