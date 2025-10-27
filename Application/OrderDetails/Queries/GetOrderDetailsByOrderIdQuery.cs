using MediatR;
using Domain.Entities;

namespace Application.OrderDetails.Queries;

public record GetOrderDetailsByOrderIdQuery(long OrderId) : IRequest<Orderdetail[]>;