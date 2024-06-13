namespace NeofiliaDomain.Application.Common.Hubs;

public interface IRewardHubClient
{   
    public Task RewardGenerated(int tableId);
    public Task RewardRedeemed(Guid rewardId);
}
public interface IRewardHubService
{
    Task NotifyRewardGenerated(int tableId);
    Task NotifyRewardRedeemed(Guid rewardId);
}