using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Cards.DTOs
{
	public class CardDto
	{
		public string Name { get; set; } = null!;
		public string? Type { get; set; }
		public string? HumanReadableType { get; set; }
		public string? FrameType { get; set; }
		public string? Description { get; set; }
		public string? Race { get; set; }
		public string? Attribute { get; set; }
		public string? Archetype { get; set; }
		public string? ImageUrl { get; set; }
	}
}
