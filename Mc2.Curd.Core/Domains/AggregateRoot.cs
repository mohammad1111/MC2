using Mc2.Curd.Core.Events;
using Newtonsoft.Json;

namespace Mc2.Curd.Core.Domains;

public class AggregateRoot<TIdentity> : IAggregateRoot<TIdentity>
    where TIdentity : IIdentity
{
    private readonly Dictionary<Type, Action<object>> _eventHandlers = new();

    public TIdentity Id { get; }

    [JsonIgnore] public IList<IEvent<TIdentity>> UncommittedEvents { get; }

    protected void Register<TAggregateEvent>(Action<TAggregateEvent> handler)
        where TAggregateEvent : IEvent
    {
        if (handler == null) throw new ArgumentNullException(nameof(handler));

        Type eventType = typeof(TAggregateEvent);
        if (_eventHandlers.ContainsKey(eventType))
            throw new ArgumentException(
                $"There's already a event handler registered for the  event '{eventType.Name}'");
        _eventHandlers[eventType] = e => handler((TAggregateEvent)e);
    }

    public void Emit(IEvent<TIdentity> @event)
    {
        UncommittedEvents.Add(@event);
    }

    public async Task LoadEvents(IEventStore eventStore)
    {
        var domainEvents = await eventStore.LoadEvents(Id).ConfigureAwait(false);
        ApplyEvents(domainEvents);
    }

    public void ApplyEvents(IEnumerable<IEvent> domainEvents)
    {
        if (domainEvents == null) throw new ArgumentNullException(nameof(domainEvents));

        foreach (IEvent domainEvent in domainEvents) ApplyEvent(domainEvent);
    }

    protected void ApplyEvent(IEvent @event)
    {
        if (@event == null) throw new ArgumentNullException(nameof(@event));

        Type eventType = @event.GetType();
        if (_eventHandlers.ContainsKey(eventType))
            _eventHandlers[eventType](@event);
        else
            throw new Exception("There is no handler for this event");
    }
}