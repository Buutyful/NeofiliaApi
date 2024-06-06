using System.ComponentModel.DataAnnotations;

namespace NeofiliaDomain;

public class Pub
{
    private Pub() { }
    private readonly List<PubTable> _tables = [];
    public int Id { get; private set; }

    [Required, EmailAddress]
    public string Email { get; private set; } = null!;

    [Required, Phone]
    public string PhoneNumber { get; private set; } = null!;

    public IReadOnlyCollection<PubTable> Tables => _tables.AsReadOnly();

    public Pub(string email, string phoneNumber)
    {
        Email = email ?? throw new ArgumentNullException(nameof(email));
        PhoneNumber = phoneNumber ?? throw new ArgumentNullException(nameof(phoneNumber));
    }

}
