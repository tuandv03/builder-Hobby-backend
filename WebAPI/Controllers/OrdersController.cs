using MediatR;
using Microsoft.AspNetCore.Mvc;
using Application.Orders.Queries;
using Application.Orders.Commands;
using Application.OrderDetails.Queries;
using Domain.Entities;
using Domain.Enums;
using Domain.Enums.Extensions; // Thêm dòng này để sử dụng extension methods

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IMediator _mediator;
    public OrdersController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<List<Order>>> GetAllOrders()
    {
        var orders = await _mediator.Send(new GetAllOrdersQuery());
        return Ok(orders);
    }

    [HttpGet("{orderId:long}")]
    public async Task<ActionResult<Order>> GetOrderById(long orderId)
    {
        var order = await _mediator.Send(new GetOrderByIdQuery(orderId));
        return Ok(order);
    }

    [HttpGet("by-customer/{customerName}")]
    public async Task<ActionResult<List<Order>>> GetOrdersByCustomer(string customerName)
    {
        var orders = await _mediator.Send(new GetOrdersByCustomerQuery(customerName));
        return Ok(orders);
    }

    [HttpPost]
    public async Task<ActionResult<Order>> CreateOrder([FromBody] CreateOrderCommand command)
    {
        var order = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetOrderById), new { orderId = order.OrderId }, order);
    }

    [HttpPost("{orderId:long}/details")]
    public async Task<ActionResult<Orderdetail>> AddOrderDetail(long orderId, [FromBody] CreateOrderDetailCommand command)
    {
        command = command with { OrderId = orderId };
        var orderDetail = await _mediator.Send(command);
        return Ok(orderDetail);
    }

    [HttpPut("{orderId:long}/status")]
    public async Task<ActionResult<Order>> UpdateOrderStatus(long orderId, [FromBody] UpdateOrderStatusCommand command)
    {
        command = command with { OrderId = orderId };
        var order = await _mediator.Send(command);
        return Ok(order);
    }

    [HttpGet("statuses")]
    public ActionResult<object[]> GetOrderStatuses()
    {
        var statuses = Enum.GetValues<OrderStatusEnum>()
            .Select(status => new
            {
                Value = (int)status,
                Name = status.ToString(),
                DisplayName = status.GetDisplayName(), // Bây giờ sẽ hoạt động
                Description = status.GetDescription()  // Bây giờ sẽ hoạt động
            })
            .ToArray();

        return Ok(statuses);
    }

    [HttpGet("{orderId:long}/details")]
    public async Task<ActionResult<List<Orderdetail>>> GetOrderDetails(long orderId)
    {
        var orderDetails = await _mediator.Send(new Application.OrderDetails.Queries.GetOrderDetailsByOrderIdQuery(orderId));
        return Ok(orderDetails);
    }
}