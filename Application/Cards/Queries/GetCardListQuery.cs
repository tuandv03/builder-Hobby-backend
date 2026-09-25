using Application.Cards.DTOs;
using MediatR;

namespace Application.Cards.Queries;

public record GetCardListQuery(
	string? CardName,
	string? Rarity,
	string? SetCode,
	string? SetName,
	string? Type,
	decimal? PriceMin,
	decimal? PriceMax,
	int Page = 1,
	int PageSize = 12) : IRequest<CardListResponseDto>;
