namespace Mc2.Curd.Core.Events;

public interface IEventStore
{
    Task<IEnumerable<IEvent>> LoadEvents<TIdentity>(TIdentity id);
}