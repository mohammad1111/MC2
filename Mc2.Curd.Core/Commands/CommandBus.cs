namespace Mc2.Curd.Core.Commands;

public interface ICommandBus
{
   Task DispatchAsync<TIIdentity, TICommand>(TICommand command, TIIdentity identity);
}