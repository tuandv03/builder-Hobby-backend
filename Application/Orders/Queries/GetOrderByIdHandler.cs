using MediatR;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Orders.Queries;

public class GetOrderByIdHandler : IRequestHandler<GetOrderByIdQuery, Order>
{
    private readonly IOrderRepository _repo;
    public GetOrderByIdHandler(IOrderRepository repo) => _repo = repo;

    public async Task<Order> Handle(GetOrderByIdQuery request, CancellationToken ct)
    {
        var order = await _repo.GetByIdAsync(request.Id);
        return order is null ? throw new KeyNotFoundException($"Order {request.Id} not found") : order;
    }
}