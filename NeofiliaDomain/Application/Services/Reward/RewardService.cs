using NeofiliaDomain.Application.Common.Repositories;
using NeofiliaDomain.DomainEvents.Reward;
using NeofiliaDomain.DomainEvents;
using NeofiliaDomain.Application.Services.Reward;
using NeofiliaDomain.Application.Common.Hubs;
using Microsoft.Extensions.DependencyInjection;

namespace NeofiliaApi.Services;

public class RewardService : IRewardService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public RewardService(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;

        Events.Register<RewardGeneratedEvent>(HandleRewardGeneratedEventAsync);
        Events.Register<RewardRedeemedEvent>(HandleRewardRedeemedEventAsync);
    }
    private async Task PersistTableAndRewardState(int tableId)
    {
        using (var scope = _serviceScopeFactory.CreateScope())
        {
            var tableRepository = scope.ServiceProvider.GetRequiredService<ITableRepository>();
            var rewardRepository = scope.ServiceProvider.GetRequiredService<IRewardRepository>();
            //get table
            var table = await tableRepository.GetById(tableId)
            ?? throw new ArgumentException("Invalid table ID");
            //get the currently created reward
            var reward = table.CurrentTableReward
                 ?? throw new ArgumentException("reward not generated");
            //create new reward entry
            await rewardRepository.Create(reward);
            //update current table
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
                ?? throw new ArgumentException("Reward shouldnt be null when reedemed");
            var table = await tableRepository.GetById(reward.PubTableId)
                ?? throw new ArgumentException("Invalid table ID");

            await table.RedeemReward();


            await rewardRepository.Update(reward);
            await tableRepository.Update(table);
        }
    }


    private async Task HandleRewardGeneratedEventAsync(RewardGeneratedEvent domainEvent)
    {
        await PersistTableAndRewardState(domainEvent.TableId);
        using (var scope = _serviceScopeFactory.CreateScope())
        {
            var hubService = scope.ServiceProvider.GetRequiredService<IRewardHubService>();
            await hubService.NotifyRewardGenerated(domainEvent.TableId);
        }
    }

    private async Task HandleRewardRedeemedEventAsync(RewardRedeemedEvent domainEvent)
    {
        using (var scope = _serviceScopeFactory.CreateScope())
        {
            var hubService = scope.ServiceProvider.GetRequiredService<IRewardHubService>();
            await hubService.NotifyRewardRedeemed(domainEvent.RewardId);
        }
    }
}
