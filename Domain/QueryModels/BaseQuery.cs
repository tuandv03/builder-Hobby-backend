namespace Domain.QueryModels
{
	public class BaseQuery
	{
		public int Id { get; set; }
		public int CardId { get; set; }
		public string CardCode { get; set; }
		public int PageIndex { get; set; } = 0;
		public int PageSize { get; set; } = 20;
	}
}