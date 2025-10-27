using Domain.Entities;

namespace Domain.Interfaces;

public interface IOrderDetailRepository
{
    Task<Orderdetail?> GetByIdAsync(long id);
    Task<List<Orderdetail>> GetAllAsync();
    Task<List<Orderdetail>> GetByOrderIdAsync(long orderId);
    Task<List<Orderdetail>> GetByCardsetIdAsync(long cardsetId);
    Task AddAsync(Orderdetail orderDetail);
    Task UpdateAsync(Orderdetail orderDetail);
}