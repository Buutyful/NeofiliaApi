namespace NeofiliaDomain.Application.Services.Reward;
public interface IRewardService
{
    Task RedeemReward(Guid rewardId);
}



