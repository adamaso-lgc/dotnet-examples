using FieldUp.Domain.Core;

namespace FieldUp.Domain.ValueObjects;

public class Name : ValueObject
{
    public string FirstName { get; }
    public string LastName { get; }
    
    public Name(string firstName, string lastName)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentNullException(nameof(firstName));
            
        if (string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentNullException(nameof(lastName));
        
        FirstName = firstName.Trim();
        LastName = lastName.Trim();
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return FirstName;
        yield return LastName;
    }
}