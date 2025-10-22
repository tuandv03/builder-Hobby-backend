using Application.Cards.Commands;
using Application.Cards.DTOs;
using AutoMapper;
using Domain.Entities;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Application.Cards.Mapping;
public class CardMappingProfile : Profile
{
	public CardMappingProfile()
	{
		// Command -> Entity
		CreateMap<CreateCardCommand, Card>();

		// Entity -> DTO
		CreateMap<Card, CardDto>()
			.ForMember(dest => dest.ImageUrl, opt =>
				opt.MapFrom(src => src.Cardimages != null && src.Cardimages.Any()
					? src.Cardimages.First().ImageUrl
					: null));

		CreateMap<Card, CardDetailDto>();
	}
}