using Mc2.Curd.Core.Events;

namespace Mc2.Curd.Core.Domains;

public interface IAggregateRoot
{
    Task LoadEvents(IEventStore eventStore);
}

public interface IAggregateRoot<TIIdentity> : IAggregateRoot
    where TIIdentity : IIdentity
{
    TIIdentity Id { get; }
    IList<IEvent<TIIdentity>> UncommittedEvents { get; }
    IList<UniqueConstraint> UniqueConstraints { get; }
}