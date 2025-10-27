using MediatR;
using Domain.Entities;

namespace Application.Orders.Queries;

public record GetAllOrdersQuery() : IRequest<List<Order>>;