using Mc2.Curd.Core.Domains;

namespace Mc2.Curd.Core.Commands;

public interface ICommandHandler<TIIdentity,TICommand> 
    where TIIdentity:IIdentity
    where TICommand : ICommand
    
{
    Task HandlerAsync(IAggregateRoot<TIIdentity> aggregateRoot, ICommand command);
}