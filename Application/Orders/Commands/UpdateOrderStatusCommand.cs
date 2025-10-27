using MediatR;
using Domain.Entities;
using Domain.Enums;

namespace Application.Orders.Commands;

public record UpdateOrderStatusCommand(
    long OrderId,
    OrderStatusEnum NewStatus
) : IRequest<Order>;