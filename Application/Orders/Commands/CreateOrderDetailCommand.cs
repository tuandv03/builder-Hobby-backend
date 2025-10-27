using MediatR;
using Domain.Entities;

namespace Application.Orders.Commands;

public record CreateOrderDetailCommand(
    long OrderId,
    long InventoryId,
    long CardsetId,
    int Quantity,
    decimal UnitPrice,
    decimal? Subtotal
) : IRequest<Orderdetail>;