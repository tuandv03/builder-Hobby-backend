using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class Orderdetail
{
    public long OrderDetailId { get; set; }

    public long OrderId { get; set; }

    public long InventoryId { get; set; }

    public long CardsetId { get; set; }

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public decimal? Subtotal { get; set; }

    public virtual Cardset Cardset { get; set; } = null!;

    public virtual Cardinventory Inventory { get; set; } = null!;

    public virtual Order Order { get; set; } = null!;
}
