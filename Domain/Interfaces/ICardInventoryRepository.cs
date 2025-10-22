using Domain.Entities;
using Domain.QueryModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
	public interface ICardInventoryRepository
	{
		Task<Cardinventory?> GetByIdAsync(int id);
		Task<List<Cardinventory>> GetCardInventoryListAsync(CardInventoryQuery query);
		Task AddAsync(Cardinventory card);
	}
}
