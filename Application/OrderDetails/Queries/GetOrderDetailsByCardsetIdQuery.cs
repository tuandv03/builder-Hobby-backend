using MediatR;
using Domain.Entities;

namespace Application.OrderDetails.Queries;

public record GetOrderDetailsByCardsetIdQuery(long CardsetId) : IRequest<Orderdetail[]>;