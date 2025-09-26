using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Persistence;

namespace Infrastructure.Repositories;

public class CardRepository : ICardRepository
{
	private readonly AppDbContext _db;
	public CardRepository(AppDbContext db) => _db = db;

	public async Task<Card?> GetByIdAsync(int id) =>
		await _db.Cards.FirstOrDefaultAsync(x => x.Id == id);

	public async Task<List<Card>> GetAllAsync() =>
		await _db.Cards.ToListAsync();

	public async Task AddAsync(Card card)
	{
		_db.Cards.Add(card);
		await _db.SaveChangesAsync();
	}
}
