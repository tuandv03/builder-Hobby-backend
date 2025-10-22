using Application.CardInventory.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CardInventoryController : ControllerBase
{
    private readonly IMediator _mediator;

    public CardInventoryController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get list of cards in inventory with optional filtering
    /// </summary>
    /// <param name="cardName">Filter by card name (partial match)</param>
    /// <param name="setCode">Filter by set code (exact match)</param>
    /// <param name="rarity">Filter by rarity (exact match)</param>
    /// <param name="setName">Filter by set name (partial match)</param>
    /// <param name="pageNumber">Page number (default: 1)</param>
    /// <param name="pageSize">Page size (default: 10)</param>
    /// <returns>List of card inventory items</returns>
    [HttpGet]
    public async Task<IActionResult> GetCardInventoryList(
        [FromQuery] string? cardName = null,
        [FromQuery] string? setCode = null,
        [FromQuery] string? rarity = null,
        [FromQuery] string? setName = null,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        try
        {
            var query = new GetCardInventoryListQuery(
                cardName, setCode, rarity, setName, pageNumber, pageSize);
            
            var result = await _mediator.Send(query);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest($"Error retrieving card inventory: {ex.Message}");
        }
    }
}