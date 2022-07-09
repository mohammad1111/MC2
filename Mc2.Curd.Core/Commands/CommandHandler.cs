using Mc2.Curd.Core.Domains;
using Mc2.Curd.Core.Events;
using Mc2.Curd.Core.Repositories;
using Newtonsoft.Json;

namespace Mc2.Curd.Core.Commands;

public abstract class CommandHandler<TIIdentity, TICommand> : ICommandHandler<TIIdentity, TICommand>
    where TIIdentity : IIdentity
    where TICommand : ICommand
{
    private readonly IRepository _repository;
    private readonly IEventBus _eventBus;
    private readonly IEventStore _eventStore;

    public CommandHandler(IRepository repository,IEventBus eventBus,IEventStore eventStore)
    {
        _repository = repository;
        _eventBus = eventBus;
        _eventStore = eventStore;
    }

    public async Task Handler(ICommand command, TIIdentity identity)
    {
        var aggregateRoot = await _repository.GetByAsync(identity);
        await aggregateRoot.LoadEvents(_eventStore);
                                       await HandlerAsync(aggregateRoot, command);
        var events = aggregateRoot.UncommittedEvents.Select(x => new InboxEvent
        {
            EventContent = JsonConvert.SerializeObject(x),
            EventType = x.GetType().AssemblyQualifiedName,
            EventId = x.CorrelationId,
            PublishTime = null
        }).ToList();
        await _repository.SaveEvent<TIIdentity>(events);
        
        //todo: this is for sample but in product mode must be change
        
        foreach (var @event in aggregateRoot.UncommittedEvents)
        {
            await _eventBus.Publish(@event);
            await _repository.SetPublishDateInboxEvent(@event.CorrelationId);
        }
    }

    public abstract Task HandlerAsync(IAggregateRoot<TIIdentity> aggregateRoot, ICommand command);
}