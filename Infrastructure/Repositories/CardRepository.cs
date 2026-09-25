using Domain.Entities;
using Domain.Interfaces;
using Domain.QueryModels;
using Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Search;

namespace Infrastructure.Repositories;

public class CardRepository : ICardRepository
{
	private readonly AppDbContext _db;
	public CardRepository(AppDbContext db) => _db = db;

	public async Task<Card?> GetByIdAsync(int id)
	{
		return await _db.Cards.FirstOrDefaultAsync(x => x.Id == id);
	}

	public async Task<Card?> GetCardDetailByIdAsync(int id)
	{
		string sqlQuery = @"WITH img AS (
                SELECT
                    c2.card_id,
                    JSON_AGG(
                        JSON_BUILD_OBJECT(
                            'image_url', c2.image_url,
                            'image_url_small', c2.image_url_small
                        )
                    ) AS card_images
                FROM cardimages c2
                GROUP BY c2.card_id
                ORDER BY c2.image_id
            ),
            sets AS (
                SELECT
                    c3.card_id,
                    JSON_AGG(
                        JSON_BUILD_OBJECT(
                            'set_name', c3.set_name,
                            'card_code', c3.card_code,
                            'set_rarity', c3.set_rarity
                        )
                    ) AS card_sets
                FROM cardsets c3
                GROUP BY c3.card_id
                ORDER BY c3.set_id, c3.set_rarity_code
            )
            SELECT
                c.id,
                c.""name"",
                c.""type"",
                c.human_readable_type,
                c.description desc,
                c.race,
                c.atk,
                c.def,
                c.""level"",
                c.""attribute"",
                c.archetype,
                c.frame_type,
                img.card_images,
                sets.card_sets
              FROM cards c
              LEFT JOIN img ON img.card_id = c.id
              LEFT JOIN sets ON sets.card_id = c.id
              WHERE c.id = " + id.ToString() + "; ";

		var result = await _db.Cards.FromSqlRaw(sqlQuery).ToListAsync();
		return result.FirstOrDefault();
	}
	public async Task<List<Card>> GetAllAsync() =>
		await _db.Cards.ToListAsync();

	public async Task<(List<Card> Items, int Total, string? SuggestedName)> GetCardList(
		CardListQuery request,
		CancellationToken cancellationToken = default)
	{
		var baseQuery = _db.Cards.AsNoTracking().AsQueryable();

		if (!string.IsNullOrEmpty(request.Rarity))
			baseQuery = baseQuery.Where(c => c.Cardsets.Any(set => set.SetRarity == request.Rarity));

		if (!string.IsNullOrEmpty(request.SetCode))
			baseQuery = baseQuery.Where(c => c.Cardsets.Any(set => set.SetCode == request.SetCode));

		if (!string.IsNullOrEmpty(request.SetName))
			baseQuery = baseQuery.Where(c => c.Cardsets.Any(set =>
				set.SetName != null && set.SetName.Contains(request.SetName)));

		if (!string.IsNullOrEmpty(request.Type))
			baseQuery = baseQuery.Where(c => c.Type == request.Type);

		if (request.PriceMin.HasValue)
			baseQuery = baseQuery.Where(c => c.Cardsets.Any(set => set.SetPrice >= request.PriceMin.Value));

		if (request.PriceMax.HasValue)
			baseQuery = baseQuery.Where(c => c.Cardsets.Any(set => set.SetPrice <= request.PriceMax.Value));

		var query = baseQuery;
		string? suggestedName = null;
		if (!string.IsNullOrWhiteSpace(request.CardName))
		{
			query = query.Where(card => card.Name.Contains(request.CardName));
			if (request.CardName.Trim().Length >= 2 && !await query.AnyAsync(cancellationToken))
			{
				var candidateNames = await baseQuery
					.Select(card => card.Name)
					.ToListAsync(cancellationToken);

				suggestedName = CardNameMatcher.FindClosest(request.CardName, candidateNames);
				query = suggestedName is null
					? query
					: baseQuery.Where(card => card.Name == suggestedName);
			}
		}

		var total = await query.CountAsync(cancellationToken);
		var items = await query
			.OrderBy(card => card.Name)
			.Skip((request.Page - 1) * request.PageSize)
			.Take(request.PageSize)
			.Include(card => card.Cardimages)
			.Include(card => card.Cardsets)
			.AsSplitQuery()
			.ToListAsync(cancellationToken);

		return (items, total, suggestedName);
	}
	public async Task AddAsync(Card card)
	{
		_db.Cards.Add(card);
		await _db.SaveChangesAsync();
	}
}
