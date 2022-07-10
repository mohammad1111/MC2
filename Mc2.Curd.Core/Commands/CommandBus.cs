using Mc2.Curd.Core.Domains;

namespace Mc2.Curd.Core.Commands;

public class CommandBus : ICommandBus
{
    private readonly IServiceProvider _serviceProvider;

    public CommandBus(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task DispatchAsync<TIIdentity, TIAggregateRoot, TICommand>(TICommand command, TIIdentity identity)
        where TIIdentity : IIdentity
        where TICommand : ICommand
        where TIAggregateRoot : AggregateRoot<TIIdentity>
    {
        ICommandHandler<TIIdentity, TIAggregateRoot, TICommand> commandHandler =
            (ICommandHandler<TIIdentity, TIAggregateRoot, TICommand>)_serviceProvider.GetService(
                typeof(ICommandHandler<TIIdentity, TIAggregateRoot, TICommand>));
        await commandHandler.Handler(command, identity);
    }
}