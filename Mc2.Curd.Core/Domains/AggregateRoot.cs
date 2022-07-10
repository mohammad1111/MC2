using Mc2.Curd.Core.Events;
using Newtonsoft.Json;

namespace Mc2.Curd.Core.Domains;

public class AggregateRoot<TIIdentity> : IAggregateRoot<TIIdentity>
    where TIIdentity : IIdentity
{
    private readonly Dictionary<Type, Action<object>> _eventHandlers = new();

    public AggregateRoot(TIIdentity identity)
    {
    }

    public TIIdentity Id { get; private set; }

    [JsonIgnore] public IList<IEvent<TIIdentity>> UncommittedEvents { get; } = new List<IEvent<TIIdentity>>();

    [JsonIgnore] public IList<UniqueConstraint> UniqueConstraints { get; } = new List<UniqueConstraint>();

    public async Task LoadEvents(IEventStore eventStore)
    {
        IEnumerable<IEvent> domainEvents = await eventStore.LoadEvents(Id).ConfigureAwait(false);
        ApplyEvents(domainEvents);
    }

    internal void SetIdentity(TIIdentity identity)
    {
        Id = identity;
    }

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

    public void RegisterUniqueConstraint(UniqueConstraint uniqueConstraint)
    {
        UniqueConstraints.Add(uniqueConstraint);
    }

    public void Emit(IEvent<TIIdentity> @event)
    {
        UncommittedEvents.Add(@event);
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