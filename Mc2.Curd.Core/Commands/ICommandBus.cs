using Mc2.Curd.Core.Domains;

namespace Mc2.Curd.Core.Commands;

public interface ICommandBus
{
    Task DispatchAsync<TIIdentity, TIAggregateRoot, TICommand>(TICommand command, TIIdentity identity)
        where TIIdentity : IIdentity
        where TICommand : ICommand
        where TIAggregateRoot : AggregateRoot<TIIdentity>;
}