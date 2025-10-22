namespace Application.CardInventory.DTOs;

public class CardInventoryDto
{
    public long InventoryId { get; set; }
    public long CardsetId { get; set; }
    public int Quantity { get; set; }
    public decimal? BuyPrice { get; set; }
    public decimal? SellPrice { get; set; }
    public DateTime? UpdatedAt { get; set; }
    
    // Card information from Cardset
    public string? CardName { get; set; }
    public string? SetName { get; set; }
    public string? SetCode { get; set; }
    public string? SetRarity { get; set; }
    public decimal? SetPrice { get; set; }
    
    // Card details from Card entity
    public string? Type { get; set; }
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
}