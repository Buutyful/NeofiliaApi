namespace NeofiliaDomain.DomainEvents;

public interface IDomainEvent { }
public static class Events
{
    private static readonly List<Delegate> handlers = [];

    public static void Register<T>(Action<T> callback) where T : IDomainEvent
    {
        handlers.Add(callback);
    }

    public static void Raise<T>(T domainEvent) where T : IDomainEvent
    {
        foreach (var handler in handlers.OfType<Action<T>>())
        {
            handler(domainEvent);
        }
    }
}
