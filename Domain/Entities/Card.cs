using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class Card
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Type { get; set; }

    public string? HumanReadableType { get; set; }

    public string? FrameType { get; set; }

    public string? Description { get; set; }

    public string? Race { get; set; }

    public int? Atk { get; set; }

    public int? Def { get; set; }

    public int? Level { get; set; }

    public string? Attribute { get; set; }

    public string? Archetype { get; set; }

    public string? YgoprodeckUrl { get; set; }

    public virtual ICollection<Cardimage> Cardimages { get; set; } = new List<Cardimage>();

    public virtual ICollection<Cardset> Cardsets { get; set; } = new List<Cardset>();
	public string Rarity { get; set; }
	public string SetCode { get; set; }
	public string SetName { get; set; }
	public decimal? Price { get; set; }
}
