using MediatR;
using Domain.Entities;

namespace Application.Orders.Queries;

public record GetOrderDetailsByOrderIdQuery(long OrderId) : IRequest<List<Orderdetail>>;