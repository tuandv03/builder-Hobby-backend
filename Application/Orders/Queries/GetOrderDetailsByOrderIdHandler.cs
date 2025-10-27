using MediatR;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Orders.Queries;

public class GetOrderDetailsByOrderIdHandler : IRequestHandler<GetOrderDetailsByOrderIdQuery, List<Orderdetail>>
{
    private readonly IOrderDetailRepository _repo;
    public GetOrderDetailsByOrderIdHandler(IOrderDetailRepository repo) => _repo = repo;

    public async Task<List<Orderdetail>> Handle(GetOrderDetailsByOrderIdQuery request, CancellationToken ct)
    {
        return await _repo.GetByOrderIdAsync(request.OrderId);
    }
}