using MediatR;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Cards.Queries;

public class GetCardByIdHandler : IRequestHandler<GetCardByIdQuery, Card>
{
	private readonly ICardRepository _repo;
	public GetCardByIdHandler(ICardRepository repo) => _repo = repo;

	public async Task<Card> Handle(GetCardByIdQuery request, CancellationToken ct)
	{
		var card = await _repo.GetByIdAsync(request.Id);
		if (card is null)
			throw new KeyNotFoundException($"Card {request.Id} not found");

		foreach (var image in card.Cardimages)
		{
			if (!string.IsNullOrWhiteSpace(image.ImageId))
			{
				var localUrl = $"/card_images/{image.ImageId}.jpg";
				image.ImageUrl = localUrl;
				image.ImageUrlSmall = localUrl;
				image.ImageUrlCropped = localUrl;
			}
		}

		return card;
	}

	public async Task<Card> Handle(GetCardDetailByIdQuery request, CancellationToken ct)
	{
		var card = await _repo.GetCardDetailByIdAsync(request.Id);
		return card is null ? throw new KeyNotFoundException($"Card {request.Id} not found") : card;
	}
}
