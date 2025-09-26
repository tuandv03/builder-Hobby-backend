using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class Cardimage
{
    public long Id { get; set; }

    public long CardId { get; set; }

    public string? ImageUrl { get; set; }

    public string? ImageUrlSmall { get; set; }

    public string? ImageUrlCropped { get; set; }

    public string? ImageId { get; set; }

    public virtual Card Card { get; set; } = null!;
}
