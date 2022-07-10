using Mc2.Curd.Core.Domains;
using Mc2.Curd.Core.Events;
using Mc2.Curd.Core.Repositories;
using Mc2.Curd.Core.UniqueConstraints;
using Mc2.Curd.Core.UOW;
using Newtonsoft.Json;

namespace Mc2.Curd.Core.Commands;

public abstract class
    CommandHandler<TIIdentity, TIAggregateRoot, TICommand> : ICommandHandler<TIIdentity, TIAggregateRoot, TICommand>
    where TIIdentity : IIdentity
    where TICommand : ICommand
    where TIAggregateRoot : AggregateRoot<TIIdentity>, new()
{
    private readonly IUniqueConstraintService _uniqueConstraintService;
    private readonly IEventBus _eventBus;
    private readonly IEventStore _eventStore;
    private readonly IUow _uow;
    private readonly IRepository<TIAggregateRoot, TIIdentity> _repository;

    public CommandHandler(IUow uow,IRepository<TIAggregateRoot, TIIdentity> repository, IEventBus eventBus,
        IEventStore eventStore, IUniqueConstraintService uniqueConstraintService)
    {
        _uow = uow;
        _repository = repository;
        _eventBus = eventBus;
        _eventStore = eventStore;
        _uniqueConstraintService = uniqueConstraintService;
    }

    public async Task Handler(ICommand command, TIIdentity identity)
    {
        TIAggregateRoot aggregateRoot = await _repository.GetByAsync(identity);
        if (aggregateRoot == null)
        {
            aggregateRoot = new TIAggregateRoot();
            aggregateRoot.SetIdentity(identity);
        }
        else
        {
            await aggregateRoot.LoadEvents(_eventStore);
        }

        await HandlerAsync(aggregateRoot, command);
        List<InboxEvent> events = aggregateRoot.UncommittedEvents.Select(x => new InboxEvent
        {
            EventContent = JsonConvert.SerializeObject(x),
            EventType = x.GetType().AssemblyQualifiedName,
            EventId = x.CorrelationId,
            PublishTime = null
        }).ToList();

        await _repository.AddEvents(events);
        await _uniqueConstraintService.AddUniqueConstraints(aggregateRoot.UniqueConstraints);

        await _uow.Commit();
        
        //todo: this is for sample but in product mode must be change
        foreach (IEvent<TIIdentity> @event in aggregateRoot.UncommittedEvents)
        {
            await _eventBus.Publish(@event);
            await _repository.SetPublishDateInboxEvent(@event.CorrelationId);
        }
    }


    public abstract Task HandlerAsync(TIAggregateRoot aggregateRoot, ICommand command);
}