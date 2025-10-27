using MediatR;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Data; // Adjust this based on your actual DbContext location

namespace Application.Orders.Queries.Handlers;

public class GetOrderDetailsByOrderIdQueryHandler : IRequestHandler<GetOrderDetailsByOrderIdQuery, Orderdetail[]>
{
    private readonly IApplicationDbContext _context; // Use interface if available, or replace with your actual DbContext type

    public GetOrderDetailsByOrderIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Orderdetail[]> Handle(GetOrderDetailsByOrderIdQuery request, CancellationToken cancellationToken)
    {
        return await _context.Orderdetails
            .Include(od => od.Cardset)
            .Include(od => od.Inventory)
            .Include(od => od.Order)
            .Where(od => od.OrderId == request.OrderId)
            .ToArrayAsync(cancellationToken);
    }
}