using MediatR;
using Domain.Entities;

namespace Application.OrderDetails.Queries;

public record GetAllOrderDetailsQuery() : IRequest<Orderdetail[]>;