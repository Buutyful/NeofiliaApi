namespace NeofiliaDomain.Application.Common.Repositories;

public interface ITableRepository
{
    Task<List<PubTable>> Get();    
    Task<PubTable?> GetById(int id);
    Task Update(PubTable table);
}

public interface IRewardRepository
{
    Task<List<TableReward>> Get();
    Task Update(TableReward reward);
    Task Create(TableReward reward);
    Task<TableReward?> GetById(Guid id);    
}

