using MediatR;
using Domain.Entities;
using Application.Common.Interfaces; // Adjust based on your interfaces location

namespace Application.Orders.Queries.Handlers;

public class GetOrderDetailsByOrderIdQueryHandler : IRequestHandler<GetOrderDetailsByOrderIdQuery, Orderdetail[]>
{
    private readonly IRepository<Orderdetail> _orderDetailRepository;

    public GetOrderDetailsByOrderIdQueryHandler(IRepository<Orderdetail> orderDetailRepository)
    {
        _orderDetailRepository = orderDetailRepository;
    }

    public async Task<Orderdetail[]> Handle(GetOrderDetailsByOrderIdQuery request, CancellationToken cancellationToken)
    {
        return await _orderDetailRepository
            .GetAllAsync(
                filter: od => od.OrderId == request.OrderId,
                includes: new[] { "Cardset", "Inventory", "Order" },
                cancellationToken: cancellationToken
            );
    }
}