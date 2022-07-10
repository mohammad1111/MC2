namespace Mc2.Curd.Core.Events;

public class InboxEvent
{
    public string EventType { get; set; }

    public string EventContent { get; set; }

    public Guid EventId { get; set; }

    public DateTime? PublishTime { get; set; }
}