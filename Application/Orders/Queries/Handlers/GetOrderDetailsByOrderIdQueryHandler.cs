using MediatR;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Orders.Queries.Handlers;

public class GetOrderDetailsByOrderIdQueryHandler : IRequestHandler<GetOrderDetailsByOrderIdQuery, List<Orderdetail>>
{
    private readonly IOrderDetailRepository _orderDetailRepository;

    public GetOrderDetailsByOrderIdQueryHandler(IOrderDetailRepository orderDetailRepository)
    {
        _orderDetailRepository = orderDetailRepository;
    }

    public async Task<List<Orderdetail>> Handle(GetOrderDetailsByOrderIdQuery request, CancellationToken cancellationToken)
    {
        return await _orderDetailRepository.GetByOrderIdAsync(request.OrderId);
    }
}