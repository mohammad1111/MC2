using Mc2.Curd.Core.Events;

namespace Mc2.Curd.Core.Domains;

public interface IApply<in TIEvent> where TIEvent : IEvent
{
    void Apply(TIEvent @event);
}