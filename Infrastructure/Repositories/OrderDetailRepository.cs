using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public sealed class OrderDetailRepository : IOrderDetailRepository
{
    private readonly AppDbContext _db;

    public OrderDetailRepository(AppDbContext db) => _db = db;

    public Task<Orderdetail?> GetByIdAsync(long id) =>
        _db.Orderdetails.FirstOrDefaultAsync(detail => detail.OrderDetailId == id);

    public Task<List<Orderdetail>> GetAllAsync() =>
        _db.Orderdetails.AsNoTracking().ToListAsync();

    public Task<List<Orderdetail>> GetByOrderIdAsync(long orderId) =>
        _db.Orderdetails
            .AsNoTracking()
            .Where(detail => detail.OrderId == orderId)
            .ToListAsync();

    public Task<List<Orderdetail>> GetByCardsetIdAsync(long cardsetId) =>
        _db.Orderdetails
            .AsNoTracking()
            .Where(detail => detail.CardsetId == cardsetId)
            .ToListAsync();

    public async Task AddAsync(Orderdetail orderDetail)
    {
        _db.Orderdetails.Add(orderDetail);
        await _db.SaveChangesAsync();
    }

    public async Task UpdateAsync(Orderdetail orderDetail)
    {
        _db.Orderdetails.Update(orderDetail);
        await _db.SaveChangesAsync();
    }
}
