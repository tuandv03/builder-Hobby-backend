using MediatR;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Cards.Queries;

public record GetCardByIdQuery(int Id) : IRequest<Card>;

public class GetCardByIdHandler : IRequestHandler<GetCardByIdQuery, Card>
{
	private readonly ICardRepository _repo;
	public GetCardByIdHandler(ICardRepository repo) => _repo = repo;

	public async Task<Card> Handle(GetCardByIdQuery request, CancellationToken ct)
	{
		var card = await _repo.GetByIdAsync(request.Id);
		if (card is null) throw new KeyNotFoundException($"Card {request.Id} not found");
		return card;
	}
}
