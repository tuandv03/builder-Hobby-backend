using Application.CardInventory.DTOs;
using AutoMapper;
using Domain.Entities;

namespace Application.CardInventory.Mapping;

public class CardInventoryMappingProfile : Profile
{
    public CardInventoryMappingProfile()
    {
        CreateMap<Cardinventory, CardInventoryDto>()
            .ForMember(dest => dest.CardName, opt => opt.MapFrom(src => src.Cardset.CardName))
            .ForMember(dest => dest.SetName, opt => opt.MapFrom(src => src.Cardset.SetName))
            .ForMember(dest => dest.SetCode, opt => opt.MapFrom(src => src.Cardset.SetCode))
            .ForMember(dest => dest.SetRarity, opt => opt.MapFrom(src => src.Cardset.SetRarity))
            .ForMember(dest => dest.SetPrice, opt => opt.MapFrom(src => src.Cardset.SetPrice))
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Cardset.Card.Type))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Cardset.Card.Description))
            .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => 
                src.Cardset.Card.Cardimages != null && src.Cardset.Card.Cardimages.Any() 
                    ? src.Cardset.Card.Cardimages.First().ImageUrl 
                    : null));
    }
}