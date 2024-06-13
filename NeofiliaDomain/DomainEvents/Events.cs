namespace NeofiliaDomain.DomainEvents;

public interface IDomainEvent { }
public static class Events
{
    private static readonly List<Delegate> handlers = [];

    public static void Register<T>(Func<T, Task> callback) where T : IDomainEvent
    {
        handlers.Add(callback);
    }

    public static async Task Raise<T>(T domainEvent) where T : IDomainEvent
    {
        foreach (var handler in handlers.OfType<Func<T, Task>>())
        {
            await handler(domainEvent);
        }
    }
}
