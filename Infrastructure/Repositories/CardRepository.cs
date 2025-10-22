using Domain.Entities;
using Domain.Interfaces;
using Domain.QueryModels;
using Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

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

	public async Task<List<Card>> GetCardList(CardListQuery request)
	{
		var query = _db.Cards.Include(c=>c.Cardsets).AsQueryable();

		if (!string.IsNullOrEmpty(request.CardName))
			query = query.Where(c => c.Name.Contains(request.CardName));

		if (!string.IsNullOrEmpty(request.Rarity))
			query = query.Where(c => c.Rarity == request.Rarity);

		if (!string.IsNullOrEmpty(request.SetCode))
			query = query.Where(c => c.SetCode == request.SetCode);

		if (!string.IsNullOrEmpty(request.SetName))
			query = query.Where(c => c.SetName.Contains(request.SetName));

		if (!string.IsNullOrEmpty(request.Type))
			query = query.Where(c => c.Type == request.Type);

		if (request.PriceMin.HasValue)
			query = query.Where(c => c.Price >= request.PriceMin.Value);

		if (request.PriceMax.HasValue)
			query = query.Where(c => c.Price <= request.PriceMax.Value);

		return await query.ToListAsync();
	}
	public async Task AddAsync(Card card)
	{
		_db.Cards.Add(card);
		await _db.SaveChangesAsync();
	}
}
