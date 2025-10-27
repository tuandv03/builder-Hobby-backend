using MediatR;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Orders.Queries;

public class GetAllOrdersHandler : IRequestHandler<GetAllOrdersQuery, List<Order>>
{
    private readonly IOrderRepository _repo;
    public GetAllOrdersHandler(IOrderRepository repo) => _repo = repo;

    public async Task<List<Order>> Handle(GetAllOrdersQuery request, CancellationToken ct)
    {
        return await _repo.GetAllAsync();
    }
}