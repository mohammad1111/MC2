using Mc2.Curd.Core.Domains;
using Mc2.Curd.Core.Events;

namespace Mc2.Curd.Core.Repositories;

public interface IRepository<TIAggregateRoot, TIIdentity>
    where TIAggregateRoot : IAggregateRoot<TIIdentity>
    where TIIdentity : IIdentity
{
    Task<TIAggregateRoot> GetByAsync(TIIdentity iIdentity);

    Task AddEvents(IList<InboxEvent> events);

    Task Commit();

    Task SetPublishDateInboxEvent(Guid correlationId);
}