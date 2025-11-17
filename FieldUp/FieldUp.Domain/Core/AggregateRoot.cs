namespace FieldUp.Domain.Core;

public abstract class AggregateRoot<TId>
{
    public TId Id { get; protected set; }

    public int Version { get; internal set; } = 0;

    private readonly List<object> _uncommittedEvents = [];
    public IEnumerable<object> UncommittedEvents => _uncommittedEvents;

    protected void AddEvent(object @event)
    {
        _uncommittedEvents.Add(@event);

        //ApplyEvent(@event);
    }

    // protected void ApplyEvent(object @event)
    // {
    //     var method = this.GetType()
    //         .GetMethod("Apply", [@event.GetType()]);
    //
    //     if (method == null)
    //         throw new InvalidOperationException(
    //             $"Apply method not found for event {@event.GetType().Name}");
    //
    //     method.Invoke(this, [@event]);
    // }

    public void ClearUncommitted() => _uncommittedEvents.Clear();
}
