using MediatR;
using Microsoft.AspNetCore.Mvc;
using Application.Cards.Queries;
using Application.Cards.DTOs;
using Domain.Entities;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CardsController : ControllerBase
{
	private readonly IMediator _mediator;
	public CardsController(IMediator mediator) => _mediator = mediator;

	[HttpGet()]
	public async Task<ActionResult<CardListResponseDto>> GetCardList(
		[FromQuery] string? name = null,
		[FromQuery] string? code = null,
		[FromQuery] string? type = null,
		[FromQuery] string? rarity = null,
		[FromQuery] int page = 1,
		[FromQuery] int pageSize = 12,
		CancellationToken cancellationToken = default)
	{
		if (page < 1 || pageSize is < 1 or > 100)
		{
			return BadRequest(new ProblemDetails
			{
				Title = "Invalid pagination",
				Detail = "page must be at least 1 and pageSize must be between 1 and 100."
			});
		}

		var result = await _mediator.Send(
			new GetCardListQuery(name, rarity, code, null, type, null, null, page, pageSize),
			cancellationToken);
		return Ok(result);
	}

	[HttpGet("{id:int}")]
	public async Task<ActionResult<Card>> GetById(int id)
	{
		var card = await _mediator.Send(new GetCardByIdQuery(id));
		return Ok(card);
	}
}
