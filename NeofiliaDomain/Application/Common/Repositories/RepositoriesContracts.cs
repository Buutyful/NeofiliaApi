namespace NeofiliaDomain.Application.Common.Repositories;

public interface ITableRepository
{
    Task<List<PubTable>> Get();
    Task Add(PubTable table);
    Task<PubTable?> GetById(int id);
    Task Save(PubTable table);
}

public interface IRewardRepository
{
    Task<List<TableReward>> Get();
    Task Add(TableReward reward);
    Task<TableReward?> GetById(Guid id);
    Task Save(TableReward reward);
}

