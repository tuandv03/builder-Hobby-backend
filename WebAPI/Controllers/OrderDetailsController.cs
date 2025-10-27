using MediatR;
using Microsoft.AspNetCore.Mvc;
using Application.OrderDetails.Queries;
using Domain.Entities;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrderDetailsController : ControllerBase
{
    private readonly IMediator _mediator;
    public OrderDetailsController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<Orderdetail[]>> GetAllOrderDetails()
    {
        var orderDetails = await _mediator.Send(new GetAllOrderDetailsQuery());
        return Ok(orderDetails);
    }

    [HttpGet("{orderDetailId:long}")]
    public async Task<ActionResult<Orderdetail>> GetOrderDetailById(long orderDetailId)
    {
        var orderDetail = await _mediator.Send(new GetOrderDetailByIdQuery(orderDetailId));
        return Ok(orderDetail);
    }

    [HttpGet("by-order/{orderId:long}")]
    public async Task<ActionResult<Orderdetail[]>> GetOrderDetailsByOrderId(long orderId)
    {
        var orderDetails = await _mediator.Send(new GetOrderDetailsByOrderIdQuery(orderId));
        return Ok(orderDetails);
    }

    [HttpGet("by-cardset/{cardsetId:long}")]
    public async Task<ActionResult<Orderdetail[]>> GetOrderDetailsByCardsetId(long cardsetId)
    {
        var orderDetails = await _mediator.Send(new GetOrderDetailsByCardsetIdQuery(cardsetId));
        return Ok(orderDetails);
    }
}