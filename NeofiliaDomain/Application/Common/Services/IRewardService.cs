namespace NeofiliaDomain.Application.Common.Services;
public interface IRewardService
{
    Task RedeemReward(Guid rewardId);
}



