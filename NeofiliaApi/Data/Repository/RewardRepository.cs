using Microsoft.EntityFrameworkCore;
using NeofiliaDomain;
using NeofiliaDomain.Application.Common.Repositories;

namespace NeofiliaApi.Data.Repository;

public class RewardRepository : IRewardRepository
{
    private readonly ApplicationDbContext _context;

    public RewardRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Add(TableReward reward)
    {
        await _context.Rewards.AddAsync(reward);
        await _context.SaveChangesAsync();
    }

    public async Task<List<TableReward>> Get()
    {
        var rewards = await _context.Rewards.ToListAsync();
        return rewards ?? [];
    }

    public async Task<TableReward?> GetById(Guid id)
    {
        var reward = await _context.Rewards.FindAsync(id);
        return reward;
    }

    public Task Save(TableReward reward)
    {
        throw new NotImplementedException();
    }
}
