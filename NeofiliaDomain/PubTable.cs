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
    public int TableScore
    {
        get => _currentScore;
        private set
        {
            _currentScore += value;
            if (_currentScore >= _scoreCap)
            {
                GenerateReward();
                _currentScore = 0;
            }
        }
    }

    public PubTable(int pubId)
    {
        PubId = pubId;
    }
    public void GenerateReward()
    {
        CurrentTableReward = new TableReward(this.Id);
        Events.Raise(new RewardGeneratedEvent(this.Id));
    }

    public void RedeemReward()
    {
        if (CurrentTableReward == null)
            throw new InvalidOperationException("No reward to redeem.");

        CurrentTableReward.Redeem();
        Events.Raise(new RewardRedeemedEvent(CurrentTableReward.Id));
        CurrentTableReward = null;
    }
}
