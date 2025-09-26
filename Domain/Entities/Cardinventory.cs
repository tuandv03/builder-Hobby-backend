using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class Cardinventory
{
    public long InventoryId { get; set; }

    public long CardsetId { get; set; }

    public int Quantity { get; set; }

    public decimal? BuyPrice { get; set; }

    public decimal? SellPrice { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Cardset Cardset { get; set; } = null!;

    public virtual ICollection<Orderdetail> Orderdetails { get; set; } = new List<Orderdetail>();
}
