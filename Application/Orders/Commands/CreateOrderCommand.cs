using MediatR;
using Domain.Entities;
using Domain.Enums;

namespace Application.Orders.Commands;

public record CreateOrderCommand(
    string CustomerName,
    string? CustomerPhone,
    string? CustomerAddress,
    DateTime? OrderDate,
    OrderStatusEnum Status = OrderStatusEnum.Pending
) : IRequest<Order>;