using NeofiliaDomain.DomainEvents;
using NeofiliaDomain.DomainEvents.Reward;
using System.ComponentModel.DataAnnotations.Schema;

namespace NeofiliaDomain;

public class PubTable
{
    private static readonly int _scoreCap = 100;    
    private int _currentScore = 0;

    private readonly List<Participant> _participants = [];
    private readonly List<TableReward> _rewardHistory = [];
    public int Id { get; private set; }
    public int PubId { get; private set; }
    public TableReward? CurrentTableReward { get; private set; }
    public IReadOnlyCollection<Participant> Partecipants => _participants.AsReadOnly();
    public IReadOnlyCollection<TableReward> RewardHistory => _rewardHistory.AsReadOnly();

    [ForeignKey("PubId")]
    public Pub Pub { get; private set; } = null!;

    [NotMapped]
    public int TableScore => _currentScore;
  

    public PubTable(int pubId)
    {
        PubId = pubId;
    }

    public async Task RedeemReward()
    {
        if (CurrentTableReward == null)
            throw new InvalidOperationException("No reward to redeem.");

        CurrentTableReward.Redeem();
        await Events.Raise(new RewardRedeemedEvent(CurrentTableReward.Id));
        CurrentTableReward = null;
    }

    //TODO: implement real score system
    public async Task UpdateScore(int value)
    {
        _currentScore += value;
        if (_currentScore >= _scoreCap)
        {
            await GenerateReward();
            _currentScore = 0;
        }
    }
    private async Task GenerateReward()
    {
        CurrentTableReward = new TableReward(this.Id);
        await Events.Raise(new RewardGeneratedEvent(this.Id));
    }

}
