using Application.CardInventory.DTOs;
using AutoMapper;
using Domain.Interfaces;
using Domain.QueryModels;
using MediatR;

namespace Application.CardInventory.Queries;

public class GetCardInventoryListHandler : IRequestHandler<GetCardInventoryListQuery, CardInventoryDto[]>
{
    private readonly ICardInventoryRepository _repository;
    private readonly IMapper _mapper;

    public GetCardInventoryListHandler(ICardInventoryRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<CardInventoryDto[]> Handle(GetCardInventoryListQuery request, CancellationToken cancellationToken)
    {
        var query = new CardInventoryQuery
        {
            CardName = request.CardName,
            SetCode = request.SetCode,
            Rarity = request.Rarity,
            SetName = request.SetName,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };

        var inventories = await _repository.GetCardInventoryListAsync(query);
        return _mapper.Map<CardInventoryDto[]>(inventories);
    }
}