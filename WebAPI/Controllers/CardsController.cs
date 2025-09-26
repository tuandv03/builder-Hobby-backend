using MediatR;
using Microsoft.AspNetCore.Mvc;
using Application.Cards.Queries;
using Domain.Entities;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CardsController : ControllerBase
{
	private readonly IMediator _mediator;
	public CardsController(IMediator mediator) => _mediator = mediator;

	[HttpGet("{id:int}")]
	public async Task<ActionResult<Card>> GetById(int id)
	{
		var card = await _mediator.Send(new GetCardByIdQuery(id));
		return Ok(card);
	}
}
