using System.ComponentModel.DataAnnotations.Schema;

namespace NeofiliaDomain;

[NotMapped]
public class Participant
{
    public string ConnectionId { get; } = null!;
    public int PubTableId { get; private set; }
    public string NickName { get; private set; } = null!;
    public Participant(string connectionId, int pubTableId, string nickName)
    {
        ConnectionId = connectionId ?? throw new ArgumentNullException(nameof(connectionId));
        PubTableId = pubTableId;
        NickName = nickName ?? throw new ArgumentNullException(nameof(nickName));
    }
}