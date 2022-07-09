using Mc2.Curd.Core.Domains;

namespace Mc2.Curd.Core.Events;

public interface IEvent
{
    Guid CorrelationId  { get; }
}

public interface IEvent<TIIdentity> : IEntity<TIIdentity>, IEvent
    where TIIdentity : IIdentity
{
}

