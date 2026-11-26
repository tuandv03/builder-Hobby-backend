using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public sealed class OrderRepository : IOrderRepository
{
    private readonly AppDbContext _db;

    public OrderRepository(AppDbContext db) => _db = db;

    public Task<Order?> GetByIdAsync(long id) =>
        _db.Orders.FirstOrDefaultAsync(order => order.OrderId == id);

    public Task<Order?> GetOrderDetailByIdAsync(long id) =>
        _db.Orders
            .Include(order => order.Orderdetails)
            .FirstOrDefaultAsync(order => order.OrderId == id);

    public Task<List<Order>> GetAllAsync() =>
        _db.Orders.AsNoTracking().ToListAsync();

    public Task<List<Order>> GetOrdersByCustomerAsync(string customerName) =>
        _db.Orders
            .AsNoTracking()
            .Where(order => order.CustomerName.Contains(customerName))
            .ToListAsync();

    public async Task AddAsync(Order order)
    {
        _db.Orders.Add(order);
        await _db.SaveChangesAsync();
    }

    public async Task UpdateAsync(Order order)
    {
        _db.Orders.Update(order);
        await _db.SaveChangesAsync();
    }
}
