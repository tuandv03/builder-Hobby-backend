using MediatR;
using Domain.Entities;

namespace Application.Orders.Queries;

public record GetOrdersByCustomerQuery(string CustomerName) : IRequest<List<Order>>;