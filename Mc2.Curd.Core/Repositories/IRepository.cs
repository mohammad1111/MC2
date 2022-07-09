using Mc2.Curd.Core.Domains;
using Mc2.Curd.Core.Events;

namespace Mc2.Curd.Core.Repositories;

public interface IRepository
{
    Task<IAggregateRoot<TIIdentity>> GetByAsync<TIIdentity>(TIIdentity iIdentity) where TIIdentity : IIdentity;

    Task SaveEvent<TIIdentity>(IList<InboxEvent> events) where TIIdentity : IIdentity;

    Task SetPublishDateInboxEvent(Guid correlationId);
}