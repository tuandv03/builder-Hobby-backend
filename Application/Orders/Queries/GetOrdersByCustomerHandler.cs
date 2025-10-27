using MediatR;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Orders.Queries;

public class GetOrdersByCustomerHandler : IRequestHandler<GetOrdersByCustomerQuery, List<Order>>
{
    private readonly IOrderRepository _repo;
    public GetOrdersByCustomerHandler(IOrderRepository repo) => _repo = repo;

    public async Task<List<Order>> Handle(GetOrdersByCustomerQuery request, CancellationToken ct)
    {
        return await _repo.GetOrdersByCustomerAsync(request.CustomerName);
    }
}