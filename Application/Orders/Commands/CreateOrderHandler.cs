using MediatR;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Orders.Commands;

public class CreateOrderHandler : IRequestHandler<CreateOrderCommand, Order>
{
    private readonly IOrderRepository _repo;
    public CreateOrderHandler(IOrderRepository repo) => _repo = repo;

    public async Task<Order> Handle(CreateOrderCommand request, CancellationToken ct)
    {
        var order = new Order
        {
            CustomerName = request.CustomerName,
            CustomerPhone = request.CustomerPhone,
            CustomerAddress = request.CustomerAddress,
            OrderDate = request.OrderDate ?? DateTime.Now,
            Status = request.Status.ToString()
        };

        await _repo.AddAsync(order);
        return order;
    }
}