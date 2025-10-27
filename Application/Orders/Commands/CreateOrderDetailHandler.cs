using MediatR;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Orders.Commands;

public class CreateOrderDetailHandler : IRequestHandler<CreateOrderDetailCommand, Orderdetail>
{
    private readonly IOrderDetailRepository _repo;
    public CreateOrderDetailHandler(IOrderDetailRepository repo) => _repo = repo;

    public async Task<Orderdetail> Handle(CreateOrderDetailCommand request, CancellationToken ct)
    {
        var orderDetail = new Orderdetail
        {
            OrderId = request.OrderId,
            InventoryId = request.InventoryId,
            CardsetId = request.CardsetId,
            Quantity = request.Quantity,
            UnitPrice = request.UnitPrice,
            Subtotal = request.Subtotal ?? (request.UnitPrice * request.Quantity)
        };

        await _repo.AddAsync(orderDetail);
        return orderDetail;
    }
}