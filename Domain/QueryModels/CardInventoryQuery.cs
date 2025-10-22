namespace Domain.QueryModels;

public class CardInventoryQuery
{
    public string? CardName { get; set; }
    public string? SetCode { get; set; }
    public string? Rarity { get; set; }
    public string? SetName { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}