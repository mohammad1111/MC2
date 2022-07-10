using Mc2.Curd.Core.Domains;

namespace Mc2.Curd.Core.Events;

public interface IEventBus
{
    Task Publish(IEvent @event);

    Task Publish<TIIdentity>(IEvent<TIIdentity> @event) where TIIdentity : IIdentity;
}