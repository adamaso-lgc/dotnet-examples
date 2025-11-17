using FieldUp.Domain.Core;

namespace FieldUp.Domain.ValueObjects;

public class Address : ValueObject
{
    public string Line { get; }
    public string City { get; }
    public string PostalCode { get; }
    public string Country { get; }
    
    public Address(string line, string city, string postalCode, string country = "Portugal")
    {
        Line = line.Trim() ?? throw new ArgumentException("Line required");
        City = city.Trim() ?? throw new ArgumentException("City required");
        PostalCode = postalCode.Trim() ?? throw new ArgumentException("Postal code required");
        Country = country.Trim();
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Line;
        yield return City;
        yield return PostalCode;
        yield return Country;
    }
}