using Domain.Entities;

namespace Domain.Interfaces;

public interface IOrderRepository
{
    Task<Order?> GetByIdAsync(long id);
    Task<Order?> GetOrderDetailByIdAsync(long id);
    Task<List<Order>> GetAllAsync();
    Task<List<Order>> GetOrdersByCustomerAsync(string customerName);
    Task AddAsync(Order order);
    Task UpdateAsync(Order order);
}