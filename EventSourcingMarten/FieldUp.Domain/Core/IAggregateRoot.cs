namespace FieldUp.Domain.Core;

public interface IAggregateRoot<out TId>
{
    TId Id { get; }
    
    long Version { get; }
    
    IEnumerable<object> GetUncommittedEvents(); 
    
    void ClearUncommittedEvents();
}