﻿using NeofiliaDomain.Application.Common.Repositories;
using NeofiliaDomain.DomainEvents.Reward;
using NeofiliaDomain.DomainEvents;
using Microsoft.AspNetCore.SignalR;
using NeofiliaApi.Hubs;
using NeofiliaDomain.Application.Services.Reward;

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

            table.RedeemReward();
            
           
            await rewardRepository.Update(reward);
            await tableRepository.Update(table);
        }
    }


    private async void HandleRewardGeneratedEventAsync(RewardGeneratedEvent domainEvent)
    {
        await PersistTableAndRewardState(domainEvent.TableId);
        await _hubContext.Clients.All.SendAsync("RewardGenerated", domainEvent.TableId);
    }

    private async void HandleRewardRedeemedEventAsync(RewardRedeemedEvent domainEvent)
    {
        await _hubContext.Clients.All.SendAsync("RewardRedeemed", domainEvent.RewardId);
    }
}
