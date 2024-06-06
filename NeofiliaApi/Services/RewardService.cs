using NeofiliaDomain.Application.Common.Repositories;
using NeofiliaDomain.Application.Common.Services;
using NeofiliaDomain.DomainEvents.Reward;
using NeofiliaDomain.DomainEvents;
using Microsoft.AspNetCore.SignalR;
using NeofiliaApi.Hubs;

namespace NeofiliaApi.Services;

public class RewardService : IRewardService
{
    private readonly ITableRepository _tableRepository;
    private readonly IRewardRepository _rewardRepository;    
    private readonly IHubContext<RewardHub> _hubContext;

    public RewardService(ITableRepository tableRepository,
                         IRewardRepository rewardRepository,
                         IHubContext<RewardHub> hubContext)
    {
        _tableRepository = tableRepository;
        _rewardRepository = rewardRepository;
        _hubContext = hubContext;

        Events.Register<RewardGeneratedEvent>(HandleRewardGeneratedEvent);
        Events.Register<RewardRedeemedEvent>(HandleRewardRedeemedEventAsync);
    }
    private async Task PersistTableAndRewardState(int tableId)
    {
        var table = await _tableRepository.GetById(tableId)
            ?? throw new ArgumentException("Invalid table ID");

        var reward = table.CurrentTableReward
             ?? throw new ArgumentException("reward not generated");

        await _rewardRepository.Save(reward);
        await _tableRepository.Save(table);
    }

    public async Task RedeemReward(Guid rewardId)
    {
        var reward = await _rewardRepository.GetById(rewardId)
            ?? throw new ArgumentException("Invalid reward ID");

        var table = await _tableRepository.GetById(reward.PubTableId)
            ?? throw new ArgumentException("Invalid table ID");

        table.RedeemReward();
        await _tableRepository.Save(table);
    }


    private async void HandleRewardGeneratedEvent(RewardGeneratedEvent domainEvent)
    {
        await PersistTableAndRewardState(domainEvent.TableId);
        await _hubContext.Clients.All.SendAsync("RewardGenerated", domainEvent.TableId);
    }

    private async void HandleRewardRedeemedEventAsync(RewardRedeemedEvent domainEvent)
    {
        await _hubContext.Clients.All.SendAsync("RewardRedeemed", domainEvent.RewardId);
    }
}
