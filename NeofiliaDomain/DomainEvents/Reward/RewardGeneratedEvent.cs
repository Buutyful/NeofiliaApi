namespace NeofiliaDomain.DomainEvents.Reward;

public class RewardGeneratedEvent(int tableId) : IDomainEvent
{
    public int TableId { get; } = tableId;
}
