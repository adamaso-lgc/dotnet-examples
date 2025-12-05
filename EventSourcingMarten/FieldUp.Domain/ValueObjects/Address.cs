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
        if (string.IsNullOrWhiteSpace(line))
            throw new ArgumentException("Line cannot be empty.", nameof(line));
    
        if (string.IsNullOrWhiteSpace(city))
            throw new ArgumentException("City cannot be empty.", nameof(city));
    
        if (string.IsNullOrWhiteSpace(postalCode))
            throw new ArgumentException("Postal code cannot be empty.", nameof(postalCode));
    
        if (string.IsNullOrWhiteSpace(country))
            throw new ArgumentException("Country cannot be empty.", nameof(country));

        Line = line.Trim();
        City = city.Trim();
        PostalCode = postalCode.Trim();
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