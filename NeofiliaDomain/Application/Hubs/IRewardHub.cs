using Microsoft.AspNetCore.SignalR;

namespace NeofiliaDomain.Application.Hubs;

public interface IRewardHub
{
    public Task RewardGenerated(int tableId);
    public Task RewardRedeemed(Guid rewardId);
}
public class RewardHub : Hub<IRewardHub>
{
}