using Domain.Entities;
using Domain.Interfaces;
using Domain.QueryModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Cards.Queries
{
	public class GetCardListHandler : IRequestHandler<GetCardListQuery, Card[]>
	{
		private readonly ICardRepository _repo;
		public GetCardListHandler(ICardRepository repo) => _repo = repo;

		public async Task<Card[]> Handle(GetCardListQuery request, CancellationToken ct)
		{
			var CardListQuery = new CardListQuery
			{
				CardName = request.CardName,
				Rarity = request.Rarity,
				SetName = request.SetName,
				SetCode = request.SetCode,
				PriceMin = request.PriceMin,
				PriceMax = request.PriceMax
			};
			var card = await _repo.GetCardList(CardListQuery);
			return card is null ? throw new KeyNotFoundException($"Card not found") : card.ToArray();
		}

		public async Task<Card> Handle(GetCardDetailByIdQuery request, CancellationToken ct)
		{
			var card = await _repo.GetCardDetailByIdAsync(request.Id);
			return card is null ? throw new KeyNotFoundException($"Card {request.Id} not found") : card;
		}
	}
}
