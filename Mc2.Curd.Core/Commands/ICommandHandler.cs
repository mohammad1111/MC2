using Mc2.Curd.Core.Domains;

namespace Mc2.Curd.Core.Commands;

internal interface ICommandHandler<TIIdentity, TIAggregateRoot, TICommand>
    where TIIdentity : IIdentity
    where TICommand : ICommand
    where TIAggregateRoot : IAggregateRoot
{
    Task Handler(ICommand command, TIIdentity identity);

    Task HandlerAsync(TIAggregateRoot aggregateRoot, ICommand command);
}