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
        var connectionString = configuration.GetConnectionString("MongoDb")!;
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
        var filter = Builders<Order>.Filter.Eq(x => x.Id, orderId);
        return await _collection.Find(filter).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Order>> GetPendingOrdersAsync()
    {
        var filter = Builders<Order>.Filter.Eq(x => x.Status, OrderStatus.Pending);
        var result = await _collection.Find(filter).ToListAsync();
        return result;
    }

    public async Task UpdateAsync(Order order)
    {
        var filter = Builders<Order>.Filter.Eq(x => x.Id, order.Id);
        await _collection.ReplaceOneAsync(filter, order, new ReplaceOptions { IsUpsert = true });
    }
}
