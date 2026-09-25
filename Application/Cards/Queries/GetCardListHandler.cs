using Application.Cards.DTOs;
using Domain.Interfaces;
using Domain.QueryModels;
using MediatR;

namespace Application.Cards.Queries;

public class GetCardListHandler : IRequestHandler<GetCardListQuery, CardListResponseDto>
{
	private readonly ICardRepository _repo;

	public GetCardListHandler(ICardRepository repo) => _repo = repo;

	public async Task<CardListResponseDto> Handle(GetCardListQuery request, CancellationToken ct)
	{
		var cardListQuery = new CardListQuery
		{
			CardName = request.CardName?.Trim(),
			Rarity = request.Rarity,
			SetName = request.SetName,
			SetCode = request.SetCode,
			Type = request.Type,
			PriceMin = request.PriceMin,
			PriceMax = request.PriceMax,
			Page = Math.Max(1, request.Page),
			PageSize = Math.Clamp(request.PageSize, 1, 100)
		};

		var result = await _repo.GetCardList(cardListQuery, ct);
		var items = result.Items.Select(card =>
		{
			var firstSet = card.Cardsets.OrderBy(set => set.Id).FirstOrDefault();
			var firstImage = card.Cardimages.OrderBy(image => image.Id).FirstOrDefault();

			return new CardListItemDto(
				card.Id,
				card.Name,
				card.Type,
				firstImage?.ImageUrlSmall ?? firstImage?.ImageUrl,
				firstSet?.SetRarity,
				firstSet?.SetPrice);
		}).ToArray();

		var suggestion = result.SuggestedName is null || string.IsNullOrWhiteSpace(request.CardName)
			? null
			: new CardSearchSuggestionDto(request.CardName, result.SuggestedName);

		return new CardListResponseDto(
			items,
			new PaginationDto(cardListQuery.Page, cardListQuery.PageSize, result.Total),
			suggestion);
	}
}
