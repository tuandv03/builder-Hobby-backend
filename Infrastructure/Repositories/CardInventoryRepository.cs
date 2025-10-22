using Domain.Entities;
using Domain.Interfaces;
using Domain.QueryModels;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class CardInventoryRepository : ICardInventoryRepository
{
    private readonly AppDbContext _context;

    public CardInventoryRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Cardinventory?> GetByIdAsync(int id)
    {
        return await _context.Cardinventories
            .Include(ci => ci.Cardset)
                .ThenInclude(cs => cs.Card)
                    .ThenInclude(c => c.Cardimages)
            .FirstOrDefaultAsync(ci => ci.InventoryId == id);
    }

    public async Task<List<Cardinventory>> GetCardInventoryListAsync(CardInventoryQuery query)
    {
        var queryable = _context.Cardinventories
            .Include(ci => ci.Cardset)
                .ThenInclude(cs => cs.Card)
                    .ThenInclude(c => c.Cardimages)
            .AsQueryable();

        // Apply filters
        if (!string.IsNullOrEmpty(query.CardName))
        {
            queryable = queryable.Where(ci => ci.Cardset.CardName != null && 
                ci.Cardset.CardName.Contains(query.CardName));
        }

        if (!string.IsNullOrEmpty(query.SetCode))
        {
            queryable = queryable.Where(ci => ci.Cardset.SetCode == query.SetCode);
        }

        if (!string.IsNullOrEmpty(query.Rarity))
        {
            queryable = queryable.Where(ci => ci.Cardset.SetRarity == query.Rarity);
        }

        if (!string.IsNullOrEmpty(query.SetName))
        {
            queryable = queryable.Where(ci => ci.Cardset.SetName != null && 
                ci.Cardset.SetName.Contains(query.SetName));
        }

        // Apply pagination
        queryable = queryable
            .Skip((query.PageNumber - 1) * query.PageSize)
            .Take(query.PageSize);

        return await queryable.ToListAsync();
    }

    public async Task AddAsync(Cardinventory card)
    {
        _context.Cardinventories.Add(card);
        await _context.SaveChangesAsync();
    }
}