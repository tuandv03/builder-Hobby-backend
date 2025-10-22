using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.QueryModels
{
	public class CardListQuery : BaseQuery
	{
		public string CardName { get; set; }
		public string Rarity { get; set; }
		public string SetName { get; set; }
		public string SetCode { get; set; }
		public string Type { get; set; }
		public decimal? PriceMin { get; set; }
		public decimal? PriceMax { get; set; }

	}
}
