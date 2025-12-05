using System.Text.Json.Serialization;

namespace FieldUp.Domain.Core;

public abstract class AggregateRoot<TId> : IAggregateRoot<TId>
{
    public TId Id { get; set; }

    public long Version { get; internal set; } = 0;

    [JsonIgnore] private readonly List<object> _uncommittedEvents = [];
    
    public IEnumerable<object> GetUncommittedEvents()
    {
        return _uncommittedEvents;
    }

    protected void AddEvent(object @event)
    {
        _uncommittedEvents.Add(@event);

        ApplyEvent(@event);
    }

    private void ApplyEvent(object @event)
    {
        var method = this.GetType()
            .GetMethod("Apply", [@event.GetType()]);
    
        if (method == null)
            throw new InvalidOperationException(
                $"Apply method not found for event {@event.GetType().Name}");
    
        method.Invoke(this, [@event]);
        
        Version++;
    }

    public void ClearUncommittedEvents() => _uncommittedEvents.Clear();
}
