using FieldUp.Domain.Core;

namespace FieldUp.Domain.ValueObjects;

public class TimeRange : ValueObject
{
    public DateTimeOffset Start { get; }
    public DateTimeOffset End { get; }
    
    public TimeRange(DateTimeOffset start, DateTimeOffset end)
    {
        if (end <= start) throw new ArgumentException("End must be after start.");
        Start = start;
        End = end;
    }
    
    public bool Overlaps(TimeRange other)
    {
        return Start < other.End && other.Start < End;
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Start;
        yield return End;
    }
}