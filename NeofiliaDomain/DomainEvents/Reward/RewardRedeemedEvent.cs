namespace NeofiliaDomain.DomainEvents.Reward;

public class RewardRedeemedEvent(Guid rewardId) : IDomainEvent
{
    public Guid RewardId { get; } = rewardId;
}
