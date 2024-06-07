using NeofiliaDomain.Application.Common.Repositories;
using NeofiliaDomain.Application.Common.Services;
using NeofiliaDomain.DomainEvents.Reward;
using NeofiliaDomain.DomainEvents;
using Microsoft.AspNetCore.SignalR;
using NeofiliaApi.Hubs;

namespace NeofiliaApi.Services;

public class RewardService : IRewardService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly IHubContext<RewardHub> _hubContext;

    public RewardService(IServiceScopeFactory serviceScopeFactory,
                         IHubContext<RewardHub> hubContext)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _hubContext = hubContext;

        Events.Register<RewardGeneratedEvent>(HandleRewardGeneratedEvent);
        Events.Register<RewardRedeemedEvent>(HandleRewardRedeemedEventAsync);
    }
    private async Task PersistTableAndRewardState(int tableId)
    {
        using (var scope = _serviceScopeFactory.CreateScope())
        {
            var tableRepository = scope.ServiceProvider.GetRequiredService<ITableRepository>();
            var rewardRepository = scope.ServiceProvider.GetRequiredService<IRewardRepository>();
            var table = await tableRepository.GetById(tableId)
            ?? throw new ArgumentException("Invalid table ID");

            var reward = table.CurrentTableReward
                 ?? throw new ArgumentException("reward not generated");

            await rewardRepository.Create(reward);
            await tableRepository.Update(table);
        }
    }

    //called by a rewardcontroller
    public async Task RedeemReward(Guid rewardId)
    {
        using (var scope = _serviceScopeFactory.CreateScope())
        {
            var tableRepository = scope.ServiceProvider.GetRequiredService<ITableRepository>();
            var rewardRepository = scope.ServiceProvider.GetRequiredService<IRewardRepository>();
            var reward = await rewardRepository.GetById(rewardId)
            ?? throw new ArgumentException("Invalid reward ID");

            var table = await tableRepository.GetById(reward.PubTableId)
                ?? throw new ArgumentException("Invalid table ID");

            table.RedeemReward();
            await rewardRepository.Update(reward);
            await tableRepository.Update(table);
        }
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
