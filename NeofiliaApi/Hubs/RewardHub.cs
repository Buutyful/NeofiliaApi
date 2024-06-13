using Microsoft.AspNetCore.SignalR;
using NeofiliaDomain.Application.Common.Hubs;

namespace NeofiliaApi.Hubs;

public class RewardHub : Hub<IRewardHubClient>
{
    public const string HubUrl = "reward/rewardhub";    
}
public class RewardHubService(IHubContext<RewardHub, IRewardHubClient> hubContext) 
    : IRewardHubService
{
    private readonly IHubContext<RewardHub, IRewardHubClient> _hubContext = hubContext;

    public async Task NotifyRewardGenerated(int tableId)
    {
        //TODO: change this to table group
        await _hubContext.Clients.All.RewardGenerated(tableId);
    }

    public async Task NotifyRewardRedeemed(Guid rewardId)
    {
        //TODO: change this to table group
        await _hubContext.Clients.All.RewardRedeemed(rewardId);
    }
}
