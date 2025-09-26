using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class Cardset
{
    public long Id { get; set; }

    public long CardId { get; set; }

    public string? SetName { get; set; }

    public string? SetCode { get; set; }

    public string? SetRarity { get; set; }

    public string? SetRarityCode { get; set; }

    public decimal? SetPrice { get; set; }

    public string? CardName { get; set; }

    public string? CardCode { get; set; }

    public virtual Card Card { get; set; } = null!;

    public virtual ICollection<Cardinventory> Cardinventories { get; set; } = new List<Cardinventory>();

    public virtual ICollection<Orderdetail> Orderdetails { get; set; } = new List<Orderdetail>();
}
