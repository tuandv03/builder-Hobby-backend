using MediatR;
using Domain.Entities;

namespace Application.OrderDetails.Queries;

public record GetOrderDetailByIdQuery(long OrderDetailId) : IRequest<Orderdetail>;