namespace NeofiliaDomain;

public class TableReward
{
    public Guid Id { get; private set; }
    public int PubTableId { get; private set; }
    public bool IsRedeemed { get; private set; }
    public DateTime GeneratedAt { get; private set; }
    public DateTime? RedeemedAt { get; private set; }
    public TableReward(int tableId)
    {
        Id = Guid.NewGuid();
        PubTableId = tableId;       
        IsRedeemed = false;
        GeneratedAt = DateTime.UtcNow;
    }

    public void Redeem()
    {
        if (IsRedeemed)
            throw new InvalidOperationException("Reward already redeemed.");

        IsRedeemed = true;
        RedeemedAt = DateTime.UtcNow;
    }
}


