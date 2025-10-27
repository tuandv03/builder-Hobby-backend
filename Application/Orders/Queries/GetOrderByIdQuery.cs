using MediatR;
using Domain.Entities;

namespace Application.Orders.Queries;

public record GetOrderByIdQuery(long Id) : IRequest<Order>;