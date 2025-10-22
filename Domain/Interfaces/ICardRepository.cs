using Domain.Entities;
using Domain.QueryModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
	public interface ICardRepository
	{
		Task<Card?> GetByIdAsync(int id);
		Task<Card?> GetCardDetailByIdAsync(int id);
		Task<List<Card>> GetAllAsync();
		Task<List<Card>> GetCardList(CardListQuery query);
		Task AddAsync(Card card);
	}
}
