using Mc2.Curd.Core.Events;

namespace Mc2.Curd.Core.Domains;

public interface IAggregateRoot<TIIdentity>
    where TIIdentity : IIdentity
{
    TIIdentity Id { get; }
    IList<IEvent<TIIdentity>> UncommittedEvents { get; }
    Task LoadEvents(IEventStore eventStore);
}