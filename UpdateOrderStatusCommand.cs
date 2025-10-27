namespace Application.Orders.Commands;

public record UpdateOrderStatusCommand : IRequest<Order>
{
	public long OrderId { get; init; }
	public string Status { get; init; } = string.Empty;
}
