using Application.CardInventory.DTOs;
using MediatR;

namespace Application.CardInventory.Queries;

public record GetCardInventoryListQuery(
    string? CardName,
    string? SetCode,
    string? Rarity,
    string? SetName,
    int PageNumber = 1,
    int PageSize = 10
) : IRequest<CardInventoryDto[]>;