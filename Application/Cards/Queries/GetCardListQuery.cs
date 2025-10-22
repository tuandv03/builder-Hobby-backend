using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Cards.Queries
{
	public record GetCardListQuery(string CardName, string Rarity
		, string SetCode, string SetName,string Type
		, decimal PriceMin, decimal PriceMax) : IRequest<Card[]>;
}
